using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Locafi.Client.Model.Dto.Devices
{
    public class RfidReaderDetailDto : RfidReaderSummaryDto
    {
        public PeripheralDeviceDetailDto PeripherialDevice { get; set; }

        public IpConfigDetailDto IpConfig { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ReaderMode ReaderMode { get; set; }  // ReaderMode Enum

        [JsonConverter(typeof(StringEnumConverter))]
        public SearchMode SearchMode { get; set; }  // SearchMode Enum

        public int Session { get; set; }    // Session number (0 - 3) to use for the inventory operation for this configuration

        public int PopulationEstimate { get; set; } // An estimate of the tag population in view of the RF field of the antenna (0 - 65535)

        public bool IsLdcEnabled { get; set; }

        public int LdcPingInterval { get; set; }    // Specifies in milliseconds how frequently the Reader will rescan the field of view for tags.(0 - 65535)

        public int LdcFieldTimeout { get; set; }    // Specifies in milliseconds the time the Reader will wait before entering low duty cycle mode. (0 - 65535)

        public List<RfidReaderAntennaDto> Antennas { get; set; }
    }
}
