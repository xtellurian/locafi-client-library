using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Locafi.Entity.Dto
{
    public class PersonDTO
    {
        public Guid Id { get; set; }

        public string TagId { get; set; }    // Determine the tag that this item is linked to (not implemented in legacy DB)
        public string TagNumber { get; set; } // tag EPC
        public string TagTypeName { get; set; } // display name for the type of tag

        public string GivenName { get; set; }    // GivenName property for the item

        public string Surname { get; set; }    // Surname property for the item

        public string EmailAddress { get; set; }    // EmailAddress property for the item

        public string ImageUrl { get; set; }    // ImageUrl property for the item

        //        public List<ExtendedProperty> ExtendedProperties { get; set; } // not implemented in legacy DB

        public DateTime DateCreated { set; get; } // date item was created
        public string CreatedByUserId { set; get; } // id of the user who created it
        public DateTime DateLastModified { set; get; }  // datetime fo creation
        public string LastModifiedByUserId { set; get; }    // id of last user who modified it

        public bool? IsDeleted { get; set; }    // if item is deleted
        public bool? IsActive { get; set; }     // if item is active
    }
}
