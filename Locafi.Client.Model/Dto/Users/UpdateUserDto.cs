using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Users
{
    public class UpdateUserDto
    {
        public Guid UserId { get; set; }

        public string GivenName { get; set; }

        public string Surname { get; set; }

        public string EmailAddress { get; set; }

        public IList<WriteEntityExtendedPropertyDto> UserExtendedPropertyList { get; set; }

        public UpdateUserDto()
        {
            UserExtendedPropertyList = new List<WriteEntityExtendedPropertyDto>();
        }
    }
}
