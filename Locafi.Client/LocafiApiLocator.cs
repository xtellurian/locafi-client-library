using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Locafi.Client
{
    public static class LocafiApiLocator
    {
        private const string ApiLocatorUrl =
            @"https://microsoft-apiapp9db5d7eaeefa415a9a58fc01c3227e14.azurewebsites.net/find";

        public static async Task<IDictionary<string, string>> GetApiBaseUrl(string userName)
        {
            var path = $"{ApiLocatorUrl}?userName={userName}";
            var message = new HttpRequestMessage(HttpMethod.Get, path);

            var client = new HttpClient();
            var response = await client.SendAsync(message);
            var serverMessage = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Dictionary<string,string>>(serverMessage);
        }

        public static async Task<string> GetApiBaseUrlForDevice(string serialNumber)
        {
            var path = $"{ApiLocatorUrl}/{serialNumber}";
            var message = new HttpRequestMessage(HttpMethod.Get, path);

            var client = new HttpClient();
            var response = await client.SendAsync(message);
            if (!response.IsSuccessStatusCode) return null;
            var serverMessage = await response.Content.ReadAsStringAsync();
            try
            {
                var record = JsonConvert.DeserializeObject<ApiRecord>(serverMessage);
                return record.Url;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }

        private class ApiRecord
        {
            public string Id { get; set; }
            public string Url { get; set; }
        }
    }
}
