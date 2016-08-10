using Locafi.Client.Model.Dto.Persons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Users
{
    public class UpdateUserDto : UpdatePersonDto
    {
        public string ImageUrl { get; set; }

        public Guid RoleId { get; set; }

        public UpdateUserDto()
        {
        }

        public static UpdateUserDto FromUserDetail(UserDetailDto detail)
        {
            return new UpdateUserDto
            {
                Email = detail.Email,
                GivenName = detail.GivenName,
                Id = detail.Id,
                ImageUrl = detail.ImageUrl,
                RoleId = detail.RoleId,
                Surname = detail.Surname
            };
        }
    }
}
