using System;
using System.Threading.Tasks;
using Locafi.Client.Contract.Services;
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
    }
}
