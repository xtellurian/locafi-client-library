using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Data;
using Locafi.Client.Services.Contract;
using Locafi.Client.UnitTests.Implementations;
using Newtonsoft.Json;

namespace Locafi.Client.UnitTests.Factory
{
    public static class HttpConfigFactory
    {

        public static async Task<HttpTransferConfigService> Generate(string baseUrl, string usrname, string passwrd)
        {
            var user = new UserLoginDto
            {
                username = usrname,
                password = passwrd
            };
            var result = await Post(baseUrl + "auth/login/", user);

            var configService = new HttpTransferConfigService(result)
            {
                BaseUrl = baseUrl
            };
            return configService;
        }

        private static async Task<string> Post(string url, UserLoginDto loginDto)
        {
            var message = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(JsonConvert.SerializeObject(loginDto), Encoding.UTF8, "application/json")
            };


            var client = new HttpClient();
            var response = await client.SendAsync(message);

            var result = response.IsSuccessStatusCode ? JsonConvert.DeserializeObject<AuthorizationTokenDto>(await response.Content.ReadAsStringAsync()) : null;

            return result?.tokens.Token;
        }

    }
}
