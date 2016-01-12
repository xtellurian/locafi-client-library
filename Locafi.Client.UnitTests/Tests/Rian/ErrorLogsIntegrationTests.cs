using System;
using System.Threading.Tasks;
using Locafi.Client.Model.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Locafi.Client.UnitTests.Tests.Rian
{
    [TestClass]
    public class ErrorLogsIntegrationTests : WebRepoTestsBase
    {
        [TestMethod]
        public async Task ErrorLogs_SendException()
        {
            var errorRepo = ErrorRepo;
            var message = "This is a test -- " + Guid.NewGuid();
            var innerMessage = Guid.NewGuid().ToString();
            var exception = new TestException(message, new Exception(innerMessage));

            try
            {
                throw exception;
            }
            catch (TestException ex)
            {
                var result = await errorRepo.LogException(ex, ErrorLevel.Trivial);
                Assert.IsNotNull(result);
                Assert.IsTrue(string.Equals(result.ErrorMessage, message));
                Assert.IsTrue(result.ErrorDetails.Contains(innerMessage));
            }
        }

        internal class TestException : Exception
        {
            public TestException(string message, Exception innerException) : base(message, innerException)
            {
            }
        }
    }
}
