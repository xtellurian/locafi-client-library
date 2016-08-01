using Locafi.Client.Model.Dto.Snapshots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.CycleCountDtos
{
    public class ResolveCycleCountDto : AddSnapshotDto
    {
        public Guid CycleCountId { get; set; }
    }
}
