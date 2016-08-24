using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Devices
{
    public class RfidReaderAntennaDto : EntityDtoBase
    {
        public string Name { get; set; }
        public int AntennaNumber { get; set; }    
        public bool IsEnabled { get; set; }
        public double TxPower { get; set; }

        public RfidReaderAntennaDto()
        {
            
        }

        public RfidReaderAntennaDto(RfidReaderAntennaDto dto) : base(dto)
        {
            if (dto == null) return;

            var type = typeof(RfidReaderAntennaDto);
            var properties = type.GetTypeInfo().DeclaredProperties;
            foreach (var property in properties)
            {
                var value = property.GetValue(dto);
                property.SetValue(this, value);
            }
        }
    }
}
