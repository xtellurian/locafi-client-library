using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Authentication;
using Locafi.Client.Model.Dto.Authentication;
using Locafi.Script.Implementations;
using Newtonsoft.Json;

namespace Locafi.Script.Factory
{
    public static class HttpConfigFactory
    {

        public static async Task<AuthorisedHttpTransferConfigService> Generate(string baseUrl, string usrname, string passwrd, bool isPortal = false)
        {
            var user = new UserLoginDto
            {
                Username = usrname,
                Password = passwrd
            };
            TokenGroup result;
            if (isPortal)
            {
                result = await Post(baseUrl + "Authentication/PortalLogin/", user);
            }
            else
            {
                result = await Post(baseUrl + "Authentication/Login/", user);
            }
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
