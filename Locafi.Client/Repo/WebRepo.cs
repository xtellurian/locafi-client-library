using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.ErrorHandlers;
using Locafi.Client.Contract.Http;
using Locafi.Client.Exceptions;
using Locafi.Client.Model.Responses;
using Newtonsoft.Json;

namespace Locafi.Client.Repo
{
    public abstract class WebRepo: IWebRepoErrorHandler
    {
        private readonly IHttpTransferConfigService _unauthorizedConfigService;
        private readonly string _service;
        private IAuthorisedHttpTransferConfigService _authorisedUnauthorizedConfigService;
        private readonly IHttpTransferer _transferer;
        private readonly ISerialiserService _serialiser;

        protected WebRepo(IHttpTransferer transferer, IAuthorisedHttpTransferConfigService authorisedUnauthorizedConfigService, ISerialiserService serialiser, string service) 
            : this(transferer, serialiser, service) // this as error handler, authorised base
        {
            _authorisedUnauthorizedConfigService = authorisedUnauthorizedConfigService;
            _unauthorizedConfigService = authorisedUnauthorizedConfigService;
        }

        protected WebRepo(IHttpTransferer transferer, IHttpTransferConfigService unauthorizedConfigService, ISerialiserService serialiser, string service) 
            : this(transferer, serialiser, service) // internal error handler, unauth
        {
            _unauthorizedConfigService = unauthorizedConfigService;
        }

        private WebRepo(IHttpTransferer transferer, ISerialiserService serialiser, string service) // base ctor
        {
            _transferer = transferer;
            _serialiser = serialiser;
            _service = service;
        }

        protected async Task<T> Get<T>(string extra = "")
        {
            var response = await GetResponse(HttpMethod.Get, extra);
            // try to reauthorise
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                response = await TryReauth(o => GetResponse(HttpMethod.Get, extra));
            }
            // if it didn't work
            if (!response.IsSuccessStatusCode)
            {
                return default(T);
            }
            // get payload data
            var data = await response.Content.ReadAsStringAsync();
            // if we are getting a value type
            if (typeof (T).GetTypeInfo().IsValueType)
            {
                T result = (T)Convert.ChangeType(data, typeof (T));
                if (result == null) await HandlePrivate(response);
                return result;
            }
            // we are getting a reference type
            else
            {
                var result = _serialiser.Deserialise<T>(data);
                if (result == null) await HandlePrivate(response);
                return result;
            }
        }

        protected async Task<T> Post<T>(object data, string extra = "") where T : class, new()
        {
            var serialisedData = _serialiser.Serialise(data); // serialise
            var response = await GetResponse(HttpMethod.Post, extra, serialisedData); // get response
            // try to reauthorise
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                response = await TryReauth(o => GetResponse(HttpMethod.Post, extra, serialisedData));
            }

            var result = response.IsSuccessStatusCode ? _serialiser.Deserialise<T>(await response.Content.ReadAsStringAsync()) : null;
            if(result==null) await HandlePrivate(response);
            return result;
        }

        protected async Task<bool> Delete(string extra)
        {
            var response = await GetResponse(HttpMethod.Delete, extra);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                response = await TryReauth(o => GetResponse(HttpMethod.Delete, extra));
            }
            Debug.WriteLine(response.IsSuccessStatusCode
                ? $"{_service} service deleted  extra={extra} successfully"
                : $"{_service} service failed to delete id={extra}");
            if (response.IsSuccessStatusCode)
            {
                var result = _serialiser.Deserialise<bool>(await response.Content.ReadAsStringAsync());
                return result;
            }
            else await HandlePrivate(response);
            return false; // probably is never called, but is required for compilation
        }

        private async Task<HttpResponseMessage> TryReauth(Func<object, Task<HttpResponseMessage>> resourceGetter)
        {
            var handler = _authorisedUnauthorizedConfigService.OnUnauthorised;
            if (handler != null)
            {
                _authorisedUnauthorizedConfigService = await handler(_unauthorizedConfigService);
                var response = await resourceGetter(null);
                if(response.StatusCode== HttpStatusCode.Unauthorized) throw new WebRepoUnauthorisedException();
                return response;
            }
            else
            {
                throw new WebRepoUnauthorisedException();
            }
        }

        private async Task HandlePrivate(HttpResponseMessage response)
        {
            try
            {
                var errors =
                    _serialiser.Deserialise<IList<CustomResponseMessage>>(await response.Content.ReadAsStringAsync());
                await this.Handle(errors, response.StatusCode);
            }
            catch(JsonException)
            {
                await Handle(response);
            }
        }

        

        private async Task<HttpResponseMessage> GetResponse(HttpMethod method, string extra = "", string content = null)
        {
            var baseUrl = await _unauthorizedConfigService.GetBaseUrlAsync();
            
            var path = GetFullPath(baseUrl, _service, extra);
            var token = await _authorisedUnauthorizedConfigService.GetTokenStringAsync();
            var response = await _transferer.GetResponse(method, path, content, token);
            return response;
        }

        private string GetFullPath(string baseUrl, string first, string second)
        {
            var result = new StringBuilder(baseUrl.TrimEnd('/'));
            var s1 = first.Trim('/');
            var isKeySelection = second.StartsWith("(");
            var isQuery = second.StartsWith("?");
            var s2 = second.Trim('/', '(', ')');
            if (isQuery) result.Append('/').Append(s1).Append(s2);
            else if (isKeySelection) result.Append('/').Append(s1).Append('(').Append(s2).Append(')');
            else result.Append('/').Append(s1).Append('/').Append(s2);
            return result.ToString();
        }

        public abstract Task Handle(IEnumerable<CustomResponseMessage> serverMessages, HttpStatusCode statusCode);

        public abstract Task Handle(HttpResponseMessage response);
    }
}
