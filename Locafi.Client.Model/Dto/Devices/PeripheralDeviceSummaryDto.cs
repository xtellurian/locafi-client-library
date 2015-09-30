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
    public class PeripheralDeviceSummaryDto : EntityDtoBase
    {
        public string Name { get; set; }
        public string IpAddress { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public PeripheralDeviceType DeviceType { get; set; }
    }
}
