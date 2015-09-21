using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Orders
{
    public class AddOrderSkuLineItemDto
    {
        public AddOrderSkuLineItemDto()
        {
            
        }

        public AddOrderSkuLineItemDto(Guid skuId, int quantity, int packingSize)
        {
            if (quantity <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(quantity), "Must be a positive integer");
            }
            SkuId = skuId;
            Quantity = quantity;
            PackingSize = packingSize;
        }

        public Guid SkuId { get; set; }

        public int Quantity { get; set; }

        public int PackingSize { get; set; }
    }
}
