using System;
using System.Collections.Generic;

namespace Locafi.Client.Model.Dto.PortalRules
{
    public class PortalRuleDetailDto : PortalRuleSummaryDto
    {
        public Guid RfidPortalId { get; set; }
        public string RfidPortalName { get; set; }
        public IList<Guid> Antennas { get; set; }   // list of Antenna Id's to be used in this rule

        public PortalRulePlaceDetailDto PlaceRule { get; set; }
    }
}
