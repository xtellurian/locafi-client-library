using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Locafi.Client.Model.Dto.Orders
{
    public class OrderDisputeDto
    {
        public OrderDisputeDto()
        {
            SkuItemDisputes = new List<OrderItemDisputeDto>();
            UniqueItemDisputes = new List<OrderItemDisputeDto>();
        }
        public IList<OrderItemDisputeDto> SkuItemDisputes { get; set; }
        public IList<OrderItemDisputeDto> UniqueItemDisputes { get; set; }

        public void AddSkuItemDispute(Guid lineItemId, Guid reasonId, string comment = "")
        {
            SkuItemDisputes.Add(new OrderItemDisputeDto(lineItemId, reasonId, comment));
        }

        public void AddUniqueItemDispute(Guid lineItemId, Guid reasonId, string comment = "")
        {
            UniqueItemDisputes.Add(new OrderItemDisputeDto(lineItemId, reasonId, comment));
        }
    }

    public class OrderItemDisputeDto
    {
        public OrderItemDisputeDto()
        {
            
        }

        public OrderItemDisputeDto(Guid lineItemId, Guid reasonId, string comment)
        {
            LineItemId = lineItemId;
            ReasonId = reasonId;
            Comment = comment;
        }

        public Guid LineItemId { get; set; }    // unique ItemId or SkuId
        public Guid ReasonId { get; set; }
        public string Comment { get; set; }
    }
}