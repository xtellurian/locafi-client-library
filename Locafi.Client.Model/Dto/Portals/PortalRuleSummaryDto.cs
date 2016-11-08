using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Enums;

namespace Locafi.Client.Model.Dto.Portals
{
    public class PortalRuleSummaryDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public PortalRuleType RuleType { get; set; }

        public int Timeout { get; set; }

        public Guid RfidPortalId { get; set; }

        public string RfidPortalName { get; set; }

    }
}
