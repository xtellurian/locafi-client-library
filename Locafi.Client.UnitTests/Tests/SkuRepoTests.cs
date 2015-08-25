using System;
using System.Collections.Generic;
using Locafi.Client.Contract.Services;
using Locafi.Client.Model.Dto.Skus;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Locafi.Client.UnitTests.Tests
{
    [TestClass]
    public class SkuRepoTests
    {
        private ISkuRepo _skuRepo;

        [TestInitialize]
        public void Initialize()
        {
            _skuRepo = WebRepoContainer.SkuRepo;
        }

        [TestMethod]
        public async void GetAllSkus()
        {
            var skus = await _skuRepo.GetAllSkus();

            Assert.IsNotNull(skus);
            Assert.IsInstanceOfType(skus, typeof(IEnumerable<SkuSummaryDto>));
            Assert.IsTrue(skus.Count > 0);
        }
    }
}
