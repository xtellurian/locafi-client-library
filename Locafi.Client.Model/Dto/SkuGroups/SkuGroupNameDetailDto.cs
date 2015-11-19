using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.SkuGroups
{
    public class SkuGroupNameDetailDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            var groupName = obj as SkuGroupNameDetailDto;
            if(groupName==null) return false;
            return this.Id == groupName.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
