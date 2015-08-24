using System;
using System.Collections.Generic;

namespace Locafi.Client.Data
{
    public class PlaceDto
    {
        public Guid Id { get; set; }

        public string ParentPlaceId { get; set; }    // Determine the parent item that this item is linked to (must add Guid field to DB)
        public string ParentPlaceName { get; set; }

        public string Name { get; set; }    // Name property for the item

        public string Description { get; set; }    // Description property for the item

        public string TagId { get; set; }    // Determine the tag that this item is linked to (not implemented in legacy DB)
        public string TagNumber { get; set; } // tag EPC
        public string TagTypeName { get; set; } // display name for the type of tag

        public List<string> ImageURIs { set; get; } // array of images that belong to this object

        //        public List<ExtendedProperty> ExtendedProperties { get; set; } // not implemented in legacy DB

        public int? UsageCount { get; set; }

        public DateTime DateCreated { set; get; } // date item was created
        public string CreatedByUserId { set; get; } // id of the user who created it
        public DateTime DateLastModified { set; get; }  // datetime fo creation
        public string LastModifiedByUserId { set; get; }    // id of last user who modified it

        public bool? IsDeleted { get; set; }    // if item is deleted
        public bool? IsActive { get; set; }     // if item is active
    }
}
