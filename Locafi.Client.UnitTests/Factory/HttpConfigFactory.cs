using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Authentication;
using Locafi.Client.Contract.Config;
using Locafi.Client.Model.Dto;
using Locafi.Client.Model.Dto.Authentication;
using Locafi.Client.Model.RelativeUri;
using Locafi.Client.UnitTests.Implementations;
using Newtonsoft.Json;

namespace Locafi.Client.UnitTests.Factory
{
    public static class HttpConfigFactory
    {

        public static async Task<AuthorisedHttpTransferConfigService> Generate(string baseUrl, string usrname, string passwrd)
        {
            var user = new UserLoginDto
            {
                Username = usrname,
                Password = passwrd
            };                                  
            var result = await Post(baseUrl + "Authentication/Login/", user);                            
            var configService = new UnauthorisedHttpTransferConfigService();
            var authRepo = new AuthenticationRepo(configService, new Serialiser());
            var authConfigService = new AuthorisedHttpTransferConfigService(authRepo, result)
            {
                BaseUrl = baseUrl
            };
            return authConfigService;
        }

        public static async Task<AuthorisedHttpTransferConfigService> GenerateAgent(string baseUrl, string hardwarekey)
        {
            var agent = new AgentLoginDto
            {
                HardwareKey = hardwarekey
            };
            var result = await Post(baseUrl + "Authentication/AgentLogin/", agent);
            var configService = new UnauthorisedHttpTransferConfigService();
            var authRepo = new AuthenticationRepo(configService, new Serialiser());
            var authConfigService = new AuthorisedHttpTransferConfigService(authRepo, result)
            {
                BaseUrl = baseUrl
            };
            return authConfigService;
        }

        private static async Task<TokenGroup> Post(string url, UserLoginDto loginDto)
        {
            var message = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(JsonConvert.SerializeObject(loginDto), Encoding.UTF8, "application/json")
            };


            var client = new HttpClient();
            var response = await client.SendAsync(message);

            var result = response.IsSuccessStatusCode ? JsonConvert.DeserializeObject<AuthenticationResponseDto>(await response.Content.ReadAsStringAsync()) : null;

            return result?.TokenGroup;
        }

        private static async Task<TokenGroup> Post(string url, AgentLoginDto agentLoginDto)
        {
            var message = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(JsonConvert.SerializeObject(agentLoginDto), Encoding.UTF8, "application/json")
            };


            var client = new HttpClient();
            var response = await client.SendAsync(message);

            var result = response.IsSuccessStatusCode ? JsonConvert.DeserializeObject<AuthenticationResponseDto>(await response.Content.ReadAsStringAsync()) : null;

            return result?.TokenGroup;
        }

        private static async Task<TokenGroup> Post(string url, RefreshLoginDto loginDto)
        {
            var message = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(JsonConvert.SerializeObject(loginDto), Encoding.UTF8, "application/json")
            };


            var client = new HttpClient();
            var response = await client.SendAsync(message);

            var result = response.IsSuccessStatusCode ? JsonConvert.DeserializeObject<AuthenticationResponseDto>(await response.Content.ReadAsStringAsync()) : null;

            return result?.TokenGroup;
        }

    }
}
