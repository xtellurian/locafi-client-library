using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Orders
{
    public class AddOrderSnapshotResultDto
    {
        public OrderDetailDto OrderDto { get; set; }

        public IList<ItemNotificationDto> Notifications { get; set; }

        public AddOrderSnapshotResultDto()
        {
            Notifications = new List<ItemNotificationDto>();
        }
    }
}
