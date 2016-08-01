using Locafi.Client.Model.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Snapshots
{
    public class AddSnapshotDto
    {
        public DateTime StartTime { get; set; }  // time snapshot was started
        public DateTime EndTime { get; set; }    // time snapshot was completed

        [JsonConverter(typeof(StringEnumConverter))]
        public SnapshotType SnapshotType { get; set; }  // defines if this is a snapshot of tags to add or remove from an operation

        public IList<SnapshotTagDto> Tags { get; set; }    // list of tags scanned during the snapshot (tag number and tag type)

        public AddSnapshotDto()
        {
            Tags = new List<SnapshotTagDto>();
            StartTime = DateTime.UtcNow;
            EndTime = DateTime.Now;
            SnapshotType = Enums.SnapshotType.Add;
        }
    }
}
