using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Users
{
    public class AddUserDto
    {
        public string GivenName { get; set; }    // Required

        public string MiddleName { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }    // Required (this is also the username)

        public string Password { get; set; }    // Required

        public string ImageUrl { get; set; }

        public Guid RoleId { get; set; }

        public AddUserDto()
        {
        }

    }
}
