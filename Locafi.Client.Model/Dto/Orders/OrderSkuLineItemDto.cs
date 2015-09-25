using System;

namespace Locafi.Client.Model.Dto.Orders
{
    public class OrderSkuLineItemDto
    {
        public Guid SkuId { get; set; }
        public int Quantity { get; set; }
        public int PackingSize { get; set; }
        public int QtyAllocated { get; set; }
        public int QtyReceived { get; set; }
        public string Gtin { get; set; }
        public string Name { get; set; }
    }
}