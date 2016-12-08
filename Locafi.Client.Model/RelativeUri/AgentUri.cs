using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.RelativeUri
{
    public static class AgentUri
    {
        public static string ServiceName => "Agents";
        public static string CreateAgent => "CreateAgent";
        public static string UpdateAgent => "UpdateAgent";
        public static string GetAgents => "GetFilteredAgents";
        public static string GetLoggedInAgent => "GetLoggedInAgent";
        public static string GetAgent(Guid id)
        {
            return $"GetAgent/{id}";
        }
    }
}
