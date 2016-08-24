using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        public PeripheralDeviceType? DeviceType { get; set; }

        public PeripheralDeviceSummaryDto()
        {
            
        }

        public PeripheralDeviceSummaryDto(PeripheralDeviceSummaryDto dto) : base(dto)
        {
            if (dto == null) return;

            var type = typeof(PeripheralDeviceSummaryDto);
            var properties = type.GetTypeInfo().DeclaredProperties;
            foreach (var property in properties)
            {
                var value = property.GetValue(dto);
                property.SetValue(this, value);
            }
        }
    }
}
