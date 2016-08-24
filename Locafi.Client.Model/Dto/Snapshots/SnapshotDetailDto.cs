using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Snapshots
{
    public class SnapshotDetailDto : SnapshotSummaryDto
    {
        public List<SnapshotTagDto> Tags { get; set; }    // list of tags scanned during the snapshot (tag number and tag type)

        public List<Guid> Items { get; set; }   // list of guids for the items identified from the scanned tags

        public SnapshotDetailDto()
        {
            Id = Guid.Empty;
            Tags = new List<SnapshotTagDto>();
            Items = new List<Guid>();
        }

        public SnapshotDetailDto(SnapshotDetailDto dto) : base(dto)
        {
            if (dto == null) return;

            var type = typeof(SnapshotDetailDto);
            var properties = type.GetTypeInfo().DeclaredProperties;
            foreach (var property in properties)
            {
                var value = property.GetValue(dto);
                property.SetValue(this, value);
            }
        }
    }
}
