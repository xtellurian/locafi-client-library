using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Data;
using Locafi.Client.Services.Contract;
using Locafi.Client.Services.Odata;

namespace Locafi.Client.Services.Repo
{
    public class UserRepo : WebRepo<ODataCollection<UserDto>>, IUserRepo
    {
        public UserRepo(IHttpTransferConfigService configService, ISerialiserService serialiser) : base(configService, serialiser, "Users")
        {
        }

        public async Task<List<UserDto>> GetAllUsers()
        {
            var result = await Get();
            return result.Value;
        }

        public async Task<UserDto> GetUser(string username)
        {
            var result = await Get("?$filter=UserName eq '" + username + "'");
            return result.Value.First();
        }

        public async Task<UserDto> GetUserById(Guid id)
        {
            var result = await Get("?$filter=Id eq '" + id + "'");
            return result.Value.First();
        }

        public async Task<UserDto> GetUserById(string id)
        {
            var result = await Get("?$filter=Id eq '" + id + "'");
            return result.Value.First();
        }
    }
}
