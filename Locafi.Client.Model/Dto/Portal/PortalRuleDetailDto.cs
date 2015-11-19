using System;
using System.Collections.Generic;
using Locafi.Client.Model.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Locafi.Client.Model.Dto.Portal
{
    public class PortalRuleDetailDto : PortalRuleSummaryDto
    {
        public Guid RfidPortalId { get; set; }
        public string RfidPortalName { get; set; }
        public IList<Guid> Antennas { get; set; }   // list of Antenna Id's to be used in this rule

        public PortalRulePlaceDetailDto PlaceRule { get; set; }
    }
}
