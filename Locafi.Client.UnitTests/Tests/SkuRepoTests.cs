using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        public async Task GetAllSkus()
        {
            var skus = await _skuRepo.GetAllSkus();

            Assert.IsNotNull(skus);
            Assert.IsInstanceOfType(skus, typeof(IEnumerable<SkuSummaryDto>));
            Assert.IsTrue(skus.Count > 0);
        }

        [TestMethod]
        public async Task GetSkuDetails()
        {
            var ran = new Random();
            var skus = await _skuRepo.GetAllSkus();
            var sku1Summary = skus[ran.Next(skus.Count - 1)];
            var sku2Summary = skus[ran.Next(skus.Count - 1)];

            Assert.IsNotNull(sku1Summary);
            Assert.IsNotNull(sku2Summary);
            Assert.IsInstanceOfType(sku1Summary, typeof(SkuSummaryDto));
            Assert.IsInstanceOfType(sku2Summary, typeof(SkuSummaryDto));

            var sku1Detail = await _skuRepo.GetSkuDetail(sku1Summary.Id.ToString());
            var sku2Detail = await _skuRepo.GetSkuDetail(sku2Summary.Id.ToString());

            Assert.IsNotNull(sku1Detail);
            Assert.IsNotNull(sku2Detail);
            Assert.IsInstanceOfType(sku1Detail, typeof(SkuDetailDto));
            Assert.IsInstanceOfType(sku2Detail, typeof(SkuDetailDto));
            Assert.AreEqual(sku1Summary.Id, sku1Detail.Id);
            Assert.AreEqual(sku2Summary.Id, sku2Detail.Id);
        }

        public async Task GetSkuRefFromSgtin()
        {
            
        }
    }
}
