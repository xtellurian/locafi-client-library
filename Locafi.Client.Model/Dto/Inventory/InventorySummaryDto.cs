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
        public string Name { get; set; }

        public Guid PlaceId { get; set; }

        public string PlaceName { get; set; }

        public Guid? SkuGroupId { get; set; }

        public string SkuGroupName { get; set; }

        public bool Complete { get; set; }

        public int ExpectedItemsCount { get; set; }

        public int MissingItemsCount { get; set; }

        public int UnexpectedItemsCount { get; set; }

        public InventorySummaryDto()
        {
            
        }

        public InventorySummaryDto(InventorySummaryDto dto) : base(dto)
        {
            if (dto == null) return;

            var properties = typeof(InventorySummaryDto).GetTypeInfo().DeclaredProperties;
            foreach (var property in properties)
            {
                var value = property.GetValue(dto);
                property.SetValue(this, value);
            }
        }
    }
}
