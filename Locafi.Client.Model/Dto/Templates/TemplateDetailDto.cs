using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Templates
{
    public class TemplateDetailDto : TemplateSummaryDto
    {
        public TemplateDetailDto()
        {
            
        }

        public TemplateDetailDto(TemplateDetailDto dto):base(dto)
        {
            var type = typeof(TemplateDetailDto);
            var properties = type.GetTypeInfo().DeclaredProperties;
            foreach (var property in properties)
            {
                var value = property.GetValue(dto);
                property.SetValue(this, value);
            }
        }
        public IList<TemplateExtendedPropertyDto> TemplateExtendedPropertyList { get; set; }
    }
}
