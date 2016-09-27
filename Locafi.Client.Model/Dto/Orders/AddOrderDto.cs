using Locafi.Client.Model.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Orders
{
    public class AddOrderDto
    {
        public AddOrderDto()
        {
            OrderSkus = new List<AddOrderSkuDto>();
            OrderUniqueItems = new List<AddOrderUniqueItemDto>();
        }

        public AddOrderDto(OrderType orderType, string customerOrderNumber, string comments, 
            Guid? sourcePlaceId, Guid? destinationPlaceId, IList<AddOrderSkuDto> skuLineItems, 
            IList<AddOrderUniqueItemDto> itemLineItems = null, Guid? deliverToId = null, Guid? customerId = null) : base()
        {
            CustomerOrderNumber = customerOrderNumber;
            Comments = comments;
            FromPlaceId = sourcePlaceId;
            ToPlaceId = destinationPlaceId;
            OrderSkus = skuLineItems;
            OrderUniqueItems = itemLineItems;
            DeliverToPersonId = deliverToId;
            CustomerId = customerId;
            OrderType = orderType;
        }

        public string CustomerOrderNumber { get; set; }

        public string Comments { get; set; }

        public Guid? FromPlaceId { get; set; }

        public Guid? ToPlaceId { get; set; }

        public Guid? CustomerId { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public OrderType OrderType { get; set; }

        public Guid? DeliverToPersonId { get; set; }

        public IList<AddOrderSkuDto> OrderSkus { get; set; }

        public IList<AddOrderUniqueItemDto> OrderUniqueItems { get; set; }
    }
}
