using System;
using System.Threading.Tasks;
using Locafi.Client.Contract.Repo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Locafi.Client.UnitTests.Tests.Anthony
{
    [TestClass]
    public class ReaderRepoTests
    {        
        private IReaderRepo _readerRepo;

        [TestInitialize]
        public void Initialize()
        {
            _readerRepo = WebRepoAsAuthorisedReaderContainer.ReaderRepo;
        }

        [TestMethod]
        public async Task Reader_GetBySerial()
        {
            var reader = await _readerRepo.GetReaderBySerial(StringConstants.ReaderUserName);
            Assert.IsNotNull(reader);
        }
    }
}
