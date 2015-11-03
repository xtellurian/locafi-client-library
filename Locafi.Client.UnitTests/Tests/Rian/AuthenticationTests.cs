using System;
using System.Threading.Tasks;
using Locafi.Client.Contract.Crypto;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Model.Dto.Inventory;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Locafi.Client.UnitTests.Tests.Rian
{
    [TestClass]
    public class AuthenticationTests
    {
        private IAuthenticationRepo _authRepo;
        private ISha256HashService _hashService;

        [TestInitialize]
        public void Initialize()
        {
            _authRepo = WebRepoContainer.AuthRepo;
            _hashService = ServiceContainer.HashService;
        }
        [TestMethod]
        public async Task Authentication_SuccessfulLogin()
        { 
            var result = await _authRepo.Login(StringConstants.TestingEmailAddress, StringConstants.Password);
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
            var result = await _authRepo.Login(StringConstants.TestingEmailAddress, StringConstants.Password);
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

        [TestMethod]
        public async Task Authentication_ReaderLogin() // changed implemention on server side
        {
            var password = _hashService.GenerateHash(StringConstants.Secret, StringConstants.ReaderUserName);
            var result = await _authRepo.ReaderLogin(StringConstants.ReaderUserName, password);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.TokenGroup);
            Assert.IsFalse(string.IsNullOrEmpty(result.TokenGroup.Token));
            Assert.IsFalse(string.IsNullOrEmpty(result.TokenGroup.Refresh));
        }

        [TestMethod]
        public async Task Authentication_ReaderLoginBadSerialNumber() // changed implemention on server side
        {
            var password = _hashService.GenerateHash(StringConstants.Secret, "skjdbgsijbo");
            var result = await _authRepo.ReaderLogin(StringConstants.ReaderUserName, password);
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.TokenGroup);
            Assert.IsNull(result.TokenGroup.Token);
            Assert.IsNull(result.TokenGroup.Refresh);
        }

    }
}
