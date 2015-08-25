using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LegacyNavigatorApi.Models.Dtos;
using Locafi.Client.Model.Dto.Places;

namespace Locafi.Client.Model.Extensions
{
    public static class PlaceDtoGenerationExtensions
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
