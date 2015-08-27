using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Data;
using Locafi.Client.Model.Query;

namespace Locafi.Client.Contract.Services
{
    public interface IUserRepo
    {
        Task<IList<UserDto>> GetAllUsers();
        Task<IList<UserDto>> QueryUsers(IRestQuery<UserDto> userQuery);
    }
}