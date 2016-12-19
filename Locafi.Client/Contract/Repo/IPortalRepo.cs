using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Model.Query;
using Locafi.Client.Model;
using Locafi.Client.Model.Dto.Portals;
using Locafi.Client.Model.Dto.Portals.Clusters;

namespace Locafi.Client.Contract.Repo
{
    public interface IPortalRepo
    {
        Task<PageResult<PortalSummaryDto>> GetPortals(string oDataQueryOptions = null);
        Task<PageResult<PortalSummaryDto>> GetPortals(IRestQuery<PortalSummaryDto> query);
        Task<IQueryResult<PortalSummaryDto>> GetPortalsContinuation(IRestQuery<PortalSummaryDto> query);
        Task<PortalDetailDto> GetPortal(Guid id);
        Task<PortalDetailDto> CreatePortal(AddPortalDto addPortalDto);
        Task<PortalDetailDto> UpdatePortal(UpdatePortalDto updatePortalDto);
        Task DeletePortal(Guid id);
        Task<PageResult<PortalRuleSummaryDto>> GetPortalRules(string oDataQueryOptions = null);
        Task<PageResult<PortalRuleSummaryDto>> GetPortalRules(IRestQuery<PortalRuleSummaryDto> query);
        Task<IQueryResult<PortalRuleSummaryDto>> GetPortalRulesContinuation(IRestQuery<PortalRuleSummaryDto> query);
        Task<PortalRuleDetailDto> GetPortalRule(Guid id);
        Task<PortalRuleDetailDto> CreatePortalRule(AddPortalRuleDto addPortalRuleDto);
        Task<PortalRuleDetailDto> UpdatePortalRule(UpdatePortalRuleDto updatePortalRuleDto);
        Task DeletePortalRule(Guid id);
        Task<DeviceDetailDto> CreateDevice(AddDeviceDto deviceDto);
        Task<DeviceDetailDto> UpdateDevice(UpdateDeviceDto updateDto);
        Task<PageResult<DeviceSummaryDto>> GetDevices(string oDataQueryOptions = null);
        Task<PageResult<DeviceSummaryDto>> GetDevices(IRestQuery<DeviceSummaryDto> query);
        Task<IQueryResult<DeviceSummaryDto>> GetDevicesContinuation(IRestQuery<DeviceSummaryDto> query);
        Task<DeviceDetailDto> GetDevice(Guid id);
        Task DeleteDevice(Guid id);
    }
}