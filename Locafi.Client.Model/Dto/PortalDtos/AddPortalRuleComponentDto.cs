using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.PortalDtos
{
    public class AddPortalRuleComponentDto
    {
        public string Component { get; set; }   // represents the component type enum

        public Guid Value { get; set; }   // represents the component id used

    }
}
