using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Users
{
    public class UpdateUserProfileDto
    {
        public string ImageUrl { get; set; }

        public string GivenName { get; set; }

        public string Surname { get; set; }

        public IList<WriteEntityExtendedPropertyDto> PersonExtendedPropertyList { get; set; }

        public UpdateUserProfileDto(UserDetailDto detail)
        {
            ImageUrl = detail.ImageUrl;
            GivenName = detail.GivenName;
            Surname = detail.Surname;
            PersonExtendedPropertyList = new List<WriteEntityExtendedPropertyDto>(detail.PersonExtendedPropertyList);
        }
    }
}
