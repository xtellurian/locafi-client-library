using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Roles
{
    public class RoleDetailDto : RoleSummaryDto
    {
        public RoleDetailDto()
        {
            Claims = new List<ClaimDto>();
        }

        public RoleDetailDto(RoleDetailDto dto):base(dto)
        {
            var type = typeof(RoleDetailDto);
            var properties = type.GetTypeInfo().DeclaredProperties;
            foreach (var property in properties)
            {
                var value = property.GetValue(dto);
                property.SetValue(this, value);
            }
        }

        public IList<ClaimDto> Claims { get; set; }
    }
}
