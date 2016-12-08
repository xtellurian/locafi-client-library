using System;
using System.Threading.Tasks;
using Locafi.Client.Model;
using Locafi.Client.Model.Dto.Agent;
using Locafi.Client.Model.Query;

namespace Locafi.Client.Contract.Repo
{
    public interface IAgentRepo
    {
        Task<AgentSummaryDto> CreateAgent(AddAgentDto item);
        Task<AgentSummaryDto> UpdateAgent(UpdateAgentDto updateDto);
        Task<PageResult<AgentSummaryDto>> QueryAgents(string oDataQueryOptions = null);
        Task<PageResult<AgentSummaryDto>> QueryAgents(IRestQuery<AgentSummaryDto> query);
        Task<IQueryResult<AgentSummaryDto>> QueryAgentsContinuation(IRestQuery<AgentSummaryDto> query);
        Task<AgentSummaryDto> GetAgentById(Guid id);
        Task<AgentSummaryDto> GetLoggedInAgent();
    }
}