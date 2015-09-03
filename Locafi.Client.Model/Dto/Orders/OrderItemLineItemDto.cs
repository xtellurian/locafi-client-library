using System;

namespace Locafi.Client.Model.Dto.Orders
{
    public class OrderItemLineItemDto
    {
        public string TagNumber { get; set; }
        public Guid ItemId { get; set; }
        public string Name { get; set; }
        public bool IsAllocated { get; set; }
        public bool IsReceived { get; set; }
    }
}