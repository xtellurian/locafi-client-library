using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Data;

namespace Locafi.Client.Services.Contract
{
    public interface IUserRepo
    {
        Task<List<UserDto>> GetAllUsers();
    }
}