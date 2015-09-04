using System;
using System.Collections.Generic;

namespace Locafi.Client.Model.Dto.Orders
{
    public class OrderDetailDto : OrderSummaryDto
    {
        public IList<Guid> SourceSnapshotIds { get; set; }
        public IList<Guid> DestinationSnapshotIds { get; set; }
        public IList<OrderSkuLineItemDto> RequiredSkus { get; set; }
        public IList<OrderItemLineItemDto> RequiredItems { get; set; }
        public string ServerMessage { get; set; }

    }
}