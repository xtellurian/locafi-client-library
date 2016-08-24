using Locafi.Client.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Roles
{
    public class UpdateRoleDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public IList<ClaimDto> Claims { get; set; }

        public UpdateRoleDto()
        {
            Claims = new List<ClaimDto>();
        }

        public UpdateRoleDto(RoleDetailDto dto)
        {
            Id = dto.Id;
            Name = dto.Name;
            Description = dto.Description;

            Claims = dto.Claims;
        }

        public void AddClaim(NavigatorModule module, DataAccessMode mode)
        {
            // check if claim exists
            var claim = Claims.Where(c => c.ModuleName == module && c.Permission == mode).FirstOrDefault();
            // if not add it
            if (claim == null)
                Claims.Add(new ClaimDto() { ModuleName = module, Permission = mode });
        }

        public void RemoveClaim(NavigatorModule module, DataAccessMode mode)
        {
            // check if claim exists
            var claim = Claims.Where(c => c.ModuleName == module && c.Permission == mode).FirstOrDefault();
            // if it does, remove it
            if (claim != null)
                Claims.Remove(claim);
        }
    }
}
