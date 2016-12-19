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
    public class ReadPortalRuleComponentDto
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public PortalRuleComponentType Component { get; set; }

        public Guid? ValueId { get; set; }

        public string ValueName { get; set; }
    }
}
