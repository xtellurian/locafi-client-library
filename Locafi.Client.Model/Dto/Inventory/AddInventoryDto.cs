using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Inventory
{
    public class AddInventoryDto
    {
        public AddInventoryDto()
        {
            SnapshotIds = new List<Guid>();
        }
        public string Name { get; set; }

        public Guid PlaceId { get; set; }

        public List<Guid> SnapshotIds { get; set; }
    }
}
