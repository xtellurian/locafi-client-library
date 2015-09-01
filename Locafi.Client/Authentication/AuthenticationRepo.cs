using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Model.Dto;
using Locafi.Client.Model.Dto.Authentication;
using Locafi.Client.Repo;
using Locafi.Client.Services;

namespace Locafi.Client.Authentication
{
    public class AuthenticationRepo : WebRepo, IAuthenticationRepo
    {
        public AuthenticationRepo(IHttpTransferConfigService unauthorizedConfigService, ISerialiserService serialiser) : base(unauthorizedConfigService, serialiser, "authentication")
        {
        }

        public async Task<AuthorizationTokenDto> Login (string usrname, string passwrd)
        {
            var usr = new UserLoginDto
            {
                Username = usrname,
                Password = passwrd
            };
            var result = await Post<AuthorizationTokenDto>(usr, "/Login");
            return result;
        }

        public async Task<AuthorizationTokenDto> RefreshLogin(string refreshToken)
        {
            var dto = new RefreshLoginDto(refreshToken);
            var path = "RefreshLogin";
            var result = await Post<AuthorizationTokenDto>(dto, path);
            return result;
        }
    }
}
