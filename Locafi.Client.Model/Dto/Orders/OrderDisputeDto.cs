using System;
using System.Collections.Generic;

namespace Locafi.Client.Model.Dto.Orders
{
    public class OrderDisputeDto
    {
        public List<OrderItemDisputeDto> SkuItemDisputes { get; set; }
        public List<OrderItemDisputeDto> UniqueItemDisputes { get; set; }
    }

    public class OrderItemDisputeDto
    {
        public Guid LineItemId { get; set; }    // unique ItemId or SkuId
        public Guid ReasonId { get; set; }
        public string Comment { get; set; }
    }
}