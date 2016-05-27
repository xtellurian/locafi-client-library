﻿using System;
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
        public string GivenName { get; set; }

        public string MiddleName { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
