using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Roles
{
    public class AddRoleDto
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public IList<ClaimDto> Claims { get; set; }

        public AddRoleDto()
        {
            Claims = new List<ClaimDto>();
        }
    }
}
