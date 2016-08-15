using Locafi.Client.Model.Dto.Skus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.CycleCountDtos
{
    public class CycleCountDetailDto : CycleCountSummaryDto
    {
        public List<SkuSummaryDto> SelectedSkus { get; set; }

        public List<ItemSkuCountDto> PresentItems { get; set; }

        public List<ItemSkuCountDto> MovedItems { get; set; }

        public List<ItemSkuCountDto> CreatedItems { get; set; }

        public CycleCountDetailDto()
        {
            PresentItems = new List<ItemSkuCountDto>();
            MovedItems = new List<ItemSkuCountDto>();
            CreatedItems = new List<ItemSkuCountDto>();
        }

    }
}
