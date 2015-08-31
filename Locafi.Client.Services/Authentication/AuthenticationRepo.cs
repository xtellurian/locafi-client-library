using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.Services;
using Locafi.Client.Data;
using Locafi.Client.Model.Dto;
using Locafi.Client.Model.Dto.Authentication;

namespace Locafi.Client.Services.Authentication
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
