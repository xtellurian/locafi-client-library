using System;
using System.Threading.Tasks;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Locafi.Client.UnitTests.Tests
{
    [TestClass]
    public class AuthenticationTests
    {
        private IAuthenticationRepo _authRepo;
        [TestInitialize]
        public void Initialize()
        {
            _authRepo = WebRepoContainer.AuthRepo;
        }
        [TestMethod]
        public async Task Authentication_SuccessfulLogin()
        { 
            var result = await _authRepo.Login(StringConstants.UserName, StringConstants.Password);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.TokenGroup);
            Assert.IsNotNull(result.TokenGroup.Token);
            Assert.IsNotNull(result.TokenGroup.Refresh);
        }

        [TestMethod]
        public async Task Authentication_FailedLogin()
        {
            var name = Guid.NewGuid().ToString();
            var password = Guid.NewGuid().ToString();

            var result = await _authRepo.Login(name, password);
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Messages);
            Assert.IsTrue(result.Messages.Count > 0);
        }

        [TestMethod]
        public async Task Authentication_RefreshLogin()
        {
            var result = await _authRepo.Login(StringConstants.UserName, StringConstants.Password);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.TokenGroup);
            Assert.IsFalse(string.IsNullOrEmpty(result.TokenGroup.Token));
            Assert.IsFalse(string.IsNullOrEmpty(result.TokenGroup.Refresh));

            var token = result.TokenGroup.Refresh;
            result = await _authRepo.RefreshLogin(token);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.TokenGroup);
            Assert.IsFalse(string.IsNullOrEmpty(result.TokenGroup.Token));
            Assert.IsFalse(string.IsNullOrEmpty(result.TokenGroup.Refresh));
        }
    }
}
