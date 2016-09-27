using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.CycleCountDtos
{
    public class ResolveCycleCountResultDto
    {
        public CycleCountDetailDto CycleCountDto { get; set; }

        public IList<ItemNotificationDto> Notifications { get; set; }

    }
}
