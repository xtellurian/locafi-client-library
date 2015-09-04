﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.Errors;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Exceptions;
using Locafi.Client.Model.Dto.Authentication;
using Locafi.Client.Model.Dto.Reader;
using Locafi.Client.Model.Responses;
using Locafi.Client.Model.Uri;

namespace Locafi.Client.Repo
{
    public class ReaderRepo : WebRepo, IWebRepoErrorHandler, IReaderRepo
    {
        public ReaderRepo(IAuthorisedHttpTransferConfigService authorisedUnauthorizedConfigService,
            ISerialiserService serialiser)
            : base(authorisedUnauthorizedConfigService, serialiser, ReaderUri.ServiceName)
        {
        }

        public async Task<IList<ReaderSummaryDto>> GetReaders()
        {
            var path = ReaderUri.GetReaders;
            var result = await Get<IList<ReaderSummaryDto>>(path);
            return result;
        }

        public async Task<ReaderDetailDto> GetReaderById(Guid id)
        {
            var path = ReaderUri.GetReader(id);
            var result = await Get<ReaderDetailDto>(path);
            return result;
        }

        public async Task DeleteReader(Guid id)
        {
            var path = ReaderUri.Delete(id);
            await Delete(path);
        }

        public async Task<ClusterReponseDto> ProcessCluster(ClusterDto cluster)
        {
            var path = ReaderUri.ProcessCluster;
            var result = await Post<ClusterReponseDto>(cluster,path);
            return result;
        }

        public async Task<AuthenticationResponseDto> ReaderLogin(ILoginCredentialsProvider credentials)
        {
            var dto = new UserLoginDto
            {
                Password = credentials.Password,
                Username = credentials.UserName
            };

            return await LoginWithDto(dto);
        }

        public async Task<AuthenticationResponseDto> ReaderLogin(string userName, string password)
        {
            var dto = new UserLoginDto
            {
                Password = password,
                Username = userName
            };
            return await LoginWithDto(dto);
        }

        private async Task<AuthenticationResponseDto> LoginWithDto(UserLoginDto dto)
        {
            var path = ReaderUri.Login;
            var result = await Post<AuthenticationResponseDto>(dto, path);
            return result;
        }


        public override async Task Handle(HttpResponseMessage responseMessage)
        {
            throw new ReaderRepoException(await responseMessage.Content.ReadAsStringAsync());
        }

        public override Task Handle(IEnumerable<CustomResponseMessage> serverMessages)
        {
            throw new ReaderRepoException(serverMessages);
        }
    }
}
