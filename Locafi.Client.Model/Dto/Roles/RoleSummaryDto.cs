using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Roles
{
    public class RoleSummaryDto : EntityDtoBase
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public RoleSummaryDto(RoleSummaryDto dto):base(dto)
        {
            if (dto == null) return;

            var type = typeof(RoleSummaryDto);
            var properties = type.GetTypeInfo().DeclaredProperties;
            foreach (var property in properties)
            {
                var value = property.GetValue(dto);
                property.SetValue(this, value);
            }
        }

        public RoleSummaryDto()
        {
            
        }
    }
}
