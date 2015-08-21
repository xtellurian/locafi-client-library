using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Services.Contract;

namespace Locafi.Client.Services
{
    public abstract class WebRepo
    {
        private readonly string _service;
        private readonly IHttpTransferConfigService _configService;
        private readonly ISerialiserService _serialiser;

        protected WebRepo(IHttpTransferConfigService configService, ISerialiserService serialiser, string service)
        {
            _configService = configService;
            _serialiser = serialiser;
            _service = service;
        }

        protected async Task<T> Get<T>(string extra = "") where T: class, new()
        {
            var response = await GetResponse(HttpMethod.Get, extra);
            var result = response.IsSuccessStatusCode ? _serialiser.Deserialise<T>(await response.Content.ReadAsStringAsync()) : null;
            return result;
        }

        protected async Task<T> Post<T>(object data, string extra = "") where T : class, new()
        {
            var response = await GetResponse(HttpMethod.Post, extra);
            var result = response.IsSuccessStatusCode ? _serialiser.Deserialise<T>(await response.Content.ReadAsStringAsync()) : null;
            return result;
        }

        private async Task<HttpResponseMessage> GetResponse(HttpMethod method, string extra = null)
        {
            var baseUrl = await _configService.GetBaseUrl();
            var message = new HttpRequestMessage(method, baseUrl + _service + extra);
            message.Headers.Add("Authorization", "Token " + _configService.GetTokenString());

            var client = new HttpClient();
            return await client.SendAsync(message);
        }

    }
}
