using Locafi.Client.Model.Dto.Persons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Users
{
    public class SpawnUserDto
    {
        public Guid PersonId { get; set; }

        public string Password { get; set; }

        public Guid RoleId { get; set; }

        public SpawnUserDto(PersonDetailDto detail)
        {
            PersonId = detail.Id;
        }
    }
}
