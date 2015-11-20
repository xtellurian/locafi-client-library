using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.SkuGroups;
using Locafi.Client.Model.Query;

namespace Locafi.Client.Contract.Repo
{
    public interface ISkuGroupRepo
    {
        Task<IQueryResult<SkuGroupNameDetailDto>> QuerySkuGroupNames(IRestQuery<SkuGroupNameDetailDto> query);
        Task<SkuGroupNameDetailDto> GetNameById(Guid id);
        Task<SkuGroupNameDetailDto> CreateSkuGroupName(AddSkuGroupNameDto addSkuGroupNameDto);
        Task<SkuGroupNameDetailDto> UpdateSkuGroupName(Guid id, UpdateSkuGroupNameDto updateSkuGroupNameDto);
        Task<bool> DeleteSkuGroupName(Guid id);
        Task<IQueryResult<SkuGroupSummaryDto>> QuerySkuGroups(IRestQuery<SkuGroupSummaryDto> query);
        Task<SkuGroupDetailDto> GetSkuGroupDetail(Guid id);
        Task<SkuGroupDetailDto> CreateSkuGroup(AddSkuGroupDto addSkuGroupDto);
        Task<SkuGroupDetailDto> UpdateSkuGroup(Guid id, UpdateSkuGroupDto updateSkuGroupDto);
        Task<bool> DeleteSkuGroup(Guid id);
        Task<IList<SkuGroupSummaryDto>> GetSkuGroupsForPlace(Guid placeId);
    }
}