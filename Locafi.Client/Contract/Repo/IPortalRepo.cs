using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Model.Query;
using Locafi.Client.Model;
using Locafi.Client.Model.Dto.Portals;

namespace Locafi.Client.Contract.Repo
{
    public interface IPortalRepo
    {
        Task<PageResult<PortalSummaryDto>> QueryPortals(string oDataQueryOptions = null);
        Task<PageResult<PortalSummaryDto>> QueryPortals(IRestQuery<PortalSummaryDto> query);
        Task<IQueryResult<PortalSummaryDto>> QueryPortalsContinuation(IRestQuery<PortalSummaryDto> query);
        Task<PortalDetailDto> GetPortal(Guid id);
        Task<PortalDetailDto> CreatePortal(AddPortalDto addPortalDto);
        Task<PortalDetailDto> UpdatePortal(UpdatePortalDto updatePortalDto);
        Task DeletePortal(Guid id);
        Task<PageResult<PortalRuleSummaryDto>> QueryPortalRules(string oDataQueryOptions = null);
        Task<PageResult<PortalRuleSummaryDto>> QueryPortalRules(IRestQuery<PortalRuleSummaryDto> query);
        Task<IQueryResult<PortalRuleSummaryDto>> QueryPortalRulesContinuation(IRestQuery<PortalRuleSummaryDto> query);
        Task<IList<PortalRuleDetailDto>> GetRulesForPortal(Guid id);
        Task<PortalRuleDetailDto> GetPortalRule(Guid id);
        Task<PortalRuleDetailDto> CreatePortalRule(AddPortalRuleDto addPortalRuleDto);
        Task<PortalRuleDetailDto> UpdatePortalRule(UpdatePortalRuleDto updatePortalRuleDto);
        Task DeletePortalRule(Guid id);
        Task<PortalDetailDto> GetPortal(string serial);
        //Task<PortalStatusDto> GetPortalStatus(Guid id);
        //Task<PortalStatusDto> UpdatePortalStatus(UpdatePortalStatusDto updatePortalStatusDto);
        //Task <bool> UpdatePortalHeartbeat(PortalHeartbeatDto portalHeartbeatDto);
        //Task<TagAccessResultDto> CheckAccess(CheckTagAccessDto tagAccessDto);
    }
}