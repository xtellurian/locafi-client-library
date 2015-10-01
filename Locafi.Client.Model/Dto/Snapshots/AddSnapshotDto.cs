using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Snapshots
{
    public class AddSnapshotDto
    {
        public AddSnapshotDto()
        {
            
        }
        public AddSnapshotDto(Guid placeId, string name = "")
        {
            PlaceId = placeId;
            Name = name;
            Tags = new List<SnapshotTagDto>();
            StartTimeUtc = DateTime.UtcNow;
            EndTimeUtc = DateTime.Now;
        }

        public string Name { get; set; }    // friendly name for the snapshot

        public Guid PlaceId { get; set; }    // id of location this asset is in

        public DateTime StartTimeUtc { get; set; }  // time snapshot was started
        public DateTime EndTimeUtc { get; set; }    // time snapshot was completed

        public IList<SnapshotTagDto> Tags { get; set; }    // list of tags scanned during the snapshot (tag number and tag type)


    }
}
