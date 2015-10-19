using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto;
using Locafi.Client.Model.Dto.Users;

namespace Locafi.Client.Model.Dto.Users
{
    public class UserDetailDto : UserSummaryDto
    {
        public UserDetailDto()
        {
            
        }
        public UserDetailDto(UserDetailDto dto)
        {
            var type = typeof(UserDetailDto);
            var properties = type.GetTypeInfo().DeclaredProperties;
            foreach (var property in properties)
            {
                var value = property.GetValue(dto);
                property.SetValue(this, value);
            }
        }
        public Guid? TagId { get; set; }

        public string EmailAddress { get; set; }

        public IList<ReadEntityExtendedPropertyDto> UserExtendedPropeertyList { get; set; }

    }
}
