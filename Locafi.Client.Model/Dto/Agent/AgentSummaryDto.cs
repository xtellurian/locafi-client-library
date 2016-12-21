using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Locafi.Client.Model.Dto.Agent
{
    public class AgentSummaryDto
    {
        public Guid Id { get; set; }

        public Guid HardwareKey { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public LicenseAgentType AgentType { get; set; }

        public string Name { get; set; }
    }
}
