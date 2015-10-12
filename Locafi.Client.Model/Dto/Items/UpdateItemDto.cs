using System;
using System.Collections.Generic;

namespace Locafi.Client.Model.Dto.Items
{
    public class UpdateItemDto
    {
        public Guid ItemId { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public Guid PlaceId { get; set; }

        public Guid SkuId { get; set; }

        public IList<WriteItemExtendedPropertyDto> ItemExtendedPropertyList { get; set; }

        public UpdateItemDto()
        {
            ItemExtendedPropertyList = new List<WriteItemExtendedPropertyDto>();
        }

        public static UpdateItemDto FromItemDetail(ItemDetailDto detail)
        {
            return new UpdateItemDto
            {
                Description = detail.Description,
                ItemExtendedPropertyList = new List<WriteItemExtendedPropertyDto>(detail.ItemExtendedPropertyList),
                ItemId = detail.Id,
                Name = detail.Name,
                PlaceId = detail.PlaceId,
                SkuId = detail.SkuId
            };
        }
    }
}