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
    public class AddPeripheralDeviceDto
    {
        public string Name { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public PeripheralDeviceType DeviceType { get; set; }

        public AddIpConfigDto IpConfig { get; set; }

        public AddSerialConfigDto SerialConfig { get; set; }

        public IList<AddPeripheralDeviceSensorDto> Sensors { get; set; }

        public IList<AddPeripheralDeviceActuatorDto> Actuators { get; set; }
    }
}
