using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.Services;
using Locafi.Client.Data;
using Locafi.Client.Model.Dto.Reasons;

namespace Locafi.Client.Services.Repo
{
    public class ReasonRepo : WebRepo, IReasonRepo
    {
        public ReasonRepo(IAuthorisedHttpTransferConfigService authorisedConfigService, ISerialiserService serialiser) 
            : base(authorisedConfigService, serialiser, "Reasons")
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
