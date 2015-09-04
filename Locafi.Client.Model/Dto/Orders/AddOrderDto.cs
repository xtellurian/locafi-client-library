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
            
        }

        public AddOrderDto(string referenceNumber, string description, 
            Guid sourcePlaceId, Guid destinationPlaceId, IList<AddOrderSkuLineItemDto> skuLineItems, 
            IList<AddOrderItemLineItemDto> itemLineItems = null, Guid? deliverToId = null)
        {
            ReferenceNumber = referenceNumber;
            Description = description;
            SourcePlaceId = sourcePlaceId;
            DestinationPlaceId = destinationPlaceId;
            SkuLineItems = skuLineItems;
            ItemLineItems = itemLineItems;
            DeliverToId = deliverToId;
        }

        public string ReferenceNumber { get; set; }

        public string Description { get; set; }

        public Guid SourcePlaceId { get; set; }

        public Guid DestinationPlaceId { get; set; }

        public Guid? DeliverToId { get; set; }

        public IList<AddOrderSkuLineItemDto> SkuLineItems { get; set; }

        public IList<AddOrderItemLineItemDto> ItemLineItems { get; set; }
    }
}
