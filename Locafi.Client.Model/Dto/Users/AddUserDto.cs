using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Users
{
    public class AddUserDto
    {
        public AddUserDto()
        {
            
        }
        public Guid TemplateId { get; set; }    // Required

        public string GivenName { get; set; }    // Required

        public string Surname { get; set; }

        public string EmailAddress { get; set; }    // Required (this is also the username)

        public string Password { get; set; }    // Required

        public IList<WriteEntityExtendedPropertyDto> UserExtendedPropertyList { get; set; }

    }
}
