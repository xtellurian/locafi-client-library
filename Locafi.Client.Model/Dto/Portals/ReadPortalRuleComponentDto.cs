using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Portals
{
    public class ReadPortalRuleComponentDto
    {
        public string Component { get; set; }

        public Guid? ValueId { get; set; }

        public string ValueName { get; set; }
    }
}
