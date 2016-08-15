using Locafi.Client.Model.Dto.Persons;
using System;
using System.Collections.Generic;
using System.Linq;
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

    }
}
