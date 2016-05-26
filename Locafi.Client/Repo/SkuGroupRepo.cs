using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.Http;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Exceptions;
using Locafi.Client.Model.Dto.SkuGroups;
using Locafi.Client.Model.Query;
using Locafi.Client.Model.RelativeUri;
using Locafi.Client.Model.Responses;
using Locafi.Client.Model;

namespace Locafi.Client.Repo
{
    public class SkuGroupRepo : WebRepo, ISkuGroupRepo
    {
        public SkuGroupRepo(IAuthorisedHttpTransferConfigService authorisedConfigService, ISerialiserService serialiser)
            : base(new SimpleHttpTransferer(), authorisedConfigService, serialiser, SkuGroupUri.ServiceName)
        {
        }
        #region Sku Group Names
        public async Task<PageResult<SkuGroupNameDetailDto>> QuerySkuGroupNames(string oDataQueryOptions = null)
        {
            var path = SkuGroupUri.Names.GetNames;

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
            var result = await Get<PageResult<SkuGroupNameDetailDto>>(path);
            return result;
        }

        public async Task<PageResult<SkuGroupNameDetailDto>> QuerySkuGroupNames(IRestQuery<SkuGroupNameDetailDto> query)
        {
            return await QuerySkuGroupNames(query.AsRestQuery());
        }

        public async Task<IQueryResult<SkuGroupNameDetailDto>> QuerySkuGroupNamesContinuation(IRestQuery<SkuGroupNameDetailDto> query)
        {
            var result = await QuerySkuGroupNames(query.AsRestQuery());
            return result.AsQueryResult(query);
        }

        public async Task<SkuGroupNameDetailDto> GetNameById(Guid id)
        {
            var path = SkuGroupUri.Names.GetSkuGroupNameDetail(id);
            var result = await Get<SkuGroupNameDetailDto>(path);
            return result;
        }

        public async Task<SkuGroupNameDetailDto> CreateSkuGroupName(AddSkuGroupNameDto addSkuGroupNameDto)
        {
            var path = SkuGroupUri.Names.CreateSkuGroupName;
            var result = await Post<SkuGroupNameDetailDto>(addSkuGroupNameDto, path);
            return result;
        }

        public async Task<SkuGroupNameDetailDto> UpdateSkuGroupName(UpdateSkuGroupNameDto updateSkuGroupNameDto)
        {
            var path = SkuGroupUri.Names.UpdateSkuGroupName;
            var result = await Post<SkuGroupNameDetailDto>(updateSkuGroupNameDto, path);
            return result;
        }

        public async Task<bool> DeleteSkuGroupName(Guid id)
        {
            var path = SkuGroupUri.Names.DeleteSkuGroupName(id);
            var result = await Delete(path);
            return result;
        }
        #endregion

        public async Task<PageResult<SkuGroupSummaryDto>> QuerySkuGroups(string oDataQueryOptions = null)
        {
            var path = SkuGroupUri.GetSkuGroups;

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
            var result = await Get<PageResult<SkuGroupSummaryDto>>(path);
            return result;
        }

        public async Task<PageResult<SkuGroupSummaryDto>> QuerySkuGroups(IRestQuery<SkuGroupSummaryDto> query)
        {
            return await QuerySkuGroups(query.AsRestQuery());
        }

        public async Task<IQueryResult<SkuGroupSummaryDto>> QuerySkuGroupsContinuation(IRestQuery<SkuGroupSummaryDto> query )
        {
            var result = await QuerySkuGroups(query.AsRestQuery());
            return result.AsQueryResult(query);
        }

        public async Task<IList<SkuGroupSummaryDto>> GetSkuGroupsForPlace(Guid placeId)
        {
            var path = SkuGroupUri.GetSkyGroupsForPlace(placeId);
            var result = await Get<List<SkuGroupSummaryDto>>(path);
            return result;
        }

        public async Task<SkuGroupDetailDto> GetSkuGroupDetail(Guid id)
        {
            var path = SkuGroupUri.GetSkuGroupDetail(id);
            var result = await Get<SkuGroupDetailDto>(path);
            return result;
        }

        public async Task<SkuGroupDetailDto> CreateSkuGroup(AddSkuGroupDto addSkuGroupDto)
        {
            var path = SkuGroupUri.CreateSkuGroup;
            var result = await Post<SkuGroupDetailDto>(addSkuGroupDto, path);
            return result;
        }

        public async Task<SkuGroupDetailDto> UpdateSkuGroup(UpdateSkuGroupDto updateSkuGroupDto)
        {
            var path = SkuGroupUri.UpdateSkuGroup;
            var result = await Post<SkuGroupDetailDto>(updateSkuGroupDto, path);
            return result;
        }

        public async Task<bool> DeleteSkuGroup(Guid id)
        {
            var path = SkuGroupUri.DeleteSkuGroup(id);
            var result = await Delete(path);
            return result;
        }

        public override Task Handle(IEnumerable<CustomResponseMessage> serverMessages, HttpStatusCode statusCode, string url, string payload)
        {
            throw new SkuGroupRepoException(serverMessages, statusCode, url, payload);
        }

        public override async Task Handle(HttpResponseMessage response, string url, string payload)
        {
            throw new SkuGroupRepoException($"{url} -- {payload} -- " + await response.Content.ReadAsStringAsync());
        }
    }
}
