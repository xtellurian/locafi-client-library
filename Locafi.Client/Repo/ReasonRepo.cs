using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.Errors;
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
        public ReasonRepo(IAuthorisedHttpTransferConfigService authorisedUnauthorizedConfigService, ISerialiserService serialiser) 
            : base(authorisedUnauthorizedConfigService, serialiser, ReasonUri.ServiceName)
        {
        }

        public async Task<IList<ReasonDetailDto>> GetAllReasons()
        {
            var path = ReasonUri.GetReasons;
            var result = await Get<IList<ReasonDetailDto>>(path);
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
            var result = await Get<IList<ReasonDetailDto>>(path);
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

        public override Task Handle(IEnumerable<CustomResponseMessage> serverMessages)
        {
            throw new ReasonRepoException(serverMessages);
        }
    }
}
