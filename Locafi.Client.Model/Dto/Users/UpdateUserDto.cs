using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Users
{
    public class UpdateUserDto
    {
        public Guid Id { get; set; }

        public string GivenName { get; set; }

        public string MiddleName { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

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
                MiddleName = detail.MiddleName,
                RoleId = detail.RoleId,
                Surname = detail.Surname
            };
        }
    }
}
