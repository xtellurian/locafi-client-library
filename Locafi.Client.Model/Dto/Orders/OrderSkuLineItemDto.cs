using System;
using System.Collections.Generic;

namespace Locafi.Client.Model.Dto.Orders
{
    public class OrderSkuLineItemDto
    {
        public OrderSkuLineItemDto()
        {
            AllocatedTagNumbers = new List<string>();
            ReceivedTagNumbers = new List<string>();
        }
        public Guid SkuId { get; set; }
        public int Quantity { get; set; }
        public int PackingSize { get; set; }
        public IList<string> AllocatedTagNumbers { get; set; } 
        public IList<string> ReceivedTagNumbers { get; set; }
        public string Gtin { get; set; }
        public string Name { get; set; }
    }
}