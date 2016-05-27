using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Model.Dto.Users;
using Locafi.Client.Model.Query;
using Locafi.Client.Model.Query.PropertyComparison;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Locafi.Client.UnitTests.Tests.Rian
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
            var users = await _userRepo.QueryUsers();
            Assert.IsNotNull(users);
            Assert.IsInstanceOfType(users,typeof(IEnumerable<UserSummaryDto>));
            Assert.IsTrue(users.Count > 0);
        }

       [TestMethod]
        public async Task User_QueryUsers()
        {
            var ran = new Random();
            var users = await _userRepo.QueryUsers();
            var user = users.Items.ElementAt(ran.Next(users.Items.Count() - 1));
            Assert.IsNotNull(user);
            Assert.IsInstanceOfType(user,typeof(UserSummaryDto));
            // query surname
            var q = new UserQuery();
            q.CreateQuery(u=>u.Email,user.Email,ComparisonOperator.Equals);
            var result = await _userRepo.QueryUsers(q);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Items.Contains(user));
        }
    }
}
