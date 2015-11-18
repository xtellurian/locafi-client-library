using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.SkuGroups
{
    public class UpdateSkuGroupDto
    {
        public Guid SkuGroupId { get; set; }    // Id of the sku group to modify
        public Guid? SkuGroupNameId { get; set; }    // set if you want to change the name of the group
        public IList<Guid> AddSkuIds { get; set; }
        public IList<Guid> AddPlaceIds { get; set; }
        public IList<Guid> RemoveSkuIds { get; set; }
        public IList<Guid> RemovePlaceIds { get; set; }
    }
}
