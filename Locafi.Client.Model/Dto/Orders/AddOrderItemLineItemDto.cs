using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Orders
{
    public class AddOrderItemLineItemDto
    {
        public AddOrderItemLineItemDto()
        {
            
        }

        public AddOrderItemLineItemDto(Guid itemId)
        {
            ItemId = itemId;
        }

        public Guid ItemId { get; set; }
    }
}
