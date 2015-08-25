using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.Services;
using Locafi.Client.Data;
using Locafi.Client.Model.Dto;

namespace Locafi.Client.Services.Authentication
{
    public class AuthenticationRepo : WebRepo, IAuthenticationRepo
    {
        public AuthenticationRepo(IHttpTransferConfigService configService, ISerialiserService serialiser) : base(configService, serialiser, "authentication")
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
    }
}
