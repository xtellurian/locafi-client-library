using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Exceptions;
using Locafi.Client.Model.Dto;
using Locafi.Client.Model.Dto.Authentication;
using Locafi.Client.Model.RelativeUri;
using Locafi.Client.Model.Responses;
using Locafi.Client.Repo;

namespace Locafi.Client.Authentication
{
    public class AuthenticationRepo : WebRepo, IAuthenticationRepo
    {
        public AuthenticationRepo(IHttpTransferConfigService unauthorizedConfigService, ISerialiserService serialiser) 
            : base(unauthorizedConfigService, serialiser, AuthenticationUri.ServiceName)
        {
        }

        public async Task<AuthenticationResponseDto> Login (string usrname, string passwrd)
        {
            var usr = new UserLoginDto
            {
                Username = usrname,
                Password = passwrd
            };
            var result = await Post<AuthenticationResponseDto>(usr, AuthenticationUri.Login);
            return result;
        }

        public async Task<AuthenticationResponseDto> Login(ILoginCredentialsProvider credentials)
        {
            var usr = new UserLoginDto
            {
                Username = credentials.UserName,
                Password = credentials.Password
            };
            var result = await Post<AuthenticationResponseDto>(usr, AuthenticationUri.Login);
            return result;
        }

        public async Task<AuthenticationResponseDto> RefreshLogin(string refreshToken)
        {
            var dto = new RefreshLoginDto(refreshToken);
            var path = AuthenticationUri.RefreshLogin;
            var result = await Post<AuthenticationResponseDto>(dto, path);
            return result;
        }

        public override Task Handle(IEnumerable<CustomResponseMessage> serverMessages)
        {
            throw new AuthenticationRepoException(serverMessages);
        }

        public override async Task Handle(HttpResponseMessage response)
        {
            throw new AuthenticationRepoException(await response.Content.ReadAsStringAsync());
        }
    }
}
