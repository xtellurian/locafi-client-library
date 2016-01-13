using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.ErrorHandlers;
using Locafi.Client.Contract.Http;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Exceptions;
using Locafi.Client.Model.Dto.Reasons;
using Locafi.Client.Model.Enums;
using Locafi.Client.Model.Responses;
using Locafi.Client.Model.Uri;

namespace Locafi.Client.Repo
{
    public class ReasonRepo : WebRepo, IReasonRepo, IWebRepoErrorHandler
    {
        public ReasonRepo(IAuthorisedHttpTransferConfigService authorisedConfigService, ISerialiserService serialiser) 
            : base(new SimpleHttpTransferer(), authorisedConfigService, serialiser, ReasonUri.ServiceName)
        {
        }

        public ReasonRepo(IHttpTransferer transferer, IAuthorisedHttpTransferConfigService authorisedConfigService, ISerialiserService serialiser)
           : base(transferer, authorisedConfigService, serialiser, ReasonUri.ServiceName)
        {
        }

        public async Task<IList<ReasonDetailDto>> GetAllReasons()
        {
            var path = ReasonUri.GetReasons;
            var result = await Get<List<ReasonDetailDto>>(path);
            return result;
        }

        public async Task<ReasonDetailDto> CreateReason(AddReasonDto reasonDto)
        {
            var path = ReasonUri.CreateReason;
            var result = await Post<ReasonDetailDto>(reasonDto, path);
            return result;
        }

        public async Task<IList<ReasonDetailDto>> GetReasonsFor(ReasonFor reasonFor)
        {
            var path = ReasonUri.GetReasonsFor(reasonFor);
            var result = await Get<List<ReasonDetailDto>>(path);
            return result;
        }

        public async Task Delete(Guid id)
        {
            var path = ReasonUri.DeleteReason(id);
            await Delete(path);
        }

        public override async Task Handle(HttpResponseMessage responseMessage)
        {
            throw new ReasonRepoException(await responseMessage.Content.ReadAsStringAsync());
        }

        public override Task Handle(IEnumerable<CustomResponseMessage> serverMessages, HttpStatusCode statusCode)
        {
            throw new ReasonRepoException(serverMessages, statusCode);
        }
    }
}
