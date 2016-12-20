using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Portals
{
    public class PortalDetailDto : PortalSummaryDto
    {
        public IList<DeviceSummaryDto> PortalDevices { get; set; }

        public IList<PortalRuleSummaryDto> PortalRules { get; set; }

        public PortalDetailDto()
        {
            PortalDevices = new List<DeviceSummaryDto>();
        }

        public int ReaderConnectBackoffDelay { get; set; }

        public int OnlineTestPingDelay { get; set; }

        public int OnlineThreshold { get; set; }

        public int OfflineThreshold { get; set; }

        public int CacheFlushAmount { get; set; }

        public int CacheFlushInterval { get; set; }
    }
}
