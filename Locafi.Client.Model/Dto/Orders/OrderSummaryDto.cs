using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Orders
{
    public class OrderSummaryDto : EntityDtoBase
    {
        public OrderSummaryDto()
        {
            
        }

        public OrderSummaryDto(OrderSummaryDto orderSummaryDto) : base(orderSummaryDto)
        {
            ReferenceNumber = orderSummaryDto.ReferenceNumber;
            Status = orderSummaryDto.Status;
            Description = orderSummaryDto.Description;
            SourcePlaceId = orderSummaryDto.SourcePlaceId;
            DestinationPlaceId = orderSummaryDto.DestinationPlaceId;
            DeliverToId = orderSummaryDto.DeliverToId;
        }
        public string ReferenceNumber { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public Guid SourcePlaceId { get; set; }
        public Guid DestinationPlaceId { get; set; }
        public Guid? DeliverToId { get; set; }
    }
}
