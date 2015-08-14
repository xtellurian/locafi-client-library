using System;
using System.Collections.Generic;

namespace Locafi.Entity.Dto
{
    public class ExtendedProperty
    {
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        public string Label { get; set; }
        public bool Required { get; set; }
        public string DataType { get; set; }
        public string Value { get; set; }
    }

    public class ItemDTO
    {
        public string Id { get; set; }    // this references the Guid field in the DB, not the ID_Asset field (must add Guid field to DB)

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

    public class PlaceDTO
    {
        public string Id { get; set; }

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

    public class PersonDTO
    {
        public string Id { get; set; }

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

    public class SnapshotDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }    // friendly name for the snapshot

        public string PlaceId { get; set; }    // id of location this asset is in (must add Guid field to DB)

        public DateTime StartTimeUtc { get; set; }  // time snapshot was started
        public DateTime? EndTimeUtc { get; set; }    // time snapshot was completed

        public string UserId { get; set; }  // user who scanned the items

        public List<SnapshotDtoTag> Tags { get; set; }    // list of tags scanned during the snapshot (tag number and tag type)

        public List<string> Items { get; set; }   // list of guids for the items identified from the scanned tags

        public SnapshotDTO()
        {
            Id = "0";
            Tags = new List<SnapshotDtoTag>() { };
            Items = new List<string>() { };
        }
    }

    public class SnapshotDtoTag
    {
        public string TagNumber { get; set; }   // tag number ie. EPC, barcode number, etc
        public string TagTypeId { get; set; }   // reference to the type of tag ie. passive UHF, barcode, RFCode, etc

        public SnapshotDtoTag()
        {
            TagTypeId = "0";
        }
    }

    public class InventoryBaseDTO
    {
        public string Name { get; set; }
        public string PlaceId { get; set; }
    }

    public class InventoryDTO : InventoryBaseDTO
    {
        public string Id { get; set; }
        public bool Complete { get; set; }
//        public string SnapshotId { get; set; }
        public List<string> SnapshotIds { get; set; }
        public List<string> FoundItemsExpected { get; set; }
        public List<string> FoundItemsUnexpected { get; set; }
        public List<string> MissingItems { get; set; }
//        public List<InventoryReasonDTO> Reasons { get; set; }
        public Dictionary<string, string> Reasons { get; set; }

        public InventoryDTO()
        {
            // initialise empty arrays
            FoundItemsExpected = new List<string>();
            FoundItemsUnexpected = new List<string>();
            MissingItems = new List<string>();
//            Reasons = new List<InventoryReasonDTO>();
            Reasons = new Dictionary<string, string>();
        }
    }

    public class InventoryReasonDTO
    {
        public string ItemId { get; set; }  // the item that the reason relates to
        public string ReasonId { get; set; } // the reason for the item
    }

    public class ItemTypeDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string GroupId { get; set; }
        public string GroupName { get; set; }

        public DateTime DateCreated { set; get; } // date item was created
        public string CreatedByUserId { set; get; } // id of the user who created it
        public DateTime DateLastModified { set; get; }  // datetime fo creation
        public string LastModifiedByUserId { set; get; }    // id of last user who modified it

        public bool? IsDeleted { get; set; }    // if item is deleted
        public bool? IsActive { get; set; }     // if item is active
    }

    public class UserDTO
    {
        public string Id { get; set; }
        public string GivenName { get; set; }
        public string Surname { get; set; }
        public string EmailAddress { get; set; }
        public string OrganisationName { get; set; }
        public string ImageUrl { get; set; }
        public string UserName { get; set; }

        public DateTime DateCreated { set; get; } // date item was created
        public string CreatedByUserId { set; get; } // id of the user who created it
        public DateTime DateLastModified { set; get; }  // datetime fo creation
        public string LastModifiedByUserId { set; get; }    // id of last user who modified it

        public bool? IsDeleted { get; set; }    // if item is deleted
        public bool? IsActive { get; set; }     // if item is active
    }

    public class ReasonDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class TagReservationDTO
    {
        public IList<string> TagNumbers { get; set;}

        public TagReservationDTO()
        {
            TagNumbers = new List<string>();
        }
    }
}