using System;
using System.Collections.Generic;
using Locafi.Client.Model.Dto.Skus;
using Locafi.Client.Model.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Locafi.Client.Model.Dto.Tags;
using System.Linq;

namespace Locafi.Client.Model.Dto.Items
{
    public class AddItemDto
    {
        public AddItemDto()
        {
            ItemExtendedPropertyList = new List<WriteItemExtendedPropertyDto>();
            ItemTagList = new List<WriteTagDto>();
        }

        public AddItemDto(SkuDetailDto skuDetail, Guid placeId, string name = "", string description = "", 
            Guid? parentItemId = null, Guid? personId = null, string tagNumber = null, TagType tagType = TagType.PassiveRfid, List<WriteItemExtendedPropertyDto> extProps = null)
        {
            SkuId = skuDetail.Id;
            PlaceId = placeId;
            Name = name;
            Description = description;
            ParentItemId = parentItemId;
            PersonId = personId;
            ItemExtendedPropertyList = new List<WriteItemExtendedPropertyDto>();
            ItemTagList = new List<WriteTagDto>();

            if (!string.IsNullOrEmpty(tagNumber))
                ItemTagList.Add(new WriteTagDto() { TagNumber = tagNumber, TagType = tagType });

            // popultate the extended properties
            foreach (var extProp in skuDetail.SkuExtendedPropertyList.Where(s => !s.IsSkuLevelProperty))
            {
                if (extProps != null && extProps.Select(p => p.ExtendedPropertyId).Contains(extProp.ExtendedPropertyId))
                    ItemExtendedPropertyList.Add(extProps.First(p => p.ExtendedPropertyId == extProp.ExtendedPropertyId));
                else
                {
                    var newProp = new WriteItemExtendedPropertyDto()
                    {
                        ExtendedPropertyId = extProp.ExtendedPropertyId
                    };
                    
                    switch (extProp.ExtendedPropertyDataType)
                    {
//                        case TemplateDataTypes.AutoId: newProp.Value = new Random(DateTime.UtcNow.Millisecond).Next().ToString();  break;
                        case TemplateDataTypes.Bool: newProp.Value = true.ToString(); break;
                        case TemplateDataTypes.DateTime: newProp.Value = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssK"); break;
                        case TemplateDataTypes.Decimal: newProp.Value = (((double)new Random(DateTime.UtcNow.Millisecond).Next())/10.0).ToString(); break;
                        case TemplateDataTypes.Number: newProp.Value = new Random(DateTime.UtcNow.Millisecond).Next().ToString(); break;
                        case TemplateDataTypes.String: newProp.Value = Guid.NewGuid().ToString(); break;
                    }

                    ItemExtendedPropertyList.Add(newProp);
                }
            }

        }

        public Guid? ParentItemId { get; set; }

        public Guid SkuId { get; set; }

        public Guid PlaceId { get; set; }

        public Guid? PersonId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public IList<WriteTagDto> ItemTagList { get; set; }

        public IList<WriteItemExtendedPropertyDto> ItemExtendedPropertyList { get; set; }
    }
}