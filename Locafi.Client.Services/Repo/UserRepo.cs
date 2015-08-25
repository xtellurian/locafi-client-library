using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.Services;
using Locafi.Client.Data;
using Locafi.Client.Services.Odata;

namespace Locafi.Client.Services.Repo
{
    public class UserRepo : WebRepo, IUserRepo
    {
        public UserRepo(IAuthorisedHttpTransferConfigService configService, ISerialiserService serialiser) : base(configService, serialiser, "Users")
        {
        }

        public async Task<IList<UserDto>> GetAllUsers()
        {
            var result = await Get<IList<UserDto>>();
            return result;
        }

        public async Task<UserDto> GetUser(string username)
        {
            var result = await Get<UserDto>("?$filter=UserName eq '" + username + "'");
            return result;
        }

        public async Task<UserDto> GetUserById(Guid id)
        {
            var result = await Get<UserDto>("?$filter=Id eq '" + id + "'");
            return result;
        }

        public async Task<UserDto> GetUserById(string id)
        {
            var result = await Get<UserDto>("?$filter=Id eq '" + id + "'");
            return result;
        }
    }
}
