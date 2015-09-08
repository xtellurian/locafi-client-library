﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Model.Dto.Users;
using Locafi.Client.Model.Query;
using Locafi.Client.Model.Query.PropertyComparison;
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
            Assert.IsInstanceOfType(users,typeof(IEnumerable<UserSummaryDto>));

        }

       [TestMethod]
        public async Task User_QueryUsers()
        {
            var ran = new Random();
            var users = await _userRepo.GetAllUsers();
            var user = users[ran.Next(users.Count - 1)];
            Assert.IsNotNull(user);
            Assert.IsInstanceOfType(user,typeof(UserSummaryDto));
            // query surname
            var q = new UserQuery();
            q.CreateQuery(u=>u.EmailAddress,user.EmailAddress,ComparisonOperator.Equals);
            var result = await _userRepo.QueryUsers(q);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Contains(user));
        }
    }
}
