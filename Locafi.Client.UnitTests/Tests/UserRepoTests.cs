using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Data;
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

       
        public async Task User_QueryUsers()
        {
            var ran = new Random();
            var users = await _userRepo.GetAllUsers();
            var user = users[ran.Next(users.Count - 1)];
            Assert.IsNotNull(user);
            Assert.IsInstanceOfType(user,typeof(UserDto));
            // query surname
            
        }
    }
}
