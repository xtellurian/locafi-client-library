using System;
using System.Collections.Generic;
using LegacyNavigatorApi.Models.Dtos;

namespace Locafi.Client.Model.Dto.Places
{
    public class UpdatePlaceDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public IList<WriteEntityExtendedPropertyDto> PlaceExtendedPropertyList { get; set; }

        public UpdatePlaceDto()
        {
            PlaceExtendedPropertyList = new List<WriteEntityExtendedPropertyDto>();
        }
    }
}
