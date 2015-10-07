using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.Http;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Exceptions;
using Locafi.Client.Model.Dto;
using Locafi.Client.Model.Dto.Authentication;
using Locafi.Client.Model.RelativeUri;
using Locafi.Client.Model.Responses;
using Locafi.Client.Model.Uri;
using Locafi.Client.Repo;

namespace Locafi.Client.Authentication
{
    public class AuthenticationRepo : WebRepo, IAuthenticationRepo
    {
        public AuthenticationRepo(IHttpTransferConfigService unauthorizedConfigService, ISerialiserService serialiser) 
            : base(new SimpleHttpTransferer(), unauthorizedConfigService, serialiser, AuthenticationUri.ServiceName)
        {
        }

        public AuthenticationRepo(IHttpTransferer transferer, IAuthorisedHttpTransferConfigService authorisedUnauthorizedConfigService, ISerialiserService serialiser)
           : base(transferer, authorisedUnauthorizedConfigService, serialiser, AuthenticationUri.ServiceName)
        {
        }

        public async Task<AuthenticationResponseDto> Login (string emailAddress, string password)
        {
            var dto = new UserLoginDto
            {
                Username = emailAddress,
                Password = password
            };
            return await LoginWithDto(dto, AuthenticationUri.Login);
        }

        public async Task<AuthenticationResponseDto> Login(ILoginCredentialsProvider credentials)
        {
            var dto = new UserLoginDto
            {
                Username = credentials.UserName,
                Password = credentials.Password
            };
            return await LoginWithDto(dto, AuthenticationUri.Login);
        }

        public async Task<AuthenticationResponseDto> RefreshLogin(string refreshToken)
        {
            var dto = new RefreshLoginDto(refreshToken);
            var path = AuthenticationUri.RefreshLogin;
            var result = await Post<AuthenticationResponseDto>(dto, path);
            return result;
        }


        public async Task<AuthenticationResponseDto> ReaderLogin(ILoginCredentialsProvider credentials)
        {
            var dto = new UserLoginDto
            {
                Password = credentials.Password,
                Username = credentials.UserName
            };

            return await LoginWithDto(dto, AuthenticationUri.ReaderLogin);
        }

        public async Task<AuthenticationResponseDto> ReaderLogin(string emailAddress, string password)
        {
            var dto = new UserLoginDto
            {
                Password = password,
                Username = emailAddress
            };
            return await LoginWithDto(dto, AuthenticationUri.ReaderLogin);
        }

        private async Task<AuthenticationResponseDto> LoginWithDto(UserLoginDto dto, string path)
        {
            var result = await Post<AuthenticationResponseDto>(dto, path);
            return result;
        }

        public override Task Handle(IEnumerable<CustomResponseMessage> serverMessages, HttpStatusCode statusCode)
        {
            throw new AuthenticationRepoException(serverMessages, statusCode);
        }

        public override async Task Handle(HttpResponseMessage response)
        {
            throw new AuthenticationRepoException(await response.Content.ReadAsStringAsync());
        }
    }
}
