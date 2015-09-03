using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Orders
{
    public class OrderSummaryDto : EntityDtoBase
    {
        public string ReferenceNumber { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public Guid SourcePlaceId { get; set; }
        public Guid DestinationPlaceId { get; set; }
        public Guid? DeliverToId { get; set; }
    }
}
