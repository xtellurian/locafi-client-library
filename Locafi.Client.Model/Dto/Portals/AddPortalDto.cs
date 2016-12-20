using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Portals
{
    public class AddPortalDto
    {
        public string Name { get; set; }

        public string HardwareKey { get; set; }

        public int ReaderConnectBackoffDelay { get; set; }

        public int OnlineTestPingDelay { get; set; }

        public int OnlineThreshold { get; set; }

        public int OfflineThreshold { get; set; }

        public int CacheFlushAmount { get; set; }

        public int CacheFlushInterval { get; set; }
    }
}
