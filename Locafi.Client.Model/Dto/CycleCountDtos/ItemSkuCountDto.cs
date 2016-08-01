using Locafi.Client.Model.Dto.Skus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.CycleCountDtos
{
    public class ItemSkuCountDto : SkuSummaryDto
    {
        public ItemSkuCountDto(SkuSummaryDto skuSummaryDto, int count)
        {
            var properties = typeof(SkuSummaryDto).GetTypeInfo().DeclaredProperties;
            foreach (var property in properties)
            {
                var value = property.GetValue(skuSummaryDto);
                property.SetValue(this, value);
            }

            ItemCount = count;
        }

        public int ItemCount { get; set; }
    }
}
