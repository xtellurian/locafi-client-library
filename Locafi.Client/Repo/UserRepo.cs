﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Data;
using Locafi.Client.Model.Query;
using Locafi.Client.Repo;
using Microsoft.OData.Core.UriParser.Semantic;

namespace Locafi.Client.Services.Repo
{
    public class UserRepo : WebRepo, IUserRepo
    {
        public UserRepo(IAuthorisedHttpTransferConfigService unauthorizedConfigService, ISerialiserService serialiser) : base(unauthorizedConfigService, serialiser, "Users")
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
    }
}
