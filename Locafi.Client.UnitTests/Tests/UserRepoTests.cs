using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Contract.Services;
using Locafi.Client.Data;
using Locafi.Client.Model.Query;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Locafi.Client.UnitTests.Tests
{
    [TestClass]
    public class UserRepoTests
    {
        private IUserRepo _userRepo;
        [TestInitialize]
        public void Initialize()
        {
            _userRepo = WebRepoContainer.UserRepo;
        }

        [TestMethod]
        public async Task User_GetAllUsers()
        {
            var users = await _userRepo.GetAllUsers();
            Assert.IsNotNull(users);
            Assert.IsInstanceOfType(users,typeof(IEnumerable<UserDto>));

        }

        [TestMethod]
        public async Task User_QueryUsers()
        {
            var ran = new Random();
            var users = await _userRepo.GetAllUsers();
            var user = users[ran.Next(users.Count - 1)];
            Assert.IsNotNull(user);
            Assert.IsInstanceOfType(user,typeof(UserDto));
            // query surname
            if (!string.IsNullOrEmpty(user.Surname))
            {
                var query1 = new SimpleUserQuery(user.Surname, SimpleUserQuery.SearchParameter.Surname);
                var result1 = await _userRepo.QueryUsers(query1);
                Assert.IsTrue(result1.Contains(user));
            }

            if (!string.IsNullOrEmpty(user.EmailAddress))
            {
                var query2 = new SimpleUserQuery(user.EmailAddress, SimpleUserQuery.SearchParameter.Email);
                var result2 = await _userRepo.QueryUsers(query2);
                Assert.IsTrue(result2.Contains(user));
            }

            // query given name
            if (!string.IsNullOrEmpty(user.GivenName))
            {
                var query3 = new SimpleUserQuery(user.GivenName, SimpleUserQuery.SearchParameter.GivenName);
                var result3 = await _userRepo.QueryUsers(query3);
                Assert.IsTrue(result3.Contains(user));
            }
            
            // query username - cannot be null or empty
            var query4 = new SimpleUserQuery(user.UserName, SimpleUserQuery.SearchParameter.UserName);
            var result4 = await _userRepo.QueryUsers(query4);
            Assert.IsTrue(result4.Contains(user));
        }
    }
}
