using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Inventory
{
    public class ResolveInventoryDto
    {
        public IDictionary<Guid, Guid> Reasons { get; set; }

        public ResolveInventoryDto()
        {
            Reasons = new Dictionary<Guid, Guid>();
        }
    }
}
