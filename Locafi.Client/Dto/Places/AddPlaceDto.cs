using System;
using System.Collections.Generic;

namespace Locafi.Client.Model.Dto.Places
{
    public class AddPlaceDto
    {
        public Guid TemplateId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string TagNumber { get; set; }

        public int TagType { get; set; }

        // ReSharper disable once MemberCanBePrivate.Global
        public IList<WriteEntityExtendedPropertyDto> PlaceExtendedPropertyList { get; set; }

        public AddPlaceDto()
        {
            PlaceExtendedPropertyList = new List<WriteEntityExtendedPropertyDto>();
        }

    }

}
