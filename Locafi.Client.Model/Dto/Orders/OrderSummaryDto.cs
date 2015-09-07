using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Orders
{
    public class OrderSummaryDto : EntityDtoBase
    {
        public OrderSummaryDto()
        {
            
        }

        public OrderSummaryDto(OrderSummaryDto dto) : base(dto)
        {
            var properties = this.GetType().GetTypeInfo().DeclaredProperties;
            foreach (var property in properties)
            {
                var value = property.GetValue(dto);
                property.SetValue(this, value);
            }
        }
        public string ReferenceNumber { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public Guid SourcePlaceId { get; set; }
        public Guid DestinationPlaceId { get; set; }
        public Guid? DeliverToId { get; set; }
    }
}
