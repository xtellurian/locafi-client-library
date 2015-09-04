using System.Collections.Generic;
using Locafi.Client.Model.Dto;
using Locafi.Client.Model.Dto.Places;

namespace Locafi.Client.Model.Conversion
{
    public static class PlaceDtoConversions
    {
        public static UpdatePlaceDto Update(this PlaceDetailDto placeDetail, string updateName = null, string updateDescription = null, IList<WriteEntityExtendedPropertyDto> extendedProperties = null )
        {
            var update = new UpdatePlaceDto
            {
                Id = placeDetail.Id,
                Description = updateDescription ?? placeDetail.Description,
                Name = updateName ?? placeDetail.Name,
                PlaceExtendedPropertyList = extendedProperties ?? new List<WriteEntityExtendedPropertyDto>(placeDetail.PlaceExtendedPropertyList)
            };
            return update;
        }
    }
}
