using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Inventory
{
    public class InventoryDetailDto : InventorySummaryDto
    {
        public InventoryDetailDto()
        {
            // initialise empty arrays
            FoundItemsExpected = new List<Guid>();
            FoundItemsUnexpected = new List<Guid>();
            MissingItems = new List<Guid>();
            Reasons = new Dictionary<Guid, Guid>();
        }

        public InventoryDetailDto(InventoryDetailDto dto) : base(dto)
        {
            var properties = typeof(InventoryDetailDto).GetTypeInfo().DeclaredProperties;
            foreach (var property in properties)
            {
                var value = property.GetValue(dto);
                property.SetValue(this, value);
            }
        }

        public List<Guid> SnapshotIds { get; set; }
        public List<Guid> FoundItemsExpected { get; set; }
        public List<Guid> FoundItemsUnexpected { get; set; }
        public List<Guid> MissingItems { get; set; }
        public Dictionary<Guid, Guid> Reasons { get; set; } // item id, reason id

        
    }
}
