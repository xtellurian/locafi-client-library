using Locafi.Client.Model.Dto.Persons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Users
{
    public class AddUserDto : AddPersonDto
    {

        public string Password { get; set; }    // Required

        public string ImageUrl { get; set; }

        public Guid RoleId { get; set; }

        public AddUserDto()
        {
        }

        public AddUserDto(AddPersonDto dto)
        {
            if (dto == null) return;

            var type = typeof(AddPersonDto);
            var properties = type.GetTypeInfo().DeclaredProperties;
            foreach (var property in properties)
            {
                var value = property.GetValue(dto);
                property.SetValue(this, value);
            }
        }
    }
}
