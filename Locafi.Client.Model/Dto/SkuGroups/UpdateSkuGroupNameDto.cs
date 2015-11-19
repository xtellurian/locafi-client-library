using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.SkuGroups
{
    public class UpdateSkuGroupNameDto
    {
        public UpdateSkuGroupNameDto()
        {
            
        }
        public UpdateSkuGroupNameDto(Guid id, string name)
        {
            SkuGroupNameId = id;
            Name = name;
        }
        public Guid SkuGroupNameId { get; set; }

        public string Name { get; set; }
    }
}
