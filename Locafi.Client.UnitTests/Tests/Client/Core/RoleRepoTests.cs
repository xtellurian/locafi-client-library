using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Model.Dto.Persons;
using Locafi.Client.Model.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Locafi.Client.Model.Dto.Tags;
using Locafi.Client.Exceptions;
using Locafi.Client.UnitTests.Validators;
using Locafi.Client.Model;
using Locafi.Client.Model.Query.Builder;
using Locafi.Client.Model.Dto.Places;
using Locafi.Client.Model.Query;
using Locafi.Client.UnitTests.EntityGenerators;
using Locafi.Client.Model.Dto.Roles;
using Locafi.Client.UnitTests.Extensions;

namespace Locafi.Client.UnitTests.Tests
{
    [TestClass]
    public class RoleRepoTests
    {
        private IRoleRepo _roleRepo;
        private ITemplateRepo _templateRepo;
        private IList<Guid> _rolesToCleanup;

        [TestInitialize]
        public void Initialise()
        {
            _templateRepo = WebRepoContainer.TemplateRepo;
            _roleRepo = WebRepoContainer.RoleRepo;
            _rolesToCleanup = new List<Guid>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            // delete all that were created
            foreach (var Id in _rolesToCleanup)
            {
                _roleRepo.DeleteRole(Id).Wait();
            }
        }

        [TestMethod]
        public async Task Role_Create()
        {
            // create role
            var addDto = RoleGenerator.GenerateRandomAddRoleDto();
            var detail = await _roleRepo.CreateRole(addDto);
            _rolesToCleanup.AddUnique(detail.Id);

            // check result
            RoleDtoValidator.RoleDetailCheck(detail);
            
            Validator.IsTrue(string.Equals(addDto.Name, detail.Name));
            Validator.IsTrue(string.Equals(addDto.Description, detail.Description));
            Validator.IsTrue(addDto.Claims.Count == detail.Claims.Count);
        }

        [TestMethod]
        public async Task Role_Get()
        {
            // create role
            var addDto = RoleGenerator.GenerateRandomAddRoleDto();
            var detail = await _roleRepo.CreateRole(addDto);
            _rolesToCleanup.AddUnique(detail.Id);

            // check result
            RoleDtoValidator.RoleDetailCheck(detail);

            Validator.IsTrue(string.Equals(addDto.Name, detail.Name));
            Validator.IsTrue(string.Equals(addDto.Description, detail.Description));
            Validator.IsTrue(addDto.Claims.Count == detail.Claims.Count);

            // get the role
            detail = await _roleRepo.GetRoleById(detail.Id);

            // check result
            RoleDtoValidator.RoleDetailCheck(detail);

            Validator.IsTrue(string.Equals(addDto.Name, detail.Name));
            Validator.IsTrue(string.Equals(addDto.Description, detail.Description));
            Validator.IsTrue(addDto.Claims.Count == detail.Claims.Count);
        }

        [TestMethod]
        public async Task Role_GetAll()
        {
            // create role
            var addDto = RoleGenerator.GenerateRandomAddRoleDto();
            var detail = await _roleRepo.CreateRole(addDto);
            _rolesToCleanup.AddUnique(detail.Id);

            // check result
            RoleDtoValidator.RoleDetailCheck(detail);

            Validator.IsTrue(string.Equals(addDto.Name, detail.Name));
            Validator.IsTrue(string.Equals(addDto.Description, detail.Description));
            Validator.IsTrue(addDto.Claims.Count == detail.Claims.Count);

            // query roles
            var roles = await _roleRepo.QueryRoles();

            // check result
            Validator.IsNotNull(roles, "roles != null");
            Validator.IsInstanceOfType(roles, typeof(PageResult<RoleSummaryDto>));
            Validator.IsTrue(roles.Items.Count() > 0);
            Validator.IsTrue(roles.Contains(detail));
        }

        [TestMethod]
        public async Task Role_Update()
        {
            // create role
            var addDto = RoleGenerator.GenerateRandomAddRoleDto();
            var detail = await _roleRepo.CreateRole(addDto);
            _rolesToCleanup.AddUnique(detail.Id);

            // check result
            RoleDtoValidator.RoleDetailCheck(detail);

            Validator.IsTrue(string.Equals(addDto.Name, detail.Name));
            Validator.IsTrue(string.Equals(addDto.Description, detail.Description));
            Validator.IsTrue(addDto.Claims.Count == detail.Claims.Count);

            // update the person
            var updateDto = new UpdateRoleDto(detail);
            updateDto.Name += " - Update";
            updateDto.Description += " - Update";
            updateDto.RemoveClaim(updateDto.Claims[0].ModuleName.Value, updateDto.Claims[0].Permission.Value);
            var updateDetail = await _roleRepo.UpdateRole(updateDto);

            // check the response
            RoleDtoValidator.RoleDetailCheck(updateDetail);
            Validator.IsFalse(string.Equals(addDto.Name, updateDetail.Name));
            Validator.IsFalse(string.Equals(addDto.Description, updateDetail.Description));
            Validator.IsFalse(addDto.Claims.Count == updateDetail.Claims.Count);
            Validator.IsTrue(string.Equals(updateDto.Name, updateDetail.Name));
            Validator.IsTrue(string.Equals(updateDto.Description, updateDetail.Description));
            Validator.IsTrue(updateDto.Claims.Count == updateDetail.Claims.Count);
        }

        [TestMethod]
        public async Task Role_Delete()
        {
            // create role
            var addDto = RoleGenerator.GenerateRandomAddRoleDto();
            var detail = await _roleRepo.CreateRole(addDto);
            _rolesToCleanup.AddUnique(detail.Id);

            // check result
            RoleDtoValidator.RoleDetailCheck(detail);

            Validator.IsTrue(string.Equals(addDto.Name, detail.Name));
            Validator.IsTrue(string.Equals(addDto.Description, detail.Description));
            Validator.IsTrue(addDto.Claims.Count == detail.Claims.Count);

            // delete the role
            var deleteResult = await _roleRepo.DeleteRole(detail.Id);
            Validator.IsTrue(deleteResult);
            // remove from delete list
            _rolesToCleanup.Remove(detail.Id);

            // verify
            var query = QueryBuilder<RoleSummaryDto>.NewQuery(p => p.Id, detail.Id, ComparisonOperator.Equals).Build();
            var queryResult = await _roleRepo.QueryRoles(query); // get the role again
            Validator.IsFalse(queryResult.Any(p => p.Id == detail.Id)); // check our role is actually gone

            // verify with get
            try
            {
                var sameItem = await _roleRepo.GetRoleById(detail.Id);

                Validator.IsTrue(false, "Deleted entity returned");
            }
            catch (Exception e)
            {
                // this is expected                
            }
        }
    }
}
