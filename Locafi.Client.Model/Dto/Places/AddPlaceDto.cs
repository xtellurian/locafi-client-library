using System;
using System.Collections.Generic;
using Locafi.Client.Model.Dto.Tags;
using Locafi.Client.Model.Dto.Templates;
using Locafi.Client.Model.Enums;

namespace Locafi.Client.Model.Dto.Places
{
    public class AddPlaceDto
    {
        public Guid TemplateId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Guid? ParentPlaceId { get; set; }

        public IList<WriteTagDto> PlaceTagList { get; set; }

        // ReSharper disable once MemberCanBePrivate.Global
        public IList<WriteEntityExtendedPropertyDto> PlaceExtendedPropertyList { get; set; }

        public AddPlaceDto()
        {
            PlaceExtendedPropertyList = new List<WriteEntityExtendedPropertyDto>();
            PlaceTagList = new List<WriteTagDto>();
        }

        public AddPlaceDto(TemplateDetailDto template)
        {
            TemplateId = template.Id;
            PlaceExtendedPropertyList = new List<WriteEntityExtendedPropertyDto>();
            PlaceTagList = new List<WriteTagDto>();

            // popultate the extended properties
            foreach (var extProp in template.TemplateExtendedPropertyList)
            {
                var newProp = new WriteEntityExtendedPropertyDto()
                {
                    ExtendedPropertyId = extProp.ExtendedPropertyId
                };

                switch (extProp.ExtendedPropertyDataType)
                {
                    case TemplateDataTypes.AutoId: newProp.Value = new Random().Next().ToString(); break;
                    case TemplateDataTypes.Bool: newProp.Value = true.ToString(); break;
                    case TemplateDataTypes.DateTime: newProp.Value = DateTime.UtcNow.ToString(); break;
                    case TemplateDataTypes.Decimal: newProp.Value = (((double)new Random().Next()) / 10.0).ToString(); break;
                    case TemplateDataTypes.Number: newProp.Value = new Random().Next().ToString(); break;
                    case TemplateDataTypes.String: newProp.Value = Guid.NewGuid().ToString(); break;
                }

                PlaceExtendedPropertyList.Add(newProp);
            }
        }

    }

}
