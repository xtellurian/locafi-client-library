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

        public Guid PortalId { get; set; }

        public string PortalName { get; set; }

        public string Name { get; set; }

        public string RuleType { get; set; }

    }
}
