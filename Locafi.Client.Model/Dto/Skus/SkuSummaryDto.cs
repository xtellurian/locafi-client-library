using System;
using System.Reflection;

namespace Locafi.Client.Model.Dto.Skus
{
    public class SkuSummaryDto : EntityDtoBase
    {
        public SkuSummaryDto()
        {
            
        }

        public SkuSummaryDto(SkuSummaryDto dto):base(dto)
        {
            var type = typeof(SkuSummaryDto);
            var properties = type.GetTypeInfo().DeclaredProperties;
            foreach (var property in properties)
            {
                var value = property.GetValue(dto);
                property.SetValue(this, value);
            }
        }
        public string Name { get; set; }

        public string Gtin { get; set; }

        public Guid TemplateId { get; set; }
        
        public string TemplateName { get; set; }
    }
}
