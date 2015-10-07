using System;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Exceptions;
using Locafi.Client.Repo;
using Locafi.Client.UnitTests.Implementations;
using Locafi.Client.UnitTests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Locafi.Client.UnitTests.Tests.Rian
{
    [TestClass]
    public class WebRepoTests
    {
        [TestMethod]
        public async Task WebRepo_Unauthorised()
        {
            var funcWasCalled = false;
            var transferer = new UnauthorisedMockHttpTransferer();
            var config = new MockAuthorisedHttpConfigService();
            
            config.OnUnauthorised = service =>
            {
                return Task<IAuthorisedHttpTransferConfigService>.Factory.StartNew(() =>
                {
                    funcWasCalled = true;
                    return config;
                });
            };


            var itemRepo = new ItemRepo(transferer, config, new Serialiser());
            var exceptionThrown = false;
            try
            {
                var c = await itemRepo.GetItemCount();
            }
            catch(WebRepoUnauthorisedException ex)
            {
                exceptionThrown = true;
            }
            Assert.IsTrue(funcWasCalled, "func called");
            Assert.IsTrue(exceptionThrown, "exception thrown");
        }
    }
}
