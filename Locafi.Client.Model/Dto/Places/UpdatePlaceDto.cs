using System;
using System.Collections.Generic;

namespace Locafi.Client.Model.Dto.Places
{
    public class UpdatePlaceDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Guid? ParentPlaceId { get; set; }

        public Guid TemplateId { get; set; }

        public IList<WriteEntityExtendedPropertyDto> PlaceExtendedPropertyList { get; set; }

        public UpdatePlaceDto()
        {
            PlaceExtendedPropertyList = new List<WriteEntityExtendedPropertyDto>();
        }

        public UpdatePlaceDto(PlaceDetailDto detail)
        {
            Id = detail.Id;
            Description = detail.Description;
            Name = detail.Name;
            ParentPlaceId = detail.ParentPlaceId;
            TemplateId = detail.TemplateId;
            PlaceExtendedPropertyList = new List<WriteEntityExtendedPropertyDto>(detail.PlaceExtendedPropertyList);
        }

        public static UpdatePlaceDto FromPlaceDetail(PlaceDetailDto detail)
        {
            return new UpdatePlaceDto
            {
                Description = detail.Description,
                Id = detail.Id,
                Name = detail.Name,
                PlaceExtendedPropertyList = new List<WriteEntityExtendedPropertyDto>(detail.PlaceExtendedPropertyList),
                ParentPlaceId = detail.ParentPlaceId,
                TemplateId = detail.TemplateId
            };
        }
    }
}
