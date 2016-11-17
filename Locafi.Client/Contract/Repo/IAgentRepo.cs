using System.Threading.Tasks;
using Locafi.Client.Model.Dto.Agent;

namespace Locafi.Client.Contract.Repo
{
    public interface IAgentRepo
    {
        Task<AgentSummaryDto> CreateAgent(AddAgentDto item);
    }
}