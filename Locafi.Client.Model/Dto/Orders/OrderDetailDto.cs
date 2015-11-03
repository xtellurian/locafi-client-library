using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Locafi.Client.Model.RFID;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace Locafi.Client.Model.Dto.Orders
{
    public class OrderDetailDto : OrderSummaryDto
    {
        public OrderDetailDto()
        {
            
            ExpectedSkus = new List<OrderSkuLineItemDto>();
            AdditionalSkus = new List<OrderSkuLineItemDto>();
            ExpectedItems = new List<OrderItemLineItemDto>();
            AdditionalItems = new List<OrderItemLineItemDto>();
            ExcludedTagNumbers = new List<string>();
            UnknownTags = new List<IRfidTag>();
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
        
        public IList<OrderSkuLineItemDto> ExpectedSkus { get; set; }
        public IList<OrderSkuLineItemDto> AdditionalSkus { get; set; }
        public IList<OrderItemLineItemDto> ExpectedItems { get; set; }
        public IList<OrderItemLineItemDto> AdditionalItems { get; set; }
        public IList<string> ExcludedTagNumbers { get; set; } 
        public string ServerMessage { get; set; }

        [JsonIgnore]
        public IList<IRfidTag> UnknownTags { get; set; }

    }
}