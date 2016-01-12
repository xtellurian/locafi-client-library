using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.ErrorHandlers;
using Locafi.Client.Contract.Http;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Exceptions;
using Locafi.Client.Model.Dto.Portal;
using Locafi.Client.Model.RelativeUri;
using Locafi.Client.Model.Responses;

namespace Locafi.Client.Repo
{
    public class PortalRepo : WebRepo, IWebRepoErrorHandler, IPortalRepo
    {
        public PortalRepo(IAuthorisedHttpTransferConfigService authorisedUnauthorizedConfigService, ISerialiserService serialiser) 
            : base(new SimpleHttpTransferer(),authorisedUnauthorizedConfigService, serialiser, PortalUri.ServiceName)
        {
        }

        public async Task<IList<PortalSummaryDto>> GetPortals()
        {
            var path = PortalUri.GetPortals;
            var result = await Get<List<PortalSummaryDto>>(path);
            return result;
        }

        public async Task<PortalDetailDto> GetPortal(Guid id)
        {
            var path = PortalUri.GetPortal(id);
            var result = await Get<PortalDetailDto>(path);
            return result;
        }

        public async Task<PortalDetailDto> GetPortal(string serial)
        {
            var path = PortalUri.GetPortal(serial);
            var result = await Get<PortalDetailDto>(path);
            return result;
        }

        public async Task<PortalDetailDto> CreatePortal(AddPortalDto addPortalDto)
        {
            var path = PortalUri.CreatePortal;
            var result = await Post<PortalDetailDto>(addPortalDto, path);
            return result;
        }

        public async Task<PortalDetailDto> UpdatePortal(UpdatePortalDto updatePortalDto)
        {
            var path = PortalUri.UpdatePortal(updatePortalDto.Id);
            var result = await Post<PortalDetailDto>(updatePortalDto, path);
            return result;
        }

        public async Task DeletePortal(Guid id)
        {
            var path = PortalUri.DeletePortal(id);
            await Delete(path);
        }

        public async Task<IList<PortalRuleSummaryDto>> GetPortalRules()
        {
            var path = PortalUri.GetPortalRules();
            var result = await Get<List<PortalRuleSummaryDto>>(path);
            return result;
        }

        public async Task<IList<PortalRuleDetailDto>> GetPortalRules(Guid id)
        {
            var path = PortalUri.GetPortalRules(id);
            var result = await Get<List<PortalRuleDetailDto>>(path);
            return result;
        }

        public async Task<PortalRuleDetailDto> GetPortalRule(Guid id)
        {
            var path = PortalUri.GetPortalRule(id);
            var result = await Get<PortalRuleDetailDto>(path);
            return result;
        }

        public async Task<PortalRuleDetailDto> CreatePortalRule(AddPortalRuleDto addPortalRuleDto)
        {
            var path = PortalUri.CreatePortalRule;
            var result = await Post<PortalRuleDetailDto>(addPortalRuleDto, path);
            return result;
        }

        public async Task<PortalRuleDetailDto> UpdatePortalRule(UpdatePortalRuleDto updatePortalRuleDto)
        {
            var path = PortalUri.UpdatePortalRule(updatePortalRuleDto.Id);
            var result = await Post<PortalRuleDetailDto>(updatePortalRuleDto, path);
            return result;
        }

        public async Task DeletePortalRule(Guid id)
        {
            var path = PortalUri.DeletePortalRule(id);
            await Delete(path);
        }

        public override Task Handle(IEnumerable<CustomResponseMessage> serverMessages, HttpStatusCode statusCode)
        {
            throw new PortalRepoException(serverMessages, statusCode);
        }

        public override async Task Handle(HttpResponseMessage response)
        {
            throw new PortalRepoException(await response.Content.ReadAsStringAsync());
        }
    }
}
