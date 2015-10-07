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
        [ExpectedException(typeof(WebRepoUnauthorisedException))]
        public async Task WebRepo_Unauthorised()
        {
            var transferer = new UnauthorisedMockHttpTransferer();
            var config = new MockAuthorisedHttpConfigService();

            var itemRepo = new ItemRepo(transferer, config, new Serialiser());

            var c = await itemRepo.GetItemCount();

        }
    }
}
