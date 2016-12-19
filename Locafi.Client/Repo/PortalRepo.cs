using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.ErrorHandlers;
using Locafi.Client.Contract.Http;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Exceptions;
using Locafi.Client.Model.RelativeUri;
using Locafi.Client.Model.Responses;
using Microsoft.OData.Core.UriParser.Semantic;
using Locafi.Client.Model;
using Locafi.Client.Model.Dto.Portals;
using Locafi.Client.Model.Dto.Portals.Clusters;
using Locafi.Client.Model.Query;

namespace Locafi.Client.Repo
{
    public class PortalRepo : WebRepo, IWebRepoErrorHandler, IPortalRepo
    {
        public PortalRepo(IAuthorisedHttpTransferConfigService authorisedConfigService, ISerialiserService serialiser) 
            : base(new SimpleHttpTransferer(),authorisedConfigService, serialiser, PortalUri.ServiceName)
        {
        }

        #region -- portals

        public async Task<PageResult<PortalSummaryDto>> GetPortals(string oDataQueryOptions = null)
        {
            var path = PortalUri.GetPortals;

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
            var result = await Get<PageResult<PortalSummaryDto>>(path);
            return result;
        }

        public async Task<PageResult<PortalSummaryDto>> GetPortals(IRestQuery<PortalSummaryDto> query)
        {
            return await GetPortals(query.AsRestQuery());
        }

        public async Task<IQueryResult<PortalSummaryDto>> GetPortalsContinuation(IRestQuery<PortalSummaryDto> query)
        {
            var result = await GetPortals(query.AsRestQuery());
            return result.AsQueryResult(query);
        }

        public async Task<PortalDetailDto> GetPortal(Guid id)
        {
            var path = PortalUri.GetPortal(id);
            var result = await Get<PortalDetailDto>(path);
            return result;
        }

        public async Task<PortalDetailDto> CreatePortal(AddPortalDto addPortalDto)
        {
            var path = PortalUri.CreatePortal;
            var result = await Post<PortalDetailDto>(addPortalDto, path);
            return result;
        }

        public async Task<PortalDetailDto> UpdatePortal(UpdatePortalDto updatePortalDto)
        {
            var path = PortalUri.UpdatePortal;
            var result = await Post<PortalDetailDto>(updatePortalDto, path);
            return result;
        }

        public async Task DeletePortal(Guid id)
        {
            var path = PortalUri.DeletePortal(id);
            await Delete(path);
        }

        #endregion

        #region -- devices

        public async Task<DeviceDetailDto> CreateDevice(AddDeviceDto deviceDto)
        {
            var path = PortalUri.CreateDevice;
            var result = await Post<DeviceDetailDto>(deviceDto, path);
            return result;
        }

        public async Task<DeviceDetailDto> UpdateDevice(UpdateDeviceDto updateDto)
        {
            var path = PortalUri.UpdateDevice;
            var result = await Post<DeviceDetailDto>(updateDto, path);
            return result;
        }

        public async Task<PageResult<DeviceSummaryDto>> GetDevices(string oDataQueryOptions = null)
        {
            var path = PortalUri.GetDevices;

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
            var result = await Get<PageResult<DeviceSummaryDto>>(path);
            return result;
        }

        public async Task<PageResult<DeviceSummaryDto>> GetDevices(IRestQuery<DeviceSummaryDto> query)
        {
            return await GetDevices(query.AsRestQuery());
        }

        public async Task<IQueryResult<DeviceSummaryDto>> GetDevicesContinuation(IRestQuery<DeviceSummaryDto> query)
        {
            var result = await GetDevices(query.AsRestQuery());
            return result.AsQueryResult(query);
        }

        public async Task<DeviceDetailDto> GetDevice(Guid id)
        {
            var path = PortalUri.GetDevice(id);
            var result = await Get<DeviceDetailDto>(path);
            return result;
        }

        public async Task DeleteDevice(Guid id)
        {
            var path = PortalUri.DeleteDevice(id);
            await Delete(path);
        }

        #endregion

        #region -- portal rules

        public async Task<PortalRuleDetailDto> CreatePortalRule(AddPortalRuleDto addPortalRuleDto)
        {
            var path = PortalUri.CreatePortalRule;
            var result = await Post<PortalRuleDetailDto>(addPortalRuleDto, path);
            return result;
        }

        public async Task<PageResult<PortalRuleSummaryDto>> GetPortalRules(string oDataQueryOptions = null)
        {
            var path = PortalUri.GetPortalRules;

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
            var result = await Get<PageResult<PortalRuleSummaryDto>>(path);
            return result;
        }

        public async Task<PageResult<PortalRuleSummaryDto>> GetPortalRules(IRestQuery<PortalRuleSummaryDto> query)
        {
            return await GetPortalRules(query.AsRestQuery());
        }

        public async Task<IQueryResult<PortalRuleSummaryDto>> GetPortalRulesContinuation(IRestQuery<PortalRuleSummaryDto> query)
        {
            var result = await GetPortalRules(query.AsRestQuery());
            return result.AsQueryResult(query);
        }

        public async Task<PortalRuleDetailDto> UpdatePortalRule(UpdatePortalRuleDto updatePortalRuleDto)
        {
            var path = PortalUri.UpdatePortalRule;
            var result = await Post<PortalRuleDetailDto>(updatePortalRuleDto, path);
            return result;
        }

        public async Task<PortalRuleDetailDto> GetPortalRule(Guid id)
        {
            var path = PortalUri.GetPortalRule(id);
            var result = await Get<PortalRuleDetailDto>(path);
            return result;
        }

        [Obsolete("Not yet implemented on the server")]
        public async Task DeletePortalRule(Guid id)
        {
            var path = PortalUri.DeletePortalRule(id);
            await Delete(path);
        }

        #endregion

        #region -- clusters

        //removed to allow for cluster cache repo

        #endregion

        public override Task Handle(IEnumerable<CustomResponseMessage> serverMessages, HttpStatusCode statusCode, string url, string payload)
        {
            throw new PortalRepoException(serverMessages, statusCode, url, payload);
        }

        public override async Task Handle(HttpResponseMessage response, string url, string payload)
        {
            throw new PortalRepoException($"{url} -- {payload} -- " + await response.Content.ReadAsStringAsync());
        }
    }
}
