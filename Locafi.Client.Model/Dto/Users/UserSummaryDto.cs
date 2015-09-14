using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Users
{
    public class UserSummaryDto : EntityDtoBase
    {
        public UserSummaryDto()
        {
            
        }

        public UserSummaryDto(UserSummaryDto dto) : base(dto)
        {
            var type = typeof(UserSummaryDto);
            var properties = type.GetTypeInfo().DeclaredProperties;
            foreach (var property in properties)
            {
                var value = property.GetValue(dto);
                property.SetValue(this, value);
            }
        }
        public Guid TemplateId { get; set; }

        public string TemplateName { get; set; }

        public string GivenName { get; set; }

        public string Surname { get; set; }

        public string UserName { get; set; }
    }
}
