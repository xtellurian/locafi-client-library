using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Locafi.Client.Model.Dto.Portals
{
    public class WritePortalRuleComponentDto
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public PortalRuleComponentType Component { get; set; }   // represents the component type enum

        public Guid Value { get; set; }   // represents the component id used
    }
}
