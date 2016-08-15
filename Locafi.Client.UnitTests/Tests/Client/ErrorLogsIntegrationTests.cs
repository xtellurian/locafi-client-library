using System;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.ErrorLogs;
using Locafi.Client.Model.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Locafi.Client.UnitTests.Tests
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

            var arbMes = "This is a test -- " + Guid.NewGuid();
            var arbDet = "arbitrary detail -- " + Guid.NewGuid();
            var result2 = await errorRepo.LogArbitrary(new AddErrorLogDto(arbMes, arbDet, DateTime.Now, ErrorLevel.Trivial));
            Assert.IsNotNull(result2);
            Assert.IsTrue(string.Equals(result2.ErrorMessage, arbMes));
            Assert.IsTrue(result2.ErrorDetails.Contains(arbDet));
        }

        internal class TestException : Exception
        {
            public TestException(string message, Exception innerException) : base(message, innerException)
            {
            }
        }
    }
}
