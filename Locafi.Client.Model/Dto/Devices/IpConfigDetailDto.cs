using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Devices
{
    public class IpConfigDetailDto : EntityDtoBase
    {    
        public string IpAddress { get; set; }

        public string SubnetMask { get; set; }

        public int? TcpPort { get; set; }

        public int? UdpPort { get; set; }

        public string MacAddress { get; set; }

        public string Hostname { get; set; }

        public IpConfigDetailDto()
        {
            
        }

        public IpConfigDetailDto(IpConfigDetailDto dto) : base(dto)
        {
            if (dto == null) return;

            var type = typeof(IpConfigDetailDto);
            var properties = type.GetTypeInfo().DeclaredProperties;
            foreach (var property in properties)
            {
                var value = property.GetValue(dto);
                property.SetValue(this, value);
            }
        }
    }
}
