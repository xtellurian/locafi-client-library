using System.Collections.Generic;
using System.Reflection;

namespace Locafi.Client.Model.Dto.Places
{
    public class PlaceDetailDto : PlaceSummaryDto
    {
        public PlaceDetailDto()
        {
            
        }

        public PlaceDetailDto(PlaceDetailDto dto): base(dto)
        {
            var type = typeof(PlaceDetailDto);
            var properties = type.GetTypeInfo().DeclaredProperties;
            foreach (var property in properties)
            {
                var value = property.GetValue(dto);
                property.SetValue(this, value);
            }
        }
        public string Description { get; set; }
        public int TagType { get; set; }

        public long UsageCount { get; set; }

        public IList<ReadEntityExtendedPropertyDto> PlaceExtendedPropertyList { get; set; }

    }
}
