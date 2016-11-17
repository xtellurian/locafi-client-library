using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Agent
{
    public class AddAgentDto
    {
        public string Name { get; set; }

        public string HardwareKey { get; set; }

        public string AgentType { get; set; }
    }
}
