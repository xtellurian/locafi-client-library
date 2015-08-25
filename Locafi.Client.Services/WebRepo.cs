using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.Services;
using Locafi.Client.Model.Dto.Skus;

namespace Locafi.Client.Services
{
    public abstract class WebRepo : IWebRepo
    {
        private readonly IHttpTransferConfigService _configService;
        private readonly string _service;
        private readonly IAuthorisedHttpTransferConfigService _authorisedConfigService;
        private readonly ISerialiserService _serialiser;

        protected WebRepo(IAuthorisedHttpTransferConfigService authorisedConfigService, ISerialiserService serialiser, string service)
        {
            _authorisedConfigService = authorisedConfigService;
            _configService = authorisedConfigService;
            _serialiser = serialiser;
            _service = service;
        }

        protected WebRepo(IHttpTransferConfigService configService, ISerialiserService serialiser, string service)
        {
            _configService = configService;
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
                return result;
            }
            else
            {
                var result = _serialiser.Deserialise<T>(data);
                return result;
            }
        }

        protected async Task<T> Post<T>(object data, string extra = "") where T : class, new()
        {
            var response = await GetResponse(HttpMethod.Post, extra, _serialiser.Serialise(data));
            var result = response.IsSuccessStatusCode ? _serialiser.Deserialise<T>(await response.Content.ReadAsStringAsync()) : null;
            return result;
        }

        protected async Task Delete(string extra = "")
        {
            var response = await GetResponse(HttpMethod.Delete, extra);
            Debug.WriteLine(response.IsSuccessStatusCode
                ? $"{_service} service deleted  id={extra} successfully"
                : $"{_service} service failed to delete id={extra}");
        }

        private async Task<HttpResponseMessage> GetResponse(HttpMethod method, string extra = "", string content = null)
        {
            var baseUrl = _configService.BaseUrl;
            var path = GetFullPath(baseUrl, _service, extra);
            var message = new HttpRequestMessage(method, path);
            if(content!=null) message.Content = new StringContent(content, Encoding.UTF8, "application/json");

            //message.Content.Headers.Add("Content-Type", new List<string> { "application/json" });
            if(_authorisedConfigService!=null) message.Headers.Add("Authorization", "Token " + _authorisedConfigService.GetTokenString());

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
            var s2 = second.Trim('/');
            result.Append('/').Append(s1).Append('/').Append(s2);
            return result.ToString();
        }
    }
}
