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
using Locafi.Client.Model;
using Locafi.Client.Model.Dto.Agent;
using Locafi.Client.Model.Query;
using Locafi.Client.Model.RelativeUri;
using Locafi.Client.Model.Responses;

namespace Locafi.Client.Repo
{
    public class AgentRepo : WebRepo, IAgentRepo
    {
        public AgentRepo(IHttpTransferer transferer, IAuthorisedHttpTransferConfigService authorisedConfigService, ISerialiserService serialiser, string service) : base(transferer, authorisedConfigService, serialiser, service)
        {
        }

        public AgentRepo(IHttpTransferer transferer, IHttpTransferConfigService configService, ISerialiserService serialiser, string service) : base(transferer, configService, serialiser, service)
        {
        }

        public async Task<AgentSummaryDto> CreateAgent(AddAgentDto item)
        {
            var path = AgentUri.CreateAgent;
            var result = await Post<AgentSummaryDto>(item, path);
            return result;
        }

        public async Task<AgentSummaryDto> UpdateAgent(UpdateAgentDto updateDto)
        {
            var path = AgentUri.UpdateAgent;
            var result = await Post<AgentSummaryDto>(updateDto, path);
            return result;
        }

        public async Task<PageResult<AgentSummaryDto>> QueryAgents(string oDataQueryOptions = null)
        {
            var path = AgentUri.GetAgents;

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
            var items = await Get<PageResult<AgentSummaryDto>>(path);
            return items;
        }

        public async Task<PageResult<AgentSummaryDto>> QueryAgents(IRestQuery<AgentSummaryDto> query)
        {
            return await QueryAgents(query.AsRestQuery());
        }

        public async Task<IQueryResult<AgentSummaryDto>> QueryAgentsContinuation(IRestQuery<AgentSummaryDto> query)
        {
            var result = await QueryAgents(query.AsRestQuery());
            return result.AsQueryResult(query);
        }

        public async Task<AgentSummaryDto> GetAgentById(Guid id)
        {
            var path = AgentUri.GetAgent(id);
            var result = await Get<AgentSummaryDto>(path);
            return result;
        }

        public override Task Handle(IEnumerable<CustomResponseMessage> serverMessages, HttpStatusCode statusCode, string url, string payload)
        {
            throw new AgentRepoException(serverMessages, statusCode, url, payload);
        }

        public override async Task Handle(HttpResponseMessage response, string url, string payload)
        {
            throw new AgentRepoException($"{url} -- {payload} -- " + await response.Content.ReadAsStringAsync());
        }
    }
}
