using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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

        public static async Task<AuthorisedHttpTransferConfigService> Generate(string baseUrl, string usrname, string passwrd, bool isReader = false)
        {
            var user = new UserLoginDto
            {
                Username = usrname,
                Password = passwrd
            };
            TokenGroup result;
            if (isReader)
            {
                result = await Post(baseUrl + "Authentication/ReaderLogin/", user);
            }
            else
            {                
                result = await Post(baseUrl + "Authentication/Login/", user);                
            }
            var configService = new AuthorisedHttpTransferConfigService(result, o => GenerateReauthedConfig(result.Refresh, baseUrl))
            {
                BaseUrl = baseUrl
            };
            return configService;
        }

        private static async Task<IAuthorisedHttpTransferConfigService> GenerateReauthedConfig(string refreshToken, string baseUrl)
        {
            var dto = new RefreshLoginDto(refreshToken);
            var path = AuthenticationUri.RefreshLogin;
            var result = await Post(path, dto);

            var service = new AuthorisedHttpTransferConfigService(result,
                o => GenerateReauthedConfig(result.Refresh, baseUrl))
            {
                BaseUrl = baseUrl
            };
            return service;
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
