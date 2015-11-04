using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Locafi.Client
{
    public static class LocafiApiLocator
    {
        private const string ApiLocatorUrl =
            @"https://microsoft-apiapp9db5d7eaeefa415a9a58fc01c3227e14.azurewebsites.net/api/apilocation";

        public static async Task<IList<string>> GetApiBaseUrl(string userName)
        {
            var path = $"{ApiLocatorUrl}?userName={userName}";
            var message = new HttpRequestMessage(HttpMethod.Get, path);

            var client = new HttpClient();
            var response = await client.SendAsync(message);
            var serverMessage = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<IList<string>>(serverMessage);
        }
    }
}
