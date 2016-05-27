using System.Collections.Generic;
using System.Reflection;
using Locafi.Client.Model.Dto.Tags;

namespace Locafi.Client.Model.Dto.Places
{
    public class PlaceDetailDto : PlaceSummaryDto
    {
        public PlaceDetailDto()
        {
            PlaceExtendedPropertyList = new List<ReadEntityExtendedPropertyDto>();
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
        public IList<TagDetailDto> PlaceTagList { get; set; }
        public IList<ReadEntityExtendedPropertyDto> PlaceExtendedPropertyList { get; set; }

    }
}
