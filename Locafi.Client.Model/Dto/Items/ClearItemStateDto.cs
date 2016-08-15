using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Items
{
    public class ClearItemStateDto
    {
        public Guid PlaceId { get; set; }

        public Guid? SkuGroupId { get; set; }

        public IList<Guid> CustomSkuList { get; set; }
    }
}
