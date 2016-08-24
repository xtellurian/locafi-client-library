using Locafi.Client.Contract.Repo;
using Locafi.Client.Model.Dto.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.UnitTests.EntityGenerators
{
    public static class UserGenerator
    {
        public static async Task<AddUserDto> GenerateRandomAddUserDto()
        {
            IRoleRepo _roleRepo = WebRepoContainer.RoleRepo;

            var addDto = new AddUserDto(await PersonGenerator.GenerateRandomAddPersonDto());

            addDto.Password = Guid.NewGuid().ToString();

            var addRole = RoleGenerator.GenerateRandomAddRoleDto();
            var role = await _roleRepo.CreateRole(addRole);
            addDto.RoleId = role.Id;

            return addDto;
        }
    }
}
