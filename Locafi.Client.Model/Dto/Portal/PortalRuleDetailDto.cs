using System;
using System.Collections.Generic;
using Locafi.Client.Model.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Locafi.Client.Model.Dto.Portal
{
    public class PortalRuleDetailDto : EntityDtoBase
    {
        public string Name { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public PortalRuleType RuleType { get; set; }

        public int Timeout { get; set; }

        public IList<Guid> Antennas { get; set; }   // list of Antenna Id's to be used in this rule

        public PortalRulePlaceDetailDto PlaceRule { get; set; }
    }
}
