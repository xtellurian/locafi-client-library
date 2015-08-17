using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Locafi.Entity.Dto
{
    public class SnapshotDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }    // friendly name for the snapshot

        public string PlaceId { get; set; }    // id of location this asset is in (must add Guid field to DB)

        public DateTime StartTimeUtc { get; set; }  // time snapshot was started
        public DateTime? EndTimeUtc { get; set; }    // time snapshot was completed

        public string UserId { get; set; }  // user who scanned the items

        public List<SnapshotDtoTag> Tags { get; set; }    // list of tags scanned during the snapshot (tag number and tag type)

        public List<string> Items { get; set; }   // list of guids for the items identified from the scanned tags

        public SnapshotDTO()
        {
            Id = Guid.NewGuid();
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
}
