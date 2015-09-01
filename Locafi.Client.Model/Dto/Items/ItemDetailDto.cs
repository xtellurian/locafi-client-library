using System;
using System.Collections.Generic;

namespace Locafi.Client.Model.Dto.Items
{
    public class ItemDetailDto : ItemSummaryDto
    {
//        public Guid Id { get; set; }
//        public string Name { get; set; }

//        public Guid SkuId { get; set; }
//        public string SkuName { get; set; }

//        public Guid PlaceId { get; set; }
//        public string PlaceName { get; set; }

        public Guid? TagId { get; set; }
//        public string TagNumber { get; set; }
//        public string TagTypeName { get; set; }

        //public Guid? CreatedByUserId { get; set; }

        //public string CreatedByUserFullName { get; set; }

        //public DateTime DateCreated { get; set; }

        //public Guid? LastModifiedByUserId { get; set; }

        //public string LastModifiedByUserFullName { get; set; }

        //public DateTime? DateLastModified { get; set; }

        public Guid? ParentItemId { get; set; }
        public string ParentItemName { get; set; }

//        public Guid TypeId { get; set; }      // not needed check with puru
//        public string TypeName { get; set; }  // not needed check with puru

        public Guid? PersonId { get; set; }
        public string PersonName { get; set; }

        public string Description { get; set; }

        public IList<string> Images { get; set; }

        public IList<ReadItemExtendedPropertyDto> ItemExtendedPropertyList { get; set; }

        public ItemDetailDto()
        {
            ItemExtendedPropertyList = new List<ReadItemExtendedPropertyDto>();
        }
    }
}
