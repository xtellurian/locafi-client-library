using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Devices
{
    public class SerialConfigDetailDto : EntityDtoBase
    {        
        public int ComPort { get; set; }
        public int BitRate { get; set; }
        public int DataBits { get; set; }
        public int Parity { get; set; }
        public int StopBits { get; set; }
        public int FlowControl { get; set; }

        public SerialConfigDetailDto()
        {
            
        }

        public SerialConfigDetailDto(SerialConfigDetailDto dto) : base(dto)
        {
            var type = typeof(SerialConfigDetailDto);
            var properties = type.GetTypeInfo().DeclaredProperties;
            foreach (var property in properties)
            {
                var value = property.GetValue(dto);
                property.SetValue(this, value);
            }
        }
    }
}
