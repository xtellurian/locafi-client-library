// This is a contract file
// Breaking changes to this contract should be accompanied by a version change in the third version number
// ie ... 1.2.42.11 -> 1.2.43.0

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Model.Query;
using Locafi.Client.Model;
using Locafi.Client.Model.Dto.ExtendedProperties;
using Locafi.Client.Model.Dto.Roles;

namespace Locafi.Client.Contract.Repo
{
    public interface IRoleRepo
    {
        Task<PageResult<RoleSummaryDto>> QueryRoles(string oDataQueryOptions = null);
        Task<PageResult<RoleSummaryDto>> QueryRoles(IRestQuery<RoleSummaryDto> query);
        Task<IQueryResult<RoleSummaryDto>> QueryRolesContinuation(IRestQuery<RoleSummaryDto> query);
        Task<RoleDetailDto> CreateRole(AddRoleDto addDto);
        Task<RoleDetailDto> UpdateRole(UpdateRoleDto updateDto);
        Task<bool> DeleteRole(Guid placeId);
        Task<RoleDetailDto> GetRoleById(Guid id);
    }
}