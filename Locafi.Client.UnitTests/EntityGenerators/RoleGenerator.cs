using Locafi.Client.Model.Dto.Roles;
using Locafi.Client.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.UnitTests.EntityGenerators
{
    public static class RoleGenerator
    {
        public static AddRoleDto GenerateRandomAddRoleDto()
        {
            var ran = new Random(DateTime.UtcNow.Millisecond);
            var addDto = new AddRoleDto();

            addDto.Name = Guid.NewGuid().ToString();
            addDto.Description = addDto.Name + " - Description";

            Array modules = Enum.GetValues(typeof(NavigatorModule));
            Array accessModes = Enum.GetValues(typeof(DataAccessMode));

            // add a claim for each module
            foreach(var module in modules)
            {
                addDto.Claims.Add(new ClaimDto() { ModuleName = (NavigatorModule)module, Permission = (DataAccessMode)accessModes.GetValue(ran.Next(accessModes.Length)) });
            }

            return addDto;
        }
    }
}
