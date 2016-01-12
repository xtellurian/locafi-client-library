using System;
using System.Linq;
using System.Threading.Tasks;
using Locafi.Client.Model.RelativeUri;
using Locafi.Client.Repo;
using Locafi.Client.UnitTests.Implementations;
using Locafi.Client.UnitTests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Locafi.Client.UnitTests.Tests.Rian
{
    [TestClass]
    public class ErrorLogsUnitTest
    {
        [TestMethod]
        public async Task ErrorLogRepo_UnitTest()
        {
            var config = new MockAuthorisedHttpConfigService();
            var tran = new MockHttpTransferrer();
            var serialiser = new Serialiser();


            var errorLogRepo = new ErrorRepo(tran, config, serialiser );
            var message = "Test Message -- " + Guid.NewGuid();
            var innerMessage = Guid.NewGuid().ToString();
            var exception = new ErrorLogsIntegrationTests.TestException(message, new Exception(innerMessage));

            var response = await errorLogRepo.LogException(exception);

            Assert.IsNotNull(response.ErrorDetails);
            Assert.IsTrue(tran.HttpCalls.Keys.Any(k => k.Contains(ErrorLogUri.ServiceName))); // Assert we called the Error Log Service
            
        }
    }
}
