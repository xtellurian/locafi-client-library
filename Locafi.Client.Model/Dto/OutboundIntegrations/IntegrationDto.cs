using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.OutboundIntegrations
{
    public class IntegrationDto
    {
        public IList<Guid> ListenerAgents { get; set; }

        public string NotificationType { get; set; }

        public object Payload { get; set; }
    }
}
