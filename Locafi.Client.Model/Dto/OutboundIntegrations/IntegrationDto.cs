using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Locafi.Client.Model.Dto.OutboundIntegrations
{
    public class IntegrationDto
    {
        public IList<Guid> ListenerAgents { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public FileIntegrationPreferenceType NotificationType { get; set; }

        public object Payload { get; set; }
    }
}
