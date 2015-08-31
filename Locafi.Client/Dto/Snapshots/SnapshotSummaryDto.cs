using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Snapshots
{
    public class SnapshotSummaryDto : EntityDtoBase
    {
        public string Name { get; set; }    // friendly name for the snapshot

        public Guid PlaceId { get; set; }    // id of location this asset is in

        public DateTime StartTimeUtc { get; set; }  // time snapshot was started
        public DateTime? EndTimeUtc { get; set; }    // time snapshot was completed

        public Guid UserId { get; set; }  // user who scanned the items
    }
}
