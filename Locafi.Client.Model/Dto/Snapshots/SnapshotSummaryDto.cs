using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Locafi.Client.Model.Dto.Snapshots
{
    public class SnapshotSummaryDto : EntityDtoBase
    {
        public string Name { get; set; }    // friendly name for the snapshot
        public Guid PlaceId { get; set; }    // id of location this asset is in
        public DateTime StartTimeUtc { get; set; }  // time snapshot was started
        public DateTime EndTimeUtc { get; set; }    // time snapshot was completed
        public Guid UserId { get; set; }  // user who scanned the items
        [JsonConverter(typeof(StringEnumConverter))]
        public SnapshotType SnapshotType { get; set; }

        public SnapshotSummaryDto()
        {

        }

        public SnapshotSummaryDto(SnapshotSummaryDto dto) : base(dto)
        {
            var type = typeof(SnapshotSummaryDto);
            var properties = type.GetTypeInfo().DeclaredProperties;
            foreach (var property in properties)
            {
                var value = property.GetValue(dto);
                property.SetValue(this, value);
            }
        }
    }
}
