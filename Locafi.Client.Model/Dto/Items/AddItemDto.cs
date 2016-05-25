using System;
using System.Collections.Generic;
using Locafi.Client.Model.Dto.Skus;
using Locafi.Client.Model.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Locafi.Client.Model.Dto.Tags;

namespace Locafi.Client.Model.Dto.Items
{
    public class AddItemDto
    {

        public AddItemDto(Guid skuId, Guid placeId, string name = "", string description = "", 
            Guid? parentItemId = null, Guid? personId = null, string tagNumber = null, TagType tagType = TagType.PassiveRfid )
        {
            SkuId = skuId;
            PlaceId = placeId;
            Name = name;
            Description = description;
            ParentItemId = parentItemId;
            PersonId = personId;
            ItemExtendedPropertyList = new List<WriteItemExtendedPropertyDto>();
            ItemTagList = new List<WriteTagDto>();
           
        }

        public Guid? ParentItemId { get; set; }

        public Guid SkuId { get; set; }

        public Guid PlaceId { get; set; }

        public Guid? PersonId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        //public string TagNumber { get; set; }

        //[JsonConverter(typeof(StringEnumConverter))]
        //public TagType TagType { get; set; }

        public IList<WriteTagDto> ItemTagList { get; set; }

        public IList<WriteItemExtendedPropertyDto> ItemExtendedPropertyList { get; set; }
    }
}