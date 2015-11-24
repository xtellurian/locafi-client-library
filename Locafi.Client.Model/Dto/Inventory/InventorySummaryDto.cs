using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Inventory
{
    public class InventorySummaryDto : EntityDtoBase
    {
        public InventorySummaryDto()
        {
            
        }

        public InventorySummaryDto(InventorySummaryDto dto) : base(dto)
        {
            var properties = typeof(InventorySummaryDto).GetTypeInfo().DeclaredProperties;
            foreach (var property in properties)
            {
                var value = property.GetValue(dto);
                property.SetValue(this, value);
            }
        }
        public Guid? SkuGroupId { get; set; }
        public string Name { get; set; }

        public Guid PlaceId { get; set; }

        public bool Complete { get; set; }
    }
}
