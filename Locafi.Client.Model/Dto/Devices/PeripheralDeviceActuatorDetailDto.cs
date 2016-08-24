using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Devices
{
    public class PeripheralDeviceActuatorDetailDto : EntityDtoBase
    {
        public string Name { get; set; }
        public int PortNo { get; set; }

        public PeripheralDeviceActuatorDetailDto()
        {
            
        }

        public PeripheralDeviceActuatorDetailDto(PeripheralDeviceActuatorDetailDto dto) : base(dto)
        {
            if (dto == null) return;

            var type = typeof(PeripheralDeviceActuatorDetailDto);
            var properties = type.GetTypeInfo().DeclaredProperties;
            foreach (var property in properties)
            {
                var value = property.GetValue(dto);
                property.SetValue(this, value);
            }
        }
    }
}
