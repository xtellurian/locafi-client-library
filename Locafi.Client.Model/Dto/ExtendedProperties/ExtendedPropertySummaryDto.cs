using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Locafi.Client.Model.Dto.ExtendedProperties
{
    public class ExtendedPropertySummaryDto : EntityDtoBase
    {
        public ExtendedPropertySummaryDto()
        {
            
        }

        public ExtendedPropertySummaryDto(ExtendedPropertySummaryDto dto) : base(dto)
        {
            var properties = typeof(ExtendedPropertySummaryDto).GetTypeInfo().DeclaredProperties;
            foreach (var property in properties)
            {
                var value = property.GetValue(dto);
                property.SetValue(this, value);
            }
        }
        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsRequired { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public TemplateDataTypes DataType { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public TemplateFor TemplateType { get; set; }
    }
}
