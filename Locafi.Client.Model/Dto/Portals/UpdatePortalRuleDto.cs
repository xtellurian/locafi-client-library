using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Enums;

namespace Locafi.Client.Model.Dto.Portals
{
    public class UpdatePortalRuleDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public IList<WriteEntityExtendedPropertyDto> PortalRuleProperties { get; set; }

        public IList<WritePortalRuleComponentDto> ComponentList { get; set; }

        public UpdatePortalRuleDto()
        {
            PortalRuleProperties = new List<WriteEntityExtendedPropertyDto>();
            ComponentList = new List<WritePortalRuleComponentDto>();
        }

    }
}
