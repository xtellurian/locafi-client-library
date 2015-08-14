using System;
using System.Collections.Generic;

namespace Locafi.Entity.Dto
{
    public class AddItemDto
    {
        public Guid? ParentItemId { get; set; }

        public Guid SkuId { get; set; }

        public Guid PlaceId { get; set; }

        public Guid? PersonId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string TagNumber { get; set; }

        public int TagType { get; set; }

        public IList<WriteItemExtendedPropertyDto> ItemExtendedPropertyList { get; set; }
    }
}