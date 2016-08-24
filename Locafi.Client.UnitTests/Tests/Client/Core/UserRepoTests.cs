using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Model.Dto.Users;
using Locafi.Client.Model.Query;
using Locafi.Client.Model.Query.PropertyComparison;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Locafi.Client.UnitTests.EntityGenerators;
using Locafi.Client.UnitTests.Validators;
using Locafi.Client.Model;
using Locafi.Client.UnitTests.Extensions;
using Locafi.Client.Exceptions;
using Locafi.Client.Model.Dto.Persons;
using Locafi.Client.Model.Query.Builder;
using Locafi.Client.Model.Dto;
using Locafi.Builder;

namespace Locafi.Client.UnitTests.Tests
{
    [TestClass]
    public class UserRepoTests
    {
        private IUserRepo _userRepo;
        private IRoleRepo _roleRepo;
        private IPersonRepo _personRepo;
        private IAuthenticationRepo _authRepo;
        private List<Guid> _usersToDelete;
        private List<Guid> _rolesToDelete;

        [TestInitialize]
        public void Initialize()
        {
            _userRepo = WebRepoContainer.UserRepo;
            _roleRepo = WebRepoContainer.RoleRepo;
            _personRepo = WebRepoContainer.PersonRepo;
            _authRepo = WebRepoContainer.AuthRepo;
            _usersToDelete = new List<Guid>();
            _rolesToDelete = new List<Guid>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            // delete all skus that were created
            foreach (var Id in _usersToDelete)
            {
                _userRepo.DeleteUserAndPerson(Id).Wait();
            }

            foreach(var id in _rolesToDelete)
            {
                _roleRepo.DeleteRole(id).Wait();
            }
        }

        [TestMethod]
        public async Task User_Create()
        {
            // create user
            var addUseDto = await UserGenerator.GenerateRandomAddUserDto();
            _rolesToDelete.AddUnique(addUseDto.RoleId);
            var user = await _userRepo.CreateUser(addUseDto);
            _usersToDelete.AddUnique(user.Id);

            // check response
            UserDtoValidator.UserDetailCheck(user);
            Validator.IsTrue(string.Equals(addUseDto.Email, user.Email));
            Validator.IsTrue(string.Equals(addUseDto.GivenName, user.GivenName));
            Validator.IsTrue(string.Equals(addUseDto.Surname, user.Surname));
            Validator.IsTrue(addUseDto.TemplateId == user.TemplateId);
            Validator.IsTrue(addUseDto.RoleId == user.RoleId);
            Validator.IsTrue(string.Equals(addUseDto.PersonTagList[0].TagNumber.ToUpper(), user.TagNumber));
            // check we can get the associated person
            var userPerson = await _personRepo.GetPersonById(user.PersonId);
            Validator.IsTrue(string.Equals(user.Email, userPerson.Email));
            Validator.IsTrue(string.Equals(user.GivenName, userPerson.GivenName));
            Validator.IsTrue(string.Equals(user.Surname, userPerson.Surname));
            Validator.IsTrue(user.TemplateId == userPerson.TemplateId);
            Validator.IsTrue(string.Equals(user.PersonTagList[0].TagNumber.ToUpper(), userPerson.TagNumber));
        }

        [TestMethod]
        public async Task User_Get()
        {
            // create user
            var addUseDto = await UserGenerator.GenerateRandomAddUserDto();
            _rolesToDelete.AddUnique(addUseDto.RoleId);
            var user = await _userRepo.CreateUser(addUseDto);
            _usersToDelete.AddUnique(user.Id);

            // check response
            UserDtoValidator.UserDetailCheck(user);
            Validator.IsTrue(string.Equals(addUseDto.Email, user.Email));
            Validator.IsTrue(string.Equals(addUseDto.GivenName, user.GivenName));
            Validator.IsTrue(string.Equals(addUseDto.Surname, user.Surname));
            Validator.IsTrue(string.Equals(addUseDto.TemplateId, user.TemplateId));
            Validator.IsTrue(string.Equals(addUseDto.RoleId, user.RoleId));
            Validator.IsTrue(string.Equals(addUseDto.PersonTagList[0].TagNumber.ToUpper(), user.TagNumber));
            // check we can get the associated person
            var userPerson = await _personRepo.GetPersonById(user.PersonId);
            Validator.IsTrue(string.Equals(user.Email, userPerson.Email));
            Validator.IsTrue(string.Equals(user.GivenName, userPerson.GivenName));
            Validator.IsTrue(string.Equals(user.Surname, userPerson.Surname));
            Validator.IsTrue(string.Equals(user.TemplateId, userPerson.TemplateId));
            Validator.IsTrue(string.Equals(user.PersonTagList[0].TagNumber.ToUpper(), userPerson.TagNumber));

            // try and get the user back
            var userGet = await _userRepo.GetUserById(user.Id);

            // check the response
            UserDtoValidator.UserDetailCheck(userGet);
            Validator.IsTrue(string.Equals(userGet.Email, user.Email));
            Validator.IsTrue(string.Equals(userGet.GivenName, user.GivenName));
            Validator.IsTrue(string.Equals(userGet.Surname, user.Surname));
            Validator.IsTrue(string.Equals(userGet.TemplateId, user.TemplateId));
            Validator.IsTrue(string.Equals(userGet.RoleId, user.RoleId));
            Validator.IsTrue(string.Equals(userGet.PersonTagList[0].TagNumber.ToUpper(), user.TagNumber));
        }

        [TestMethod]
        public async Task User_GetAllUsers()
        {
            // create user
            var addUseDto = await UserGenerator.GenerateRandomAddUserDto();
            _rolesToDelete.AddUnique(addUseDto.RoleId);
            var user = await _userRepo.CreateUser(addUseDto);
            _usersToDelete.AddUnique(user.Id);

            // check response
            UserDtoValidator.UserDetailCheck(user);
            Validator.IsTrue(string.Equals(addUseDto.Email, user.Email));
            Validator.IsTrue(string.Equals(addUseDto.GivenName, user.GivenName));
            Validator.IsTrue(string.Equals(addUseDto.Surname, user.Surname));
            Validator.IsTrue(string.Equals(addUseDto.TemplateId, user.TemplateId));
            Validator.IsTrue(string.Equals(addUseDto.RoleId, user.RoleId));
            Validator.IsTrue(string.Equals(addUseDto.PersonTagList[0].TagNumber.ToUpper(), user.TagNumber));

            // now query for all users
            var users = await _userRepo.QueryUsers();
            Validator.IsNotNull(users);
            Validator.IsInstanceOfType(users,typeof(PageResult<UserSummaryDto>));
            Validator.IsTrue(users.Count > 0);
            Validator.IsTrue(users.Contains(user));
        }

       [TestMethod]
        public async Task User_QueryUsers()
        {
            // create user
            var addUseDto = await UserGenerator.GenerateRandomAddUserDto();
            _rolesToDelete.AddUnique(addUseDto.RoleId);
            var user = await _userRepo.CreateUser(addUseDto);
            _usersToDelete.AddUnique(user.Id);

            // check response
            UserDtoValidator.UserDetailCheck(user);
            Validator.IsTrue(string.Equals(addUseDto.Email, user.Email));
            Validator.IsTrue(string.Equals(addUseDto.GivenName, user.GivenName));
            Validator.IsTrue(string.Equals(addUseDto.Surname, user.Surname));
            Validator.IsTrue(string.Equals(addUseDto.TemplateId, user.TemplateId));
            Validator.IsTrue(string.Equals(addUseDto.RoleId, user.RoleId));
            Validator.IsTrue(string.Equals(addUseDto.PersonTagList[0].TagNumber.ToUpper(), user.TagNumber));

            // query email
            var q = new UserQuery();
            q.CreateQuery(u=>u.Email,user.Email,ComparisonOperator.Equals);
            var result = await _userRepo.QueryUsers(q);
            Validator.IsNotNull(result);
            Validator.IsTrue(result.Items.Contains(user));
        }

        [TestMethod]
        public async Task User_Update()
        {
            // create user
            var addUseDto = await UserGenerator.GenerateRandomAddUserDto();
            _rolesToDelete.AddUnique(addUseDto.RoleId);
            var user = await _userRepo.CreateUser(addUseDto);
            _usersToDelete.AddUnique(user.Id);

            // check response
            UserDtoValidator.UserDetailCheck(user);
            Validator.IsTrue(string.Equals(addUseDto.Email, user.Email));
            Validator.IsTrue(string.Equals(addUseDto.GivenName, user.GivenName));
            Validator.IsTrue(string.Equals(addUseDto.Surname, user.Surname));
            Validator.IsTrue(addUseDto.TemplateId == user.TemplateId);
            Validator.IsTrue(addUseDto.RoleId == user.RoleId);
            Validator.IsTrue(string.Equals(addUseDto.PersonTagList[0].TagNumber.ToUpper(), user.TagNumber));
            // check we can get the associated person
            var userPerson = await _personRepo.GetPersonById(user.PersonId);
            Validator.IsTrue(string.Equals(user.Email, userPerson.Email));
            Validator.IsTrue(string.Equals(user.GivenName, userPerson.GivenName));
            Validator.IsTrue(string.Equals(user.Surname, userPerson.Surname));
            Validator.IsTrue(user.TemplateId == userPerson.TemplateId);
            Validator.IsTrue(string.Equals(user.PersonTagList[0].TagNumber.ToUpper(), userPerson.TagNumber));

            // update the user
            var updateDto = new UpdateUserDto(user);
            updateDto.GivenName += " - Update";
            updateDto.Surname += " - Update";
            // updateDto.Email += " - Update"; dont' change email and this should be allowed
            updateDto.ImageUrl += " - Update";
            var updateDetail = await _userRepo.UpdateUser(updateDto);

            // check the response
            UserDtoValidator.UserDetailCheck(updateDetail);
            Validator.IsTrue(string.Equals(updateDetail.Email, user.Email));
            Validator.IsFalse(string.Equals(updateDetail.GivenName, user.GivenName));
            Validator.IsFalse(string.Equals(updateDetail.Surname, user.Surname));
            Validator.IsTrue(updateDetail.TemplateId == user.TemplateId);
            Validator.IsTrue(updateDetail.RoleId == user.RoleId);
            Validator.IsTrue(string.Equals(updateDetail.PersonTagList[0].TagNumber.ToUpper(), user.TagNumber));

            Validator.IsTrue(string.Equals(updateDetail.GivenName, updateDto.GivenName));
            Validator.IsTrue(string.Equals(updateDetail.Surname, updateDto.Surname));

            // now try and update email (should get error)
            updateDto.Email = $"{Guid.NewGuid().ToString().Substring(0, 16)}@FakeDomain.com";
            try {
                updateDetail = await _userRepo.UpdateUser(updateDto);
                Validator.IsTrue(false, "We updated a users email");
            }catch(UserRepoException e)
            {
                // this is expected
            }

            // now try and update the email for the person for this user (should get error)
            var updatePersonDto = new UpdatePersonDto(updateDetail);
            updatePersonDto.Email = $"{Guid.NewGuid().ToString().Substring(0, 16)}@FakeDomain.com";
            try
            {
                var personDetail = await _personRepo.UpdatePerson(updatePersonDto);
                Validator.IsTrue(false, "We updated a person with user's email");
            }
            catch (PersonException e)
            {
                // this is expected
            }

            // change the users role
            updateDto.Email = user.Email; // set email address back
            var newRole = await _roleRepo.CreateRole(RoleGenerator.GenerateRandomAddRoleDto());
            _rolesToDelete.AddUnique(newRole.Id);
            updateDto.RoleId = newRole.Id;
            updateDetail = await _userRepo.UpdateUser(updateDto);

            // check the response
            UserDtoValidator.UserDetailCheck(updateDetail);
            Validator.IsTrue(updateDetail.RoleId == updateDto.RoleId);

            // now update the users tags by updating the persons tags
            var updatePersonTagDto = new UpdatePersonTagDto(updateDetail);
            updatePersonTagDto.PersonTagList[0].TagNumber = Guid.NewGuid().ToString();
            var updatedPerson = await _personRepo.UpdatePersonTag(updatePersonTagDto);

            // check the response
            PersonDtoValidator.PersonDetailCheck(updatedPerson);
            Validator.IsTrue(string.Equals(updatedPerson.PersonTagList[0].TagNumber.ToUpper(), updatePersonTagDto.PersonTagList[0].TagNumber.ToUpper()));
            Validator.IsTrue(!string.Equals(updatedPerson.PersonTagList[0].TagNumber.ToUpper(), updateDetail.PersonTagList[0].TagNumber.ToUpper()));

            // try and get the user back and check its tag is updated
            var userGet = await _userRepo.GetUserById(user.Id);

            // check the response
            UserDtoValidator.UserDetailCheck(userGet);
            Validator.IsTrue(string.Equals(userGet.Email, updateDetail.Email));
            Validator.IsTrue(string.Equals(userGet.GivenName, updateDetail.GivenName));
            Validator.IsTrue(string.Equals(userGet.Surname, updateDetail.Surname));
            Validator.IsTrue(userGet.TemplateId == updateDetail.TemplateId);
            Validator.IsTrue(userGet.RoleId == updateDetail.RoleId);
            Validator.IsTrue(string.Equals(userGet.PersonTagList[0].TagNumber.ToUpper(), updatedPerson.TagNumber));
            Validator.IsTrue(string.Equals(userGet.PersonTagList[0].TagNumber.ToUpper(), updatedPerson.PersonTagList[0].TagNumber.ToUpper()));
        }

        // Update tags for a user by updating the persons tags
        [TestMethod]
        public async Task User_UpdateTag()
        {
            // create user
            var addUseDto = await UserGenerator.GenerateRandomAddUserDto();
            _rolesToDelete.AddUnique(addUseDto.RoleId);
            var user = await _userRepo.CreateUser(addUseDto);
            _usersToDelete.AddUnique(user.Id);

            // check response
            UserDtoValidator.UserDetailCheck(user);
            Validator.IsTrue(string.Equals(addUseDto.Email, user.Email));
            Validator.IsTrue(string.Equals(addUseDto.GivenName, user.GivenName));
            Validator.IsTrue(string.Equals(addUseDto.Surname, user.Surname));
            Validator.IsTrue(addUseDto.TemplateId == user.TemplateId);
            Validator.IsTrue(addUseDto.RoleId == user.RoleId);
            Validator.IsTrue(string.Equals(addUseDto.PersonTagList[0].TagNumber.ToUpper(), user.TagNumber));
            // check we can get the associated person
            var userPerson = await _personRepo.GetPersonById(user.PersonId);
            Validator.IsTrue(string.Equals(user.Email, userPerson.Email));
            Validator.IsTrue(string.Equals(user.GivenName, userPerson.GivenName));
            Validator.IsTrue(string.Equals(user.Surname, userPerson.Surname));
            Validator.IsTrue(user.TemplateId == userPerson.TemplateId);
            Validator.IsTrue(string.Equals(user.PersonTagList[0].TagNumber.ToUpper(), userPerson.TagNumber));

            // now update the users tags by updating the persons tags
            var updatePersonTagDto = new UpdatePersonTagDto(user);
            updatePersonTagDto.PersonTagList[0].TagNumber = Guid.NewGuid().ToString();
            var updatedPerson = await _personRepo.UpdatePersonTag(updatePersonTagDto);

            // check the response
            PersonDtoValidator.PersonDetailCheck(updatedPerson);
            Validator.IsTrue(string.Equals(updatedPerson.PersonTagList[0].TagNumber.ToUpper(), updatePersonTagDto.PersonTagList[0].TagNumber.ToUpper()));
            Validator.IsTrue(!string.Equals(updatedPerson.PersonTagList[0].TagNumber.ToUpper(), user.PersonTagList[0].TagNumber.ToUpper()));

            // try and get the user back and check its tag is updated
            var userGet = await _userRepo.GetUserById(user.Id);

            // check the response
            UserDtoValidator.UserDetailCheck(userGet);
            Validator.IsTrue(string.Equals(userGet.Email, user.Email));
            Validator.IsTrue(string.Equals(userGet.GivenName, user.GivenName));
            Validator.IsTrue(string.Equals(userGet.Surname, user.Surname));
            Validator.IsTrue(userGet.TemplateId == user.TemplateId);
            Validator.IsTrue(userGet.RoleId == user.RoleId);
            Validator.IsTrue(string.Equals(userGet.PersonTagList[0].TagNumber.ToUpper(), updatedPerson.TagNumber));
            Validator.IsTrue(string.Equals(userGet.PersonTagList[0].TagNumber.ToUpper(), updatedPerson.PersonTagList[0].TagNumber.ToUpper()));
        }

        [TestMethod]
        public async Task User_UpdatePassword()
        {
            // create user
            var addUseDto = await UserGenerator.GenerateRandomAddUserDto();
            _rolesToDelete.AddUnique(addUseDto.RoleId);
            var user = await _userRepo.CreateUser(addUseDto);
            _usersToDelete.AddUnique(user.Id);

            // check response
            UserDtoValidator.UserDetailCheck(user);
            Validator.IsTrue(string.Equals(addUseDto.Email, user.Email));
            Validator.IsTrue(string.Equals(addUseDto.GivenName, user.GivenName));
            Validator.IsTrue(string.Equals(addUseDto.Surname, user.Surname));
            Validator.IsTrue(string.Equals(addUseDto.TemplateId, user.TemplateId));
            Validator.IsTrue(string.Equals(addUseDto.RoleId, user.RoleId));
            Validator.IsTrue(string.Equals(addUseDto.PersonTagList[0].TagNumber.ToUpper(), user.TagNumber));
            // check we can get the associated person
            var userPerson = await _personRepo.GetPersonById(user.PersonId);
            Validator.IsTrue(string.Equals(user.Email, userPerson.Email));
            Validator.IsTrue(string.Equals(user.GivenName, userPerson.GivenName));
            Validator.IsTrue(string.Equals(user.Surname, userPerson.Surname));
            Validator.IsTrue(string.Equals(user.TemplateId, userPerson.TemplateId));
            Validator.IsTrue(string.Equals(user.PersonTagList[0].TagNumber.ToUpper(), userPerson.TagNumber));

            // update the password (first try providing the wrong current password, so the action should be rejected)
            var updatePasswordDto = new UpdateUserPasswordDto();
            updatePasswordDto.Id = user.Id;
            updatePasswordDto.CurrentPassword = "wrong";
            updatePasswordDto.NewPassword = addUseDto.Password + "update";
            try {
                var updateDetail1 = await _userRepo.UpdatePassword(updatePasswordDto);
                Validator.IsTrue(false, "we still updated password");
            }
            catch (UserRepoException e)
            {
                // this is expected and ok
            }

            // now try with the correct current password
            updatePasswordDto.CurrentPassword = addUseDto.Password;
            var updateDetail = await _userRepo.UpdatePassword(updatePasswordDto);

            // check response
            UserDtoValidator.UserDetailCheck(updateDetail);
            Validator.IsTrue(string.Equals(addUseDto.Email, updateDetail.Email));
            Validator.IsTrue(string.Equals(addUseDto.GivenName, updateDetail.GivenName));
            Validator.IsTrue(string.Equals(addUseDto.Surname, updateDetail.Surname));
            Validator.IsTrue(string.Equals(addUseDto.TemplateId, updateDetail.TemplateId));
            Validator.IsTrue(string.Equals(addUseDto.RoleId, updateDetail.RoleId));
            Validator.IsTrue(string.Equals(addUseDto.PersonTagList[0].TagNumber.ToUpper(), updateDetail.TagNumber));

            // now try and login with new password
            var result = await _authRepo.Login(updateDetail.Email, updatePasswordDto.NewPassword);

            AuthenticationDtoValidator.AuthenticationResponseCheck(result, true);
        }

        [TestMethod]
        public async Task User_GetLoggedInUser()
        {
            var userDetail = await _userRepo.GetLoggedInUser();

            // check response
            UserDtoValidator.LoggedInUserDetailCheck(userDetail);
            Validator.IsTrue(string.Equals(DevEnvironment.TestUserEmail, userDetail.Email));
        }

        [TestMethod]
        public async Task User_UpdateProfile()
        {
            var userDetail = await _userRepo.GetLoggedInUser();

            // check response
            UserDtoValidator.LoggedInUserDetailCheck(userDetail);

            // now update the users profile
            var updateDto = new UpdateUserProfileDto(userDetail);
            updateDto.ImageUrl += "Update";
            updateDto.GivenName += "Update";
            updateDto.Surname += "Update";
            var updateDetail = await _userRepo.UpdateProfile(updateDto);

            // check response
            UserDtoValidator.LoggedInUserDetailCheck(updateDetail);
            Validator.IsTrue(!string.Equals(userDetail.ImageUrl, updateDetail.ImageUrl));
            Validator.IsTrue(!string.Equals(userDetail.GivenName, updateDetail.GivenName));
            Validator.IsTrue(!string.Equals(userDetail.Surname, updateDetail.Surname));
            Validator.IsTrue(string.Equals(updateDetail.ImageUrl, updateDto.ImageUrl));
            Validator.IsTrue(string.Equals(updateDetail.GivenName, updateDto.GivenName));
            Validator.IsTrue(string.Equals(updateDetail.Surname, updateDto.Surname));

            // now return to original
            var updateDtoOriginal = new UpdateUserProfileDto(userDetail);
            var updateDetailOrig = await _userRepo.UpdateProfile(updateDtoOriginal);
        }

        [TestMethod]
        public async Task User_SpawnUser()
        {
            // create a person
            var addPersonDto = await PersonGenerator.GenerateRandomAddPersonDto();
            var person = await _personRepo.CreatePerson(addPersonDto);

            // check response
            PersonDtoValidator.PersonDetailCheck(person);

            // spawn a user from the person
            var role = await _roleRepo.CreateRole(RoleGenerator.GenerateRandomAddRoleDto());
            _rolesToDelete.AddUnique(role.Id);
            var spawnDto = new SpawnUserDto(person);
            spawnDto.RoleId = role.Id;
            spawnDto.Password = Guid.NewGuid().ToString();
            var user = await _userRepo.SpawnUser(spawnDto);
            _usersToDelete.AddUnique(user.Id);

            // check the response
            UserDtoValidator.UserDetailCheck(user);
            Validator.IsTrue(user.RoleId == spawnDto.RoleId);

            // now try and login with new password
            var result = await _authRepo.Login(person.Email, spawnDto.Password);

            AuthenticationDtoValidator.AuthenticationResponseCheck(result, true);
        }

        [TestMethod]
        public async Task User_Delete()
        {
            // create user
            var addUseDto = await UserGenerator.GenerateRandomAddUserDto();
            _rolesToDelete.AddUnique(addUseDto.RoleId);
            var user = await _userRepo.CreateUser(addUseDto);
            _usersToDelete.AddUnique(user.Id);

            // check response
            UserDtoValidator.UserDetailCheck(user);
            Validator.IsTrue(string.Equals(addUseDto.Email, user.Email));
            Validator.IsTrue(string.Equals(addUseDto.GivenName, user.GivenName));
            Validator.IsTrue(string.Equals(addUseDto.Surname, user.Surname));
            Validator.IsTrue(string.Equals(addUseDto.TemplateId, user.TemplateId));
            Validator.IsTrue(string.Equals(addUseDto.RoleId, user.RoleId));
            Validator.IsTrue(string.Equals(addUseDto.PersonTagList[0].TagNumber.ToUpper(), user.TagNumber));
            // check we can get the associated person
            var userPerson = await _personRepo.GetPersonById(user.PersonId);
            Validator.IsTrue(string.Equals(user.Email, userPerson.Email));
            Validator.IsTrue(string.Equals(user.GivenName, userPerson.GivenName));
            Validator.IsTrue(string.Equals(user.Surname, userPerson.Surname));
            Validator.IsTrue(string.Equals(user.TemplateId, userPerson.TemplateId));
            Validator.IsTrue(string.Equals(user.PersonTagList[0].TagNumber.ToUpper(), userPerson.TagNumber));

            // delete the user
            var deleteResult = await _userRepo.DeleteUser(user.Id);
            Validator.IsTrue(deleteResult);

            // remove from delete list
            _usersToDelete.Remove(user.Id);

            // verify
            var query = QueryBuilder<UserSummaryDto>.NewQuery(p => p.Id, user.Id, ComparisonOperator.Equals).Build();
            var queryResult = await _userRepo.QueryUsers(query); // get the user again
            Validator.IsFalse(queryResult.Any(p => p.Id == user.Id)); // check our user is actually gone

            // verify with get
            try
            {
                var sameItem = await _userRepo.GetUserById(user.Id);

                Validator.IsTrue(false, "Deleted entity returned");
            }
            catch (Exception e)
            {
                // this is expected                
            }

            // should still be able to get the person
            userPerson = await _personRepo.GetPersonById(user.PersonId);

            // check response
            Validator.IsTrue(string.Equals(user.Email, userPerson.Email));
            Validator.IsTrue(string.Equals(user.GivenName, userPerson.GivenName));
            Validator.IsTrue(string.Equals(user.Surname, userPerson.Surname));
            Validator.IsTrue(string.Equals(user.TemplateId, userPerson.TemplateId));
            Validator.IsTrue(string.Equals(user.PersonTagList[0].TagNumber.ToUpper(), userPerson.TagNumber));

            // cleanup
            await _personRepo.DeletePerson(user.PersonId);
        }

        [TestMethod]
        public async Task User_DeleteAndPerson()
        {
            // create user
            var addUseDto = await UserGenerator.GenerateRandomAddUserDto();
            _rolesToDelete.AddUnique(addUseDto.RoleId);
            var user = await _userRepo.CreateUser(addUseDto);
            _usersToDelete.AddUnique(user.Id);

            // check response
            UserDtoValidator.UserDetailCheck(user);
            Validator.IsTrue(string.Equals(addUseDto.Email, user.Email));
            Validator.IsTrue(string.Equals(addUseDto.GivenName, user.GivenName));
            Validator.IsTrue(string.Equals(addUseDto.Surname, user.Surname));
            Validator.IsTrue(string.Equals(addUseDto.TemplateId, user.TemplateId));
            Validator.IsTrue(string.Equals(addUseDto.RoleId, user.RoleId));
            Validator.IsTrue(string.Equals(addUseDto.PersonTagList[0].TagNumber.ToUpper(), user.TagNumber));
            // check we can get the associated person
            var userPerson = await _personRepo.GetPersonById(user.PersonId);
            Validator.IsTrue(string.Equals(user.Email, userPerson.Email));
            Validator.IsTrue(string.Equals(user.GivenName, userPerson.GivenName));
            Validator.IsTrue(string.Equals(user.Surname, userPerson.Surname));
            Validator.IsTrue(string.Equals(user.TemplateId, userPerson.TemplateId));
            Validator.IsTrue(string.Equals(user.PersonTagList[0].TagNumber.ToUpper(), userPerson.TagNumber));

            // delete the user
            var deleteResult = await _userRepo.DeleteUserAndPerson(user.Id);
            Validator.IsTrue(deleteResult);

            // remove from delete list
            _usersToDelete.Remove(user.Id);

            // verify
            var query = QueryBuilder<UserSummaryDto>.NewQuery(p => p.Id, user.Id, ComparisonOperator.Equals).Build();
            var queryResult = await _userRepo.QueryUsers(query); // get the user again
            Validator.IsFalse(queryResult.Any(p => p.Id == user.Id)); // check our user is actually gone

            // verify with get
            try
            {
                var sameItem = await _userRepo.GetUserById(user.Id);

                Validator.IsTrue(false, "Deleted entity returned");
            }
            catch (Exception e)
            {
                // this is expected                
            }

            // verify person is also deleted
            var queryP = QueryBuilder<PersonSummaryDto>.NewQuery(p => p.Id, user.Id, ComparisonOperator.Equals).Build();
            var queryPResult = await _personRepo.QueryPersons(queryP); // get the person again
            Validator.IsFalse(queryPResult.Any(p => p.Id == user.PersonId)); // check our person is actually gone

            // verify with get
            try
            {
                var sameItem = await _personRepo.GetPersonById(user.PersonId);

                Validator.IsTrue(false, "Deleted entity returned");
            }
            catch (Exception e)
            {
                // this is expected                
            }
        }
    }
}
