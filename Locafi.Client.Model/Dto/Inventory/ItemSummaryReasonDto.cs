using Locafi.Client.Model.Dto.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Locafi.Client.Model.Dto.Inventory
{
    public class ItemSummaryReasonDto : ItemSummaryDto
    {
        public Guid ReasonId { get; set; }

        public ItemSummaryReasonDto(ItemSummaryDto summaryDto)
        {
            var properties = typeof(ItemSummaryDto).GetTypeInfo().DeclaredProperties;
            foreach (var property in properties)
            {
                var value = property.GetValue(summaryDto);
                property.SetValue(this, value);
            }
        }
    }
}
