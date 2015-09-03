using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Orders
{
    public class AddOrderSkuLineItemDto
    {
        public Guid SkuId { get; set; }

        public int Quantity { get; set; }

        public int PackingSize { get; set; }
    }
}
