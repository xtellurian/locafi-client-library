﻿using System.Collections.Generic;
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
        public AuthenticationRepo(IHttpTransferConfigService configService, ISerialiserService serialiser) 
            : base(new SimpleHttpTransferer(), configService, serialiser, AuthenticationUri.ServiceName)
        {
        }

        public AuthenticationRepo(IHttpTransferer transferer, IAuthorisedHttpTransferConfigService authorisedConfigService, ISerialiserService serialiser)
           : base(transferer, authorisedConfigService, serialiser, AuthenticationUri.ServiceName)
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

        private async Task<AuthenticationResponseDto> LoginWithDto(UserLoginDto dto, string path)
        {
            var result = await Post<AuthenticationResponseDto>(dto, path);
            return result;
        }

        private async Task<AuthenticationResponseDto> LoginWithDto(AgentLoginDto dto, string path)
        {
            var result = await Post<AuthenticationResponseDto>(dto, path);
            return result;
        }

        public async Task<bool> Register(RegistrationDto registrationDto)
        {
            var path = AuthenticationUri.Register;
            var result = await Post(registrationDto, path);
            return result;
        }

        public async Task<AuthenticationResponseDto> AgentLogin(AgentLoginDto agentLoginDto)
        {
            var path = AuthenticationUri.AgentLogin;
            return await LoginWithDto(agentLoginDto, path);
        }

        public async Task<AuthenticationResponseDto> AgentLogin(string hardwareKey)
        {
            var agentLoginDto = new AgentLoginDto {HardwareKey = hardwareKey};
            var path = AuthenticationUri.AgentLogin;
            return await LoginWithDto(agentLoginDto, path);
        }

        public async Task<AuthenticationResponseDto> RefreshAgentLogin(string refreshToken)
        {
            var dto = new RefreshLoginDto(refreshToken);
            var path = AuthenticationUri.RefreshAgentLogin;
            var result = await Post<AuthenticationResponseDto>(dto, path);
            return result;
        }

        public override Task Handle(IEnumerable<CustomResponseMessage> serverMessages, HttpStatusCode statusCode, string url, string payload)
        {
            throw new AuthenticationRepoException(serverMessages, statusCode, url, payload);
        }

        public override async Task Handle(HttpResponseMessage response, string url, string payload)
        {
            throw new AuthenticationRepoException($"{url} -- {payload}  -- " + await response.Content.ReadAsStringAsync());
        }
    }
}
