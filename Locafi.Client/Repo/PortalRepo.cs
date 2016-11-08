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
using Locafi.Client.Model.Query;

namespace Locafi.Client.Repo
{
    public class PortalRepo : WebRepo, IWebRepoErrorHandler, IPortalRepo
    {
        public PortalRepo(IAuthorisedHttpTransferConfigService authorisedConfigService, ISerialiserService serialiser) 
            : base(new SimpleHttpTransferer(),authorisedConfigService, serialiser, PortalUri.ServiceName)
        {
        }

        public async Task<PageResult<PortalSummaryDto>> QueryPortals(string oDataQueryOptions = null)
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

        public async Task<PageResult<PortalSummaryDto>> QueryPortals(IRestQuery<PortalSummaryDto> query)
        {
            return await QueryPortals(query.AsRestQuery());
        }
        public async Task<IQueryResult<PortalSummaryDto>> QueryPortalsContinuation(IRestQuery<PortalSummaryDto> query)
        {
            var result = await QueryPortals(query.AsRestQuery());
            return result.AsQueryResult(query);
        }

        public async Task<PortalDetailDto> GetPortal(Guid id)
        {
            var path = PortalUri.GetPortal(id);
            var result = await Get<PortalDetailDto>(path);
            return result;
        }

        public async Task<PortalDetailDto> GetPortal(string serial)
        {
            var path = PortalUri.GetPortal(serial);
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

        public async Task<PageResult<PortalRuleSummaryDto>> QueryPortalRules(string oDataQueryOptions = null)
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

        public async Task<PageResult<PortalRuleSummaryDto>> QueryPortalRules(IRestQuery<PortalRuleSummaryDto> query)
        {
            return await QueryPortalRules(query.AsRestQuery());
        }

        public async Task<IQueryResult<PortalRuleSummaryDto>> QueryPortalRulesContinuation(IRestQuery<PortalRuleSummaryDto> query)
        {
            var result = await QueryPortalRules(query.AsRestQuery());
            return result.AsQueryResult(query);
        }

        public async Task<IList<PortalRuleDetailDto>> GetRulesForPortal(Guid id)
        {
            var path = PortalUri.GetRulesForPortal(id);
            var result = await Get<List<PortalRuleDetailDto>>(path);
            return result;
        }

        public async Task<PortalRuleDetailDto> GetPortalRule(Guid id)
        {
            var path = PortalUri.GetPortalRule(id);
            var result = await Get<PortalRuleDetailDto>(path);
            return result;
        }

        public async Task<PortalRuleDetailDto> CreatePortalRule(AddPortalRuleDto addPortalRuleDto)
        {
            var path = PortalUri.CreatePortalRule;
            var result = await Post<PortalRuleDetailDto>(addPortalRuleDto, path);
            return result;
        }

        public async Task<PortalRuleDetailDto> UpdatePortalRule(UpdatePortalRuleDto updatePortalRuleDto)
        {
            var path = PortalUri.UpdatePortalRule;
            var result = await Post<PortalRuleDetailDto>(updatePortalRuleDto, path);
            return result;
        }

        public async Task DeletePortalRule(Guid id)
        {
            var path = PortalUri.DeletePortalRule(id);
            await Delete(path);
        }

        public async Task<PortalStatusDto> GetPortalStatus(Guid id)
        {
            var path = PortalUri.GetPortalStatus(id);
            var result = await Get<PortalStatusDto>(path);
            return result;
        }

        public async Task<PortalStatusDto> UpdatePortalStatus(UpdatePortalStatusDto updatePortalStatusDto)
        {
            var path = PortalUri.UpdatePortalStatus;
            var result = await Post<PortalStatusDto>(updatePortalStatusDto, path);
            return result;
        }

        public async Task<bool> UpdatePortalHeartbeat(PortalHeartbeatDto portalHeartbeatDto)
        {
            var path = PortalUri.UpdatePortalHeartbeat;
            return await Post(portalHeartbeatDto, path);            
        }

        public async Task<TagAccessResultDto> CheckAccess(CheckTagAccessDto tagAccessDto)
        {
            var path = PortalUri.CheckAccess;
            return await Post<TagAccessResultDto>(tagAccessDto, path);
        }

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
