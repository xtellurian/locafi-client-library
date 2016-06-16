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

        public Guid TemplateId { get; set; }

        public string TemplateName { get; set; }

        public string CompanyPrefix { get; set; }

        public string ItemReference { get; set; }

        public string CustomPrefix { get; set; }

        public string Gtin { get; set; }

        public string SkuNumber { get; set; }

        public string Thumbnail { get; set; }
    }
}
