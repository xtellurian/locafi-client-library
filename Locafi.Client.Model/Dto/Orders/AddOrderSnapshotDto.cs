using Locafi.Client.Model.Dto.Snapshots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Orders
{
    public class AddOrderSnapshotDto : AddSnapshotDto
    {
        public Guid OrderId { get; set; }

        public Guid PlaceId { get; set; }

        public AddOrderSnapshotDto() { }

        public AddOrderSnapshotDto(Guid orderId, Guid placeId, AddSnapshotDto dto)
        {
            OrderId = orderId;
            PlaceId = placeId;

            if (dto == null) return;

            var properties = typeof(AddSnapshotDto).GetTypeInfo().DeclaredProperties;
            foreach (var property in properties)
            {
                var value = property.GetValue(dto);
                property.SetValue(this, value);
            }
        }
    }
}
