using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.SkuGroups;
using Locafi.Client.Model.Query;
using Locafi.Client.Model;

namespace Locafi.Client.Contract.Repo
{
    public interface ISkuGroupRepo
    {
        Task<PageResult<SkuGroupNameDetailDto>> QuerySkuGroupNames(string oDataQueryOptions = null);
        Task<PageResult<SkuGroupNameDetailDto>> QuerySkuGroupNames(IRestQuery<SkuGroupNameDetailDto> query);
        Task<IQueryResult<SkuGroupNameDetailDto>> QuerySkuGroupNamesContinuation(IRestQuery<SkuGroupNameDetailDto> query);
        Task<SkuGroupNameDetailDto> GetNameById(Guid id);
        Task<SkuGroupNameDetailDto> CreateSkuGroupName(AddSkuGroupNameDto addSkuGroupNameDto);
        Task<SkuGroupNameDetailDto> UpdateSkuGroupName(UpdateSkuGroupNameDto updateSkuGroupNameDto);
        Task<bool> DeleteSkuGroupName(Guid id);
        Task<PageResult<SkuGroupSummaryDto>> QuerySkuGroups(string oDataQueryOptions = null);
        Task<PageResult<SkuGroupSummaryDto>> QuerySkuGroups(IRestQuery<SkuGroupSummaryDto> query);
        Task<IQueryResult<SkuGroupSummaryDto>> QuerySkuGroupsContinuation(IRestQuery<SkuGroupSummaryDto> query);
        Task<SkuGroupDetailDto> GetSkuGroupDetail(Guid id);
        Task<SkuGroupDetailDto> CreateSkuGroup(AddSkuGroupDto addSkuGroupDto);
        Task<SkuGroupDetailDto> UpdateSkuGroup(UpdateSkuGroupDto updateSkuGroupDto);
        Task<bool> DeleteSkuGroup(Guid id);
        Task<IList<SkuGroupSummaryDto>> GetSkuGroupsForPlace(Guid placeId);
    }
}