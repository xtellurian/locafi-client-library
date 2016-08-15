using Locafi.Client.Model.Dto.Snapshots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.CycleCountDtos
{
    public class ResolveCycleCountDto : AddSnapshotDto
    {
        public Guid CycleCountId { get; set; }

        public ResolveCycleCountDto(AddSnapshotDto dto)
        {
            var properties = typeof(AddSnapshotDto).GetTypeInfo().DeclaredProperties;
            foreach (var property in properties)
            {
                var value = property.GetValue(dto);
                property.SetValue(this, value);
            }
        }
    }
}
