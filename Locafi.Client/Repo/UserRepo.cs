using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Data;
using Locafi.Client.Exceptions;
using Locafi.Client.Model.Query;
using Locafi.Client.Model.RelativeUri;
using Locafi.Client.Model.Responses;

namespace Locafi.Client.Repo
{
    public class UserRepo : WebRepo, IUserRepo
    {
        public UserRepo(IAuthorisedHttpTransferConfigService unauthorizedConfigService, ISerialiserService serialiser) 
            : base(unauthorizedConfigService, serialiser, UserUri.ServiceName)
        {
        }

        public async Task<IList<UserDto>> GetAllUsers()
        {
            var result = await Get<IList<UserDto>>();
            return result;
        }

        public async Task<IList<UserDto>> QueryUsers(IRestQuery<UserDto> userQuery)
        {
            var result = await QueryUsers(userQuery.AsRestQuery());
            return result;
        }

        public async Task<UserDto> GetUserById(Guid id)
        {
            throw new NotImplementedException(); // not really done properly in api - needs update
             
        }

        protected async Task<IList<UserDto>> QueryUsers(string queryString)
        {
            var result = await Get<IList<UserDto>>(queryString);
            return result;
        }

        public override Task Handle(IEnumerable<CustomResponseMessage> serverMessages)
        {
            throw new UserRepoException(serverMessages);
        }

        public override async Task Handle(HttpResponseMessage response)
        {
            throw new UserRepoException(await response.Content.ReadAsStringAsync());
        }
    }
}
