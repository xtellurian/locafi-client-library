﻿using System;
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
using Locafi.Client.Model.Dto.Authentication;
using Locafi.Client.Model.Dto.ErrorLogs;
using Locafi.Client.Model.RelativeUri;
using Locafi.Client.Model.Responses;
using Newtonsoft.Json;

namespace Locafi.Client.Repo
{
    public abstract class WebRepo: IWebRepoErrorHandler
    {
        private readonly IHttpTransferConfigService _configService;
        private readonly string _service;
        private IAuthorisedHttpTransferConfigService _authorisedConfigService;
        private readonly IHttpTransferer _transferer;
        private readonly ISerialiserService _serialiser;

        protected WebRepo(IHttpTransferer transferer, IAuthorisedHttpTransferConfigService authorisedConfigService, ISerialiserService serialiser, string service) 
            : this(transferer, serialiser, service) // this as error handler, authorised base
        {
            _authorisedConfigService = authorisedConfigService;
            _configService = authorisedConfigService;
        }

        protected WebRepo(IHttpTransferer transferer, IHttpTransferConfigService configService, ISerialiserService serialiser, string service) 
            : this(transferer, serialiser, service) // internal error handler, unauth
        {
            _configService = configService;
        }

        private WebRepo(IHttpTransferer transferer, ISerialiserService serialiser, string service) // base ctor
        {
            _transferer = transferer;
            _serialiser = serialiser;
            _service = service;
        }

        protected async Task<T> Get<T>(string extra = "") where T : new()
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
                await HandlePrivate(response, "GET", extra, "");
            }
            // get payload data
            var data = await response.Content.ReadAsStringAsync();
            // if we are getting a value type
            if (typeof (T).GetTypeInfo().IsValueType)
            {
                T result = (T)Convert.ChangeType(data, typeof (T));
                if (result == null) await HandlePrivate(response, "GET", extra, "");
                return result;
            }
            // we are getting a reference type
            else
            {
                var result = _serialiser.Deserialise<T>(data);
                if (result == null) await HandlePrivate(response, "GET", extra, "");
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
            if(result==null) await HandlePrivate(response, "POST",  extra, serialisedData);
            return result;
        }

        protected async Task<bool> Post(object data, string extra = "")
        {
            var serialisedData = _serialiser.Serialise(data); // serialise
            var response = await GetResponse(HttpMethod.Post, extra, serialisedData); // get response
            // try to reauthorise
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                response = await TryReauth(o => GetResponse(HttpMethod.Post, extra, serialisedData));
            }

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else await HandlePrivate(response, "POST", extra, serialisedData);
            return false;
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
            else await HandlePrivate(response, "DELETE", extra, ""  );
            return false; // probably is never called, but is required for compilation
        }

        private async Task<HttpResponseMessage> TryReauth(Func<object, Task<HttpResponseMessage>> resourceGetter)
        {
            var authRepo = _authorisedConfigService.AuthenticationRepo;
            var token = await _authorisedConfigService.GetTokenGroupAsync();
            if (authRepo!=null && token!=null)
            {
                var result = await authRepo.RefreshLogin(token.Refresh);
                if (result.Success)
                {
                    await _authorisedConfigService.SetTokenGroupAsync(result.TokenGroup);
                    var response = await resourceGetter(null);
                    if (response.StatusCode == HttpStatusCode.Unauthorized) throw new WebRepoUnauthorisedException();
                    return response;
                }
                else
                {
                    throw new WebRepoUnauthorisedException();
                }
            }
            else
            {
                throw new WebRepoUnauthorisedException();
            }
        }

        private async Task HandlePrivate(HttpResponseMessage response, string verb, string extra, string payload)
        {
            try
            {
                var errors =
                    _serialiser.Deserialise<List<CustomResponseMessage>>(await response.Content.ReadAsStringAsync());
                await this.Handle(errors, response.StatusCode, extra,payload);
            }
            catch(JsonException)
            {
                await Handle(response, $"{verb} + {extra}", payload);
            }
        }

        

        private async Task<HttpResponseMessage> GetResponse(HttpMethod method, string extra = "", string content = null)
        {
            var baseUrl = await _configService.GetBaseUrlAsync();
            
            var path = GetFullPath(baseUrl, _service, extra);
            TokenGroup token = null;
            if (_authorisedConfigService != null)
            {
                token = await _authorisedConfigService.GetTokenGroupAsync();
            }
            var response = await _transferer.GetResponse(method, path, content, token?.Token);
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

        public abstract Task Handle(IEnumerable<CustomResponseMessage> serverMessages, HttpStatusCode statusCode, string url, string payload);

        public abstract Task Handle(HttpResponseMessage response, string url, string payload);


    }
}
