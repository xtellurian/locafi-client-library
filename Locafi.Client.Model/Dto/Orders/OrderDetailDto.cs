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

            OrderSkuList = new List<ReadOrderSkuDto>();
            OrderItemList = new List<ReadOrderItemDto>();
        }

        public OrderDetailDto(OrderDetailDto dto): base(dto)
        {
            if (dto == null) return;

            var type = typeof(OrderDetailDto);
            var properties = type.GetTypeInfo().DeclaredProperties;
            foreach (var property in properties)
            {
                var value = property.GetValue(dto);
                property.SetValue(this, value);
            }
        }

        public IList<ReadOrderSkuDto> OrderSkuList { get; set; }

        public IList<ReadOrderItemDto> OrderItemList { get; set; }


    }
}