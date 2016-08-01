using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.CycleCountDtos
{
    public class AddCycleCountDto
    {
        public Guid PlaceId { get; set; }

        public Guid? SkuGroupId { get; set; }

        public List<Guid> SkuIds { get; set; }

        public AddCycleCountDto()
        {
            SkuIds = new List<Guid>();
        }
    }
}
