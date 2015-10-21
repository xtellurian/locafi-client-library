using System;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Exceptions;
using Locafi.Client.Repo;
using Locafi.Client.UnitTests.Factory;
using Locafi.Client.UnitTests.Implementations;
using Locafi.Client.UnitTests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Bson;

namespace Locafi.Client.UnitTests.Tests.Rian
{
    [TestClass]
    public class WebRepoTests
    {

        [TestMethod]
        [ExpectedException(typeof(WebRepoUnauthorisedException))]
        public async Task WebRepo_Unauthorised()
        {
            var transferer = new UnauthorisedMockHttpTransferer();
            var config = new MockAuthorisedHttpConfigService();

            var skuRepo = new SkuRepo(transferer, config, new Serialiser());

            var c = await skuRepo.GetAllSkus();

        }
        [TestMethod]
        public async Task WebRepo_Reauthorise()
        {
            var authConfigService = await HttpConfigFactory.Generate(StringConstants.BaseUrl, StringConstants.TestingEmailAddress,
                StringConstants.Password);
            var skuRepo = new SkuRepo(authConfigService, new Serialiser()); // use sku repo as test
            var skus = await skuRepo.GetAllSkus();
            Assert.IsNotNull(skus);
            var tokenGroup = await authConfigService.GetTokenGroupAsync();
            tokenGroup.Token = "";
            await authConfigService.SetTokenGroupAsync(tokenGroup);

            skus = await skuRepo.GetAllSkus();
            Assert.IsNotNull(skus);

            var secondTokenGroup= await authConfigService.GetTokenGroupAsync();
            Assert.IsNotNull(secondTokenGroup?.Token);
            Assert.IsFalse(string.IsNullOrEmpty(secondTokenGroup.Token)); // the token has been refreshed
        }
    }

    
}
