using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Model.Dto.Skus;
using Locafi.Client.Model.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Locafi.Client.UnitTests.Tests
{
    [TestClass]
    public class SkuRepoTests
    {
        private ISkuRepo _skuRepo;
        private ITemplateRepo _templateRepo;
        private IList<Guid> _toDelete;

        [TestInitialize]
        public void Initialize()
        {
            _skuRepo = WebRepoContainer.SkuRepo;
            _templateRepo = WebRepoContainer.TemplateRepo;
            _toDelete = new List<Guid>();
        }

        [TestMethod]
        public async Task Sku_Create()
        {
            var ran = new Random();
            var companyPrefix = ran.Next(9999).ToString().PadLeft(4);
            var itemReference = ran.Next(9999).ToString().PadLeft(4);
            var description = Guid.NewGuid().ToString();
            var name = Guid.NewGuid().ToString();

            var templates = await _templateRepo.GetTemplatesForType(TemplateFor.Item);
            var template = templates[ran.Next(templates.Count - 1)];

            var sku = new AddSkuDto
            {
                CompanyPrefix = companyPrefix,
                Description = description,
                ItemReference = itemReference,
                ItemTemplateId = template.Id,
                Name = name,
                SkuExtendedPropertyList = new List<WriteSkuExtendedPropertyDto>()
            };

            var result = await _skuRepo.CreateSku(sku);
            Assert.IsNotNull(result);
            _toDelete.Add(result.Id);
            Assert.IsInstanceOfType(result, typeof(SkuDetailDto));
            Assert.IsTrue(string.Equals(result.ItemReference,sku.ItemReference));
            Assert.IsTrue(string.Equals(result.CompanyPrefix, sku.CompanyPrefix));
        }

        [TestMethod]
        public async Task Sku_GetAll()
        {
            var skus = await _skuRepo.GetAllSkus();

            Assert.IsNotNull(skus);
            Assert.IsInstanceOfType(skus, typeof(IEnumerable<SkuSummaryDto>));
            Assert.IsTrue(skus.Count > 0);
        }

        [TestMethod]
        public async Task Sku_GetDetails()
        {
            var ran = new Random();
            var skus = await _skuRepo.GetAllSkus();
            Assert.IsNotNull(skus);
            Assert.IsInstanceOfType(skus,typeof(IEnumerable<SkuSummaryDto>));
            foreach (var sku in skus)
            {
                var detail = await _skuRepo.GetSkuDetail(sku.Id);
                Assert.IsNotNull(detail,"detail != null");
                Assert.AreEqual(sku,detail);
            }

        }

        public async Task GetSkuRefFromSgtin()
        {
            
        }
        [TestCleanup]
        public async void Cleanup()
        {
            foreach (var id in _toDelete)
            {
                await _skuRepo.Delete(id);
            }
        }
    }
}
