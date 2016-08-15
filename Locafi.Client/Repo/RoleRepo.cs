using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.ErrorHandlers;
using Locafi.Client.Contract.Http;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Exceptions;
using Locafi.Client.Model.Query;
using Locafi.Client.Model.Responses;
using Locafi.Client.Model.Uri;
using Locafi.Client.Model;
using Locafi.Client.Model.Dto.Roles;

namespace Locafi.Client.Repo
{
    public class RoleRepo : WebRepo, IRoleRepo
    {

        public RoleRepo(IAuthorisedHttpTransferConfigService authorisedUnauthorizedConfigService, ISerialiserService serialiser) 
            : base(new SimpleHttpTransferer(), authorisedUnauthorizedConfigService, serialiser, RoleUri.ServiceName)
        {
        }

        public RoleRepo(IHttpTransferer transferer, IAuthorisedHttpTransferConfigService authorisedUnauthorizedConfigService, ISerialiserService serialiser)
           : base(transferer, authorisedUnauthorizedConfigService, serialiser, RoleUri.ServiceName)
        {
        }

        public async Task<PageResult<RoleSummaryDto>> QueryRoles(string oDataQueryOptions = null)
        {
            var path = RoleUri.GetRoles;

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
            var result = await Get<PageResult<RoleSummaryDto>>(path);
            return result;
        }

        public async Task<PageResult<RoleSummaryDto>> QueryRoles(IRestQuery<RoleSummaryDto> query)
        {
            return await QueryRoles(query.AsRestQuery());
        }

        public async Task<IQueryResult<RoleSummaryDto>> QueryRolesContinuation(IRestQuery<RoleSummaryDto> query)
        {
            var results = await QueryRoles(query.AsRestQuery());
            return results.AsQueryResult(query);
        }

        public async Task<RoleDetailDto> CreateRole(AddRoleDto addDto)
        {
            var path = RoleUri.CreateRole;
            var result = await Post<RoleDetailDto>(addDto, path);
            return result;
        }

        public async Task<RoleDetailDto> UpdateRole(UpdateRoleDto updateDto)
        {
            var path = RoleUri.UpdateRole;
            var result = await Post<RoleDetailDto>(updateDto, path);
            return result;
        }

        public async Task<RoleDetailDto> GetRoleById(Guid id)
        {
            var path = RoleUri.GetRole(id);
            var result = await Get<RoleDetailDto>(path);
            return result;
        }       

        public async Task<bool> DeleteRole(Guid id)
        {
            var path = RoleUri.DeleteRole(id);
            return await Delete(path);
        }

        public override async Task Handle(HttpResponseMessage responseMessage, string url, string payload)
        {
            throw new RoleRepoException($"{url} -- {payload} -- " + await responseMessage.Content.ReadAsStringAsync());
        }

        public override Task Handle(IEnumerable<CustomResponseMessage> serverMessages, HttpStatusCode statusCode, string url, string payload)
        {
            throw new RoleRepoException(serverMessages, statusCode, url, payload);
        }
    }
}
