using Locafi.Client.Model.Dto.Snapshots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Inventory
{
    public class AddInventorySnapshotDto : AddSnapshotDto
    {
        public Guid InventoryId { get; set; }

        public AddInventorySnapshotDto(Guid inventoryId, AddSnapshotDto snapshotDto)
        {
            InventoryId = inventoryId;

            if (snapshotDto == null) return;

            var properties = typeof(AddSnapshotDto).GetTypeInfo().DeclaredProperties;
            foreach (var property in properties)
            {
                var value = property.GetValue(snapshotDto);
                property.SetValue(this, value);
            }
        }
    }
}
