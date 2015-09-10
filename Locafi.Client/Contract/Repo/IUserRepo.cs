// This is a contract file
// Breaking changes to this contract should be accompanied by a version change in the third version number
// ie ... 1.2.42.11 -> 1.2.43.0

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.Users;
using Locafi.Client.Model.Query;

namespace Locafi.Client.Contract.Repo
{
    public interface IUserRepo
    {
        Task<IList<UserSummaryDto>> GetAllUsers();
        Task<IList<UserSummaryDto>> QueryUsers(IRestQuery<UserSummaryDto> userQuery);
        Task<UserDetailDto> GetUserById(Guid id);
        Task<UserDetailDto> CreateUser(AddUserDto addUserDto);
        Task<UserDetailDto> UpdateUser(UpdateUserDto updateUserDto);
        Task<bool> DeleteUser(Guid id);

    }
}