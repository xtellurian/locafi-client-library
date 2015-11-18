using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.SkuGroups
{
    public class UpdateSkuGroupNameDto
    {
        public Guid SkuGroupNameId { get; set; }

        public string Name { get; set; }
    }
}
