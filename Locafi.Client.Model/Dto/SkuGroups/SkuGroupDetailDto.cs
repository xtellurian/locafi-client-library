using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.Places;
using Locafi.Client.Model.Dto.Skus;

namespace Locafi.Client.Model.Dto.SkuGroups
{
    public class SkuGroupDetailDto : SkuGroupSummaryDto
    {
        public IList<SkuSummaryDto> Skus { get; set; }
        public IList<PlaceSummaryDto> Places { get; set; }
    }
}
