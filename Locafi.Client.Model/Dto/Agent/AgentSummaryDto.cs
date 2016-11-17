using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Agent
{
    public class AgentSummaryDto
    {
        public Guid Id { get; set; }

        public Guid HardwareKey { get; set; }

        public string AgentType { get; set; }

        public string Name { get; set; }
    }
}
