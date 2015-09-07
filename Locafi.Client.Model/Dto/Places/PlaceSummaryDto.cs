using System;
using System.Reflection;

namespace Locafi.Client.Model.Dto.Places
{
    public class PlaceSummaryDto : EntityDtoBase
    {
        public PlaceSummaryDto()
        {
            
        }

        public PlaceSummaryDto(PlaceSummaryDto dto):base(dto)
        {
            var type = typeof(PlaceSummaryDto);
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

        public string TagNumber { get; set; }
        public string TagTypeName { get; set; }
    }
}
