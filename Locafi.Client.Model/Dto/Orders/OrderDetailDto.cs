using System;
using System.Collections.Generic;
using System.Reflection;

namespace Locafi.Client.Model.Dto.Orders
{
    public class OrderDetailDto : OrderSummaryDto
    {
        public OrderDetailDto()
        {
            
        }

        public OrderDetailDto(OrderDetailDto dto): base(dto)
        {
            var type = typeof(OrderDetailDto);
            var properties = type.GetTypeInfo().DeclaredProperties;
            foreach (var property in properties)
            {
                var value = property.GetValue(dto);
                property.SetValue(this, value);
            }
        }
        public IList<Guid> SourceSnapshotIds { get; set; }
        public IList<Guid> DestinationSnapshotIds { get; set; }
        public IList<OrderSkuLineItemDto> RequiredSkus { get; set; }
        public IList<OrderItemLineItemDto> RequiredItems { get; set; }
        public string ServerMessage { get; set; }

    }
}