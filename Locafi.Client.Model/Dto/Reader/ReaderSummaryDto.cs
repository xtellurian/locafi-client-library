using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Reader
{
    public class ReaderSummaryDto : EntityDtoBase
    {
        public ReaderSummaryDto()
        {
            
        }

        public ReaderSummaryDto(ReaderSummaryDto dto) : base(dto)
        {
            var type = typeof(ReaderSummaryDto);
            var properties = type.GetTypeInfo().DeclaredProperties;
            foreach (var property in properties)
            {
                var value = property.GetValue(dto);
                property.SetValue(this, value);
            }
        }
        public string Name { get; set; }

        public string IPAddress { get; set; }

        public string ReaderType { get; set; }  // ReaderType Enum

        public string SerialNumber { get; set; }
    }
}
