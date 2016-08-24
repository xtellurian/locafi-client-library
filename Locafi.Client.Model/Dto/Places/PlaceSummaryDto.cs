using System;
using System.Reflection;
using Locafi.Client.Model.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Locafi.Client.Model.Dto.Places
{
    public class PlaceSummaryDto : EntityDtoBase
    {
        public PlaceSummaryDto()
        {
            
        }

        public PlaceSummaryDto(PlaceSummaryDto dto):base(dto)
        {
            if (dto == null) return;

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
        [JsonConverter(typeof(StringEnumConverter))]
        public TagType? TagType { get; set; }

        public Guid? ParentPlaceId { get; set; }
        public string ParentPlaceName { get; set; }
    }
}
