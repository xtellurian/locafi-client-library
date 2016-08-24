using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Locafi.Client.Model.Dto.Templates
{
    public class TemplateSummaryDto : EntityDtoBase
    {
        public TemplateSummaryDto()
        {
            
        }

        public TemplateSummaryDto(TemplateSummaryDto dto):base(dto)
        {
            if (dto == null) return;

            var type = typeof(TemplateSummaryDto);
            var properties = type.GetTypeInfo().DeclaredProperties;
            foreach (var property in properties)
            {
                var value = property.GetValue(dto);
                property.SetValue(this, value);
            }
        }
        public string Name { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public TemplateFor? TemplateType { get; set; }
    }
}
