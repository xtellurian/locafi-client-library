using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        public PeripheralDeviceDetailDto()
        {

        }

        public PeripheralDeviceDetailDto(PeripheralDeviceDetailDto dto) : base(dto)
        {
            if (dto == null) return;

            var type = typeof (PeripheralDeviceDetailDto);
            var properties = type.GetTypeInfo().DeclaredProperties;
            foreach (var property in properties)
            {
                var value = property.GetValue(dto);
                property.SetValue(this, value);
            }
        }
    }
}
