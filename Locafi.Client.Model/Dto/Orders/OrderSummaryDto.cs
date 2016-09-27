using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Locafi.Client.Model.Dto.Orders
{
    public class OrderSummaryDto : EntityDtoBase
    {
        public OrderSummaryDto()
        {
            
        }

        public OrderSummaryDto(OrderSummaryDto dto) : base(dto)
        {
            if (dto == null) return;

            var properties = typeof(OrderSummaryDto).GetTypeInfo().DeclaredProperties;
            foreach (var property in properties)
            {
                var value = property.GetValue(dto);
                property.SetValue(this, value);
            }
        }

        public string CustomerOrderNumber { get; set; }

        public string Comments { get; set; }

        public Guid? ToPlaceId { get; set; }

        public string ToPlaceName { get; set; }

        public Guid? FromPlaceId { get; set; }

        public string FromPlaceName { get; set; }

        public Guid? CustomerId { get; set; }

        public string CustomerName { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public OrderType OrderType { get; set; }

        public Guid? DeliverToPersonId { get; set; }

        public string DeliverToPersonName { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public OrderStateType OrderState { get; set; }
    }
}
