using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.CycleCountDtos
{
    public class CycleCountSummaryDto : EntityDtoBase
    {
        public string Name { get; set; }

        public Guid PlaceId { get; set; }

        public Guid? SkuGroupId { get; set; }

        public string SkuGroupName { get; set; }

        public bool Complete { get; set; }
    }
}
