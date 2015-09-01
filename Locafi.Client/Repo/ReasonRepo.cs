using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Model.Dto.Reasons;
using Locafi.Client.Services;

namespace Locafi.Client.Repo
{
    public class ReasonRepo : WebRepo, IReasonRepo
    {
        public ReasonRepo(IAuthorisedHttpTransferConfigService authorisedUnauthorizedConfigService, ISerialiserService serialiser) 
            : base(authorisedUnauthorizedConfigService, serialiser, "Reasons")
        {
        }

        public async Task<IList<ReasonDetailDto>> GetAllReasons()
        {
            var path = "GetReasons";
            var result = await Get<IList<ReasonDetailDto>>(path);
            return result;
        }

        public async Task<ReasonDetailDto> CreateReason(AddReasonDto reasonDto)
        {
            var path = "CreateReason";
            var result = await Post<ReasonDetailDto>(reasonDto, path);
            return result;
        }

        public async Task Delete(Guid id)
        {
            var path = $"DeleteReason/{id}";
            await Delete(path);
        }
    }
}
