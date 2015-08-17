using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Locafi.Entity.Dto
{
    public class ItemDTO
    {
        public Guid Id { get; set; }    // this references the Guid field in the DB, not the ID_Asset field (must add Guid field to DB)

        public string ParentItemId { get; set; }    // Determine the parent item that this item is linked to (must add Guid field to DB)
        public string ParentItemName { get; set; }

        public string TypeId { get; set; }    // This represents the Sku category for the item (must add Guid field to DB)
        public string TypeName { get; set; }

        public string PlaceId { get; set; }    // id of location this asset is in (must add Guid field to DB)
        public string PlaceName { get; set; }

        public string PersonId { get; set; }    // id of user/employee that is associated with this item (must add Guid field to DB)
        public string PersonName { get; set; }

        public string Name { get; set; }    // Name property for the item

        public string Description { get; set; }    // Description property for the item

        public string TagId { get; set; }    // Determine the tag that this item is linked to (not implemented in legacy DB)
        public string TagNumber { get; set; } // tag EPC
        public string TagTypeName { get; set; } // display name for the type of tag

        public List<string> ImageURIs { set; get; } // array of images that belong to this object

        //        public List<ExtendedProperty> ExtendedProperties { get; set; } // not implemented in legacy DB

        public DateTime? DateCreated { set; get; } // date item was created
        public string CreatedByUserId { set; get; } // id of the user who created it
        public DateTime? DateLastModified { set; get; }  // datetime fo creation
        public string LastModifiedByUserId { set; get; }    // id of last user who modified it

        public bool? IsDeleted { get; set; }    // if item is deleted
        public bool? IsActive { get; set; }     // if item is active

        public string Units { get; set; }
        public int? UnitValue { get; set; }
    }
}
