using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Roles
{
    public class UpdateRoleDto : RoleSummaryDto
    {
        public IList<ClaimDto> Claims { get; set; }

        public UpdateRoleDto()
        {
            Claims = new List<ClaimDto>();
        }
    }
}
