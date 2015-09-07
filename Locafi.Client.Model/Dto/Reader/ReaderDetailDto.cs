using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Reader
{
    public class ReaderDetailDto : ReaderSummaryDto
    {
        public ReaderDetailDto()
        {
            
        }

        public ReaderDetailDto(ReaderDetailDto dto) : base(dto)
        {
            var type = typeof(ReaderDetailDto);
            var properties = type.GetTypeInfo().DeclaredProperties;
            foreach (var property in properties)
            {
                var value = property.GetValue(dto);
                property.SetValue(this, value);
            }
        }
        public string Description { get; set; }

        public List<AntennaConfigDto> AntennaConfigs { get; set; }

        public string LoginName { get; set; }

        public string Password { get; set; }

        public string ReaderMode { get; set; }  // ReaderMode Enum //TODO: make enum
    }
}
