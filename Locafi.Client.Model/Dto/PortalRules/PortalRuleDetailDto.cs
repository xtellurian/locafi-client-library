using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Locafi.Client.Model.Dto.PortalRules
{
    public class PortalRuleDetailDto : EntityDtoBase
    {
        public string Name { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public PortalRuleType RuleType { get; set; }

        public int Timeout { get; set; }

        public IList<Guid> Antennas { get; set; }   // list of Antenna Id's to be used in this rule

        public IList<PortalRulePlaceDetailDto> PlaceRules { get; set; }
    }
}
