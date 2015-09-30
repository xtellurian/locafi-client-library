using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Devices
{
    public class PeripheralDeviceDetailDto : PeripheralDeviceSummaryDto
    {
        public IpConfigDetailDto IpConfig { get; set; }
        public SerialConfigDetailDto SerialConfig { get; set; }
        public IList<PeripheralDeviceSensorDetailDto> Sensors { get; set; }
        public IList<PeripheralDeviceActuatorDetailDto> Actuators { get; set; }
    }
}
