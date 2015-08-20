using System;
using System.Collections.Generic;

namespace Locafi.Client.Data
{
    public class UpdateItemDto
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public Guid PlaceId { get; set; }

        public Guid SkuId { get; set; }

        public IList<WriteItemExtendedPropertyDto> ItemExtendedPropertyList { get; set; }

        public UpdateItemDto()
        {
            ItemExtendedPropertyList = new List<WriteItemExtendedPropertyDto>();
        }
    }
}