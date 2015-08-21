using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Services.Contract;
using System.Net.Http;


namespace Locafi.Client.Services
{
    public abstract class WebRepo<T> where T : class, new()
    {
        private readonly IHttpTransferConfigService _configService;
        private readonly ISerialiserService _serialiser;
        private readonly string _service;

        protected WebRepo(IHttpTransferConfigService configService, ISerialiserService serialiser, string service)
        {
            _configService = configService;
            _serialiser = serialiser;
            _service = service;
        }

        protected async Task<T> Get(string extra = "")
        {
            var baseUrl = await _configService.GetBaseUrl();
            var message = new HttpRequestMessage(HttpMethod.Get, baseUrl + _service + extra);
            message.Headers.Add("Authorization", "Token " + _configService.GetTokenString());

            var client = new HttpClient();
            var response = await client.SendAsync(message);

            var result = response.IsSuccessStatusCode ? _serialiser.Deserialise<T>(await response.Content.ReadAsStringAsync()) : null;

            return result;
        }

        protected async Task<IList<T>> GetList(string extra = "")
        {
            var baseUrl = await _configService.GetBaseUrl();
            var message = new HttpRequestMessage(HttpMethod.Get, baseUrl + _service + extra);
            message.Headers.Add("Authorization", "Token " + _configService.GetTokenString());

            var client = new HttpClient();
            var response = await client.SendAsync(message);

            var result = response.IsSuccessStatusCode ? _serialiser.Deserialise<IList<T>>(await response.Content.ReadAsStringAsync()) : null;

            return result;
        }

        protected async Task<T> Post(T body, string extra = "")
        {
            var baseUrl = await _configService.GetBaseUrl();
            var message = new HttpRequestMessage(HttpMethod.Post, baseUrl + _service + extra);
            message.Headers.Add("Authorization", "Token " + _configService.GetTokenString());
            message.Content = new StringContent(_serialiser.Serialise(body), Encoding.UTF8, "application/json");

            var client = new HttpClient();
            var response = await client.SendAsync(message);

            var result = response.IsSuccessStatusCode ? _serialiser.Deserialise<T>(await response.Content.ReadAsStringAsync()) : null;

            return result;
        }

        protected async Task<T> PostRaw(object body, string extra = "")
        {
            var baseUrl = await _configService.GetBaseUrl();
            var message = new HttpRequestMessage(HttpMethod.Post, baseUrl + _service + extra);
            message.Headers.Add("Authorization", "Token " + _configService.GetTokenString());
            message.Content = new StringContent(_serialiser.Serialise(body), Encoding.UTF8, "application/json");

            var client = new HttpClient();
            var response = await client.SendAsync(message);

            var result = response.IsSuccessStatusCode ? _serialiser.Deserialise<T>(await response.Content.ReadAsStringAsync()) : null;

            return result;
        }

        protected async Task<string> PostResult(object body, string extra = "")
        {
            var baseUrl = await _configService.GetBaseUrl();
            var message = new HttpRequestMessage(HttpMethod.Post, baseUrl + _service + extra);
            message.Headers.Add("Authorization", "Token " + _configService.GetTokenString());
            message.Content = new StringContent(_serialiser.Serialise(body), Encoding.UTF8, "application/json");

            var client = new HttpClient();
            var response = await client.SendAsync(message);
            if (response.IsSuccessStatusCode) return await response.Content.ReadAsStringAsync();
            return null;
        }
    }
}
