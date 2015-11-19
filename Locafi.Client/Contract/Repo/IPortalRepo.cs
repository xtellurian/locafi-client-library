using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.Portal;

namespace Locafi.Client.Contract.Repo
{
    public interface IPortalRepo
    {
        Task<IList<PortalSummaryDto>> GetPortals();
        Task<PortalDetailDto> GetPortal(Guid id);
        Task<PortalDetailDto> CreatePortal(AddPortalDto addPortalDto);
        Task<PortalDetailDto> UpdatePortal(UpdatePortalDto updatePortalDto);
        Task DeletePortal(Guid id);
        Task<IList<PortalRuleSummaryDto>> GetPortalRules();
        Task<IList<PortalRuleDetailDto>> GetPortalRules(Guid id);
        Task<PortalRuleDetailDto> GetPortalRule(Guid id);
        Task<PortalRuleDetailDto> CreatePortalRule(AddPortalRuleDto addPortalRuleDto);
        Task<PortalRuleDetailDto> UpdatePortalRule(UpdatePortalRuleDto updatePortalRuleDto);
        Task DeletePortalRule(Guid id);
    }
}