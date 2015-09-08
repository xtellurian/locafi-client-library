﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Exceptions;
using Locafi.Client.Model.ConfigurationContract;
using Locafi.Client.Model.Dto.Users;
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

        public async Task<IList<UserSummaryDto>> GetAllUsers()
        {
            var result = await Get<IList<UserSummaryDto>>();
            return result;
        }

        public async Task<IList<UserSummaryDto>> QueryUsers(IRestQuery<UserSummaryDto> userQuery)
        {
            var result = await QueryUsers(userQuery.AsRestQuery());
            return result;
        }

        public async Task<UserDetailDto> GetUserById(Guid id)
        {
            var path = UserUri.GetUser(id);
            var result = await Get<UserDetailDto>(path);
            return result;
        }

        public async Task<UserDetailDto> CreateUser(AddUserDto addUserDto)
        {
            var path = UserUri.CreateUser;
            var result = await Post<UserDetailDto>(addUserDto, path);
            return result;
        }

        public async Task<UserDetailDto> UpdateUser(UpdateUserDto updateUserDto)
        {
            var path = UserUri.UpdateUser(updateUserDto.UserId);
            var result = await Post<UserDetailDto>(updateUserDto, path);
            return result;
        }

        public async Task<bool> DeleteUser(Guid id)
        {
            var path = UserUri.DeleteUser(id);
            var result = await Delete(path);
            return result;
        }

        protected async Task<IList<UserSummaryDto>> QueryUsers(string queryString)
        {
            var result = await Get<IList<UserSummaryDto>>(queryString);
            return result;
        }

        public override Task Handle(IEnumerable<CustomResponseMessage> serverMessages, HttpStatusCode statusCode)
        {
            throw new UserRepoException(serverMessages, statusCode);
        }

        public override async Task Handle(HttpResponseMessage response)
        {
            throw new UserRepoException(await response.Content.ReadAsStringAsync());
        }
    }
}
