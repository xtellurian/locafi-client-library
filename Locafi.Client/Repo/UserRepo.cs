using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.Http;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Exceptions;
using Locafi.Client.Model.Dto.Users;
using Locafi.Client.Model.Query;
using Locafi.Client.Model.RelativeUri;
using Locafi.Client.Model.Responses;
using Locafi.Client.Model;

namespace Locafi.Client.Repo
{
    public class UserRepo : WebRepo, IUserRepo
    {
        public UserRepo(IAuthorisedHttpTransferConfigService configService, ISerialiserService serialiser) 
            : base(new SimpleHttpTransferer(), configService, serialiser, UserUri.ServiceName)
        {
        }

        public UserRepo(IHttpTransferer transferer, IAuthorisedHttpTransferConfigService authorisedConfigService, ISerialiserService serialiser)
           : base(transferer, authorisedConfigService, serialiser, UserUri.ServiceName)
        {
        }

        public async Task<PageResult<UserSummaryDto>> QueryUsers(string oDataQueryOptions = null)
        {
            var path = UserUri.GetUsers;

            // add the query options if required
            if (!string.IsNullOrEmpty(oDataQueryOptions))
            {
                if (oDataQueryOptions[0] != '?')
                    path += "?";

                path += oDataQueryOptions;
            }

            // make sure the query asks to return the item count
            if (!path.Contains("$count"))
            {
                if (path.Contains("?"))
                    path += "&$count=true";
                else
                    path += "?$count=true";
            }

            // run query
            var result = await Get<PageResult<UserSummaryDto>>(path);
            return result;
        }

        public async Task<PageResult<UserSummaryDto>> QueryUsers(IRestQuery<UserSummaryDto> userQuery)
        {
            var result = await QueryUsers(userQuery.AsRestQuery());
            return result;
        }

        public async Task<IQueryResult<UserSummaryDto>> QueryUsersContinuation(IRestQuery<UserSummaryDto> query)
        {
            var result = await QueryUsers(query.AsRestQuery());
            return result.AsQueryResult(query);
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
            var path = UserUri.UpdateUser;
            var result = await Post<UserDetailDto>(updateUserDto, path);
            return result;
        }

        public async Task<UserDetailDto> UpdateProfile(UpdateUserProfileDto updateDto)
        {
            var path = UserUri.UpdateProfile;
            var result = await Post<UserDetailDto>(updateDto, path);
            return result;
        }

        public async Task<UserDetailDto> UpdatePassword(UpdateUserPasswordDto updateDto)
        {
            var path = UserUri.UpdatePassword;
            var result = await Post<UserDetailDto>(updateDto, path);
            return result;
        }

        public async Task<UserDetailDto> SpawnUser(SpawnUserDto spawnDto)
        {
            var path = UserUri.SpawnUser;
            var result = await Post<UserDetailDto>(spawnDto, path);
            return result;
        }

        public async Task<UserDetailDto> GetLoggedInUser()
        {
            var path = UserUri.GetLoggedInUser;
            var result = await Get<UserDetailDto>(path);
            return result;
        }

        public async Task<bool> DeleteUser(Guid id)
        {
            var path = UserUri.DeleteUser(id);
            var result = await Delete(path);
            return result;
        }

        public async Task<bool> DeleteUserAndPerson(Guid id)
        {
            var path = UserUri.DeleteUserAndPerson(id);
            var result = await Delete(path);
            return result;
        }

        public override Task Handle(IEnumerable<CustomResponseMessage> serverMessages, HttpStatusCode statusCode, string url, string payload)
        {
            throw new UserRepoException(serverMessages, statusCode, url, payload);
        }

        public override async Task Handle(HttpResponseMessage response, string url, string payload)
        {
            throw new UserRepoException($"{url} -- {payload} -- " + await response.Content.ReadAsStringAsync());
        }
    }
}
