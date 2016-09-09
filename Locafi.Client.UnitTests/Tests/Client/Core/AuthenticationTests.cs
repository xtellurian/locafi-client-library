using System;
using System.Threading.Tasks;
using Locafi.Client.Contract.Crypto;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Model.Dto.Inventory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Locafi.Builder;
using Locafi.Client.UnitTests.Validators;
using Locafi.Client.Model;
using Locafi.Client.Model.Dto.Items;

namespace Locafi.Client.UnitTests.Tests
{
    [TestClass]
    public class AuthenticationTests
    {
        private IAuthenticationRepo _authRepo;
        private ISha256HashService _hashService;
        private IItemRepo _itemRepo;

        [TestInitialize]
        public void Initialize()
        {
            _authRepo = WebRepoContainer.AuthRepo;
            _hashService = ServiceContainer.HashService;
            _itemRepo = WebRepoContainer.ItemRepo;
        }

        [TestMethod]
        public async Task Authentication_SuccessfulLogin()
        { 
            var result = await _authRepo.Login(DevEnvironment.TestUserEmail, DevEnvironment.TestUserPassword);

            AuthenticationDtoValidator.AuthenticationResponseCheck(result, true);

            // change to use our new token
            await WebRepoContainer.AuthorisedHttpTransferConfigService.SetTokenGroupAsync(result.TokenGroup);

            // validate that we can use the token
            var items = await _itemRepo.QueryItems();

            Validator.IsNotNull(items);
            Validator.IsInstanceOfType(items, typeof(PageResult<ItemSummaryDto>));
        }

        [TestMethod]
        public async Task Authentication_FailedLogin()
        {
            var name = Guid.NewGuid().ToString();
            var password = Guid.NewGuid().ToString();

            var result = await _authRepo.Login(name, password);

            AuthenticationDtoValidator.AuthenticationResponseCheck(result, false);
        }

        [TestMethod]
        public async Task Authentication_RefreshLogin()
        {
            var result = await _authRepo.Login(DevEnvironment.TestUserEmail, DevEnvironment.TestUserPassword);

            AuthenticationDtoValidator.AuthenticationResponseCheck(result, true);

            var token = result.TokenGroup.Refresh;
            result = await _authRepo.RefreshLogin(token);

            AuthenticationDtoValidator.AuthenticationResponseCheck(result, true);

            // change to use our new token
            await WebRepoContainer.AuthorisedHttpTransferConfigService.SetTokenGroupAsync(result.TokenGroup);

            // validate that we can use the token
            var items = await _itemRepo.QueryItems();

            Validator.IsNotNull(items);
            Validator.IsInstanceOfType(items, typeof(PageResult<ItemSummaryDto>));
        }

        [TestMethod]
        public async Task Authentication_PortalLogin() // changed implemention on server side
        {
            var password = _hashService.GenerateHash(StringConstants.Secret, StringConstants.PortalUsername);
            var result = await _authRepo.PortalLogin(StringConstants.PortalUsername, password);

            AuthenticationDtoValidator.AuthenticationResponseCheck(result, true);
        }

        [TestMethod]
        public async Task Authentication_PortalLoginBadSerialNumber() // changed implemention on server side
        {
            var password = _hashService.GenerateHash(StringConstants.Secret, "skjdbgsijbo");
            var result = await _authRepo.PortalLogin(StringConstants.PortalUsername, password);

            AuthenticationDtoValidator.AuthenticationResponseCheck(result, false);
        }

    }
}
