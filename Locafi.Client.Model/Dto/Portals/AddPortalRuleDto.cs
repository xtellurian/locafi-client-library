using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Portals
{
    public class AddPortalRuleDto
    {
        public Guid PortalId { get; set; }

        public Guid TemplateId { get; set; }

        public string Name { get; set; }

        public string RuleType { get; set; }

        public IList<WriteEntityExtendedPropertyDto> ExtendedPropertyList { get; set; }

        public IList<WritePortalRuleComponentDto> ComponentList { get; set; }

        public AddPortalRuleDto()
        {
            ExtendedPropertyList = new List<WriteEntityExtendedPropertyDto>();
            ComponentList = new List<WritePortalRuleComponentDto>();
        }

    }
}
