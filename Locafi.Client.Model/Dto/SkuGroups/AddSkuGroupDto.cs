using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.SkuGroups
{
    public class AddSkuGroupDto
    {
        public Guid SkuGroupNameId { get; set; }
        public IList<Guid> SkuIds { get; set; }
        public IList<Guid> PlaceIds { get; set; }
    }
}
