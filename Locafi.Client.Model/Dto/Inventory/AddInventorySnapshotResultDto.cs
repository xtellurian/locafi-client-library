using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Inventory
{
    public class AddInventorySnapshotResultDto
    {
        public InventoryDetailDto InventoryDto { get; set; }

        public IList<ItemNotificationDto> Notifications { get; set; }
    }
}
