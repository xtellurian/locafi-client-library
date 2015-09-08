using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.Errors;
using Locafi.Client.Model.Responses;
using Newtonsoft.Json;

namespace Locafi.Client.Repo
{
    public abstract class WebRepo: IWebRepoErrorHandler
    {
        private readonly IHttpTransferConfigService _unauthorizedConfigService;
        private readonly string _service;
        private readonly IAuthorisedHttpTransferConfigService _authorisedUnauthorizedConfigService;
        private readonly ISerialiserService _serialiser;

        protected WebRepo(IAuthorisedHttpTransferConfigService authorisedUnauthorizedConfigService, ISerialiserService serialiser, string service) 
            : this(serialiser, service) // this as error handler, authorised base
        {
            _authorisedUnauthorizedConfigService = authorisedUnauthorizedConfigService;
            _unauthorizedConfigService = authorisedUnauthorizedConfigService;
        }

        protected WebRepo(IHttpTransferConfigService unauthorizedConfigService, ISerialiserService serialiser, string service) 
            : this(serialiser, service) // internal error handler, unauth
        {
            _unauthorizedConfigService = unauthorizedConfigService;
        }

        private WebRepo(ISerialiserService serialiser, string service) // base ctor
        {
            _serialiser = serialiser;
            _service = service;
        }

        protected async Task<T> Get<T>(string extra = "")
        {
            var response = await GetResponse(HttpMethod.Get, extra);
            if (!response.IsSuccessStatusCode) return default(T);
            var data = await response.Content.ReadAsStringAsync();
            if (typeof (T).GetTypeInfo().IsValueType)
            {
                T result = (T)Convert.ChangeType(data, typeof (T));
                if (result == null) await HandlePrivate(response);
                return result;
            }
            else
            {
                var result = _serialiser.Deserialise<T>(data);
                if (result == null) await HandlePrivate(response);
                return result;
            }
        }

        protected async Task<T> Post<T>(object data, string extra = "") where T : class, new()
        {
            var response = await GetResponse(HttpMethod.Post, extra, _serialiser.Serialise(data));
            var result = response.IsSuccessStatusCode ? _serialiser.Deserialise<T>(await response.Content.ReadAsStringAsync()) : null;
            if(result==null) await HandlePrivate(response);
            return result;
        }

        //protected async Task Delete(string key)
        //{
        //    var response = await GetResponse(HttpMethod.Delete, key);
        //    Debug.WriteLine(response.IsSuccessStatusCode
        //        ? $"{_service} service deleted  id={key} successfully"
        //        : $"{_service} service failed to delete id={key}");
        //}

        protected async Task<bool> Delete(string extra)
        {
            var response = await GetResponse(HttpMethod.Delete, extra);
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
            var baseUrl = _unauthorizedConfigService.BaseUrl;
            var path = GetFullPath(baseUrl, _service, extra);
            var message = new HttpRequestMessage(method, path);
            if(content!=null) message.Content = new StringContent(content, Encoding.UTF8, "application/json");

            //message.Content.Headers.Add("Content-Type", new List<string> { "application/json" });
            if(_authorisedUnauthorizedConfigService!=null) message.Headers.Add("Authorization", "Token " + _authorisedUnauthorizedConfigService.GetTokenString());

            var client = new HttpClient();
            Debug.WriteLine($"{method} request at {path}");
            if(content!=null) Debug.WriteLine($"Payload:\n {content}");
            var response = await client.SendAsync(message);
            var serverMessage = await response.Content.ReadAsStringAsync();
            Debug.WriteLine(response.IsSuccessStatusCode
                ? $"{method} request success at {path}"
                : $"Error executing {method} request at {path}");
            Debug.WriteLine($"Response from server:\n{serverMessage}");
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
