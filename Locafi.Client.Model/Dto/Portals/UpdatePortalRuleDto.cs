using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Enums;

namespace Locafi.Client.Model.Dto.Portals
{
    public abstract class UpdatePortalRuleDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public PortalRuleType RuleType { get; set; }

        public int Timeout { get; set; }

        public IList<Guid> Antennas { get; set; }

    }
}
