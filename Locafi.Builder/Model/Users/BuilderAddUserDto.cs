using Locafi.Builder.Model.Persons;
using Locafi.Client.Model.Dto.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Builder.Model.Users
{
    public class BuilderAddUserDto : BuilderAddPersonDto
    {
        public string RoleName { get; set; }

        public string Password { get; set; }    // Required

        public string ImageUrl { get; set; }

        public BuilderAddUserDto()
        {
            BuilderPersonExtendedPropertyList = new List<BuilderWriteEntityExtendedPropertyDto>();
        }
    }
}
