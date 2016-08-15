using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Model.Dto.Skus;
using Locafi.Client.Model.Enums;
using Locafi.Client.Model.Query;
using Locafi.Client.Model.Query.PropertyComparison;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Locafi.Client.UnitTests.Implementations;
using Locafi.Client.Processors.Encoding;

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

  //      [TestMethod]
        public async Task Sku_Create()
        {
            var ran = new Random();
            var companyPrefix = ran.Next(9999).ToString().PadLeft(4);
            var itemReference = ran.Next(9999).ToString().PadLeft(4);
            var description = Guid.NewGuid().ToString();
            var name = Guid.NewGuid().ToString();

            var templates = await _templateRepo.GetTemplatesForType(TemplateFor.Item);
            var template = templates.Items.ElementAt(ran.Next(templates.Items.Count() - 1));
            var templateDetail = await _templateRepo.GetById(template.Id);
            var extendedProperties = new List<WriteSkuExtendedPropertyDto>();
            foreach (var extendedPropRequired in templateDetail.TemplateExtendedPropertyList)
            {
                var extendedProperty = new WriteSkuExtendedPropertyDto();
                extendedProperty.ExtendedPropertyId = extendedPropRequired.ExtendedPropertyId;
                //extendedProperty.
                switch (extendedPropRequired.ExtendedPropertyDataType)
                {
                  //  case TemplateDataTypes.Bool:

                }
            }
            var sku = new AddSkuDto
            {
                CompanyPrefix = companyPrefix,
                Description = description,
                ItemReference = itemReference,
                ItemTemplateId = template.Id,
                Name = name,
             //   SkuExtendedPropertyList = 
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
            var skus = await _skuRepo.QuerySkus();

            Assert.IsNotNull(skus);
            Assert.IsInstanceOfType(skus, typeof(IEnumerable<SkuSummaryDto>));
            Assert.IsTrue(skus.Count > 0);
        }

        [TestMethod]
        public async Task Sku_Query()
        {
            var skus = await _skuRepo.QuerySkus();
            Assert.IsNotNull(skus);
            Assert.IsTrue(skus.Count > 0); // we have at least 1 sku

            var ran = new Random();
            var sku = skus.Items.ElementAt(ran.Next(skus.Items.Count() - 1));

            var query = new SkuQuery();
            query.CreateQuery(s => s.Name, sku.Name, ComparisonOperator.Equals);
            var result = await _skuRepo.QuerySkus(query);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Items.Contains(sku));

            query.CreateQuery(s => s.TemplateId, sku.TemplateId, ComparisonOperator.Equals);
            result = await _skuRepo.QuerySkus(query);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Items.Contains(sku));

            var withGtin = skus.Where(s => !string.IsNullOrEmpty(s.Gtin));
            var skuWithGin = withGtin.FirstOrDefault();
            Assert.IsNotNull(skuWithGin, "No Gtins :(");
            query.CreateQuery(s=>s.Gtin, skuWithGin.Gtin,ComparisonOperator.Equals);
            result = await _skuRepo.QuerySkus(query);
            Assert.IsTrue(result.Items.Contains(skuWithGin));

        }

        [TestMethod]
        public async Task Sku_GetDetails()
        {
            var ran = new Random();
            var skus = await _skuRepo.QuerySkus();
            Assert.IsNotNull(skus);
            Assert.IsInstanceOfType(skus,typeof(IEnumerable<SkuSummaryDto>));
            foreach (var sku in skus)
            {
                var detail = await _skuRepo.GetSku(sku.Id);
                Assert.IsNotNull(detail,"detail != null");
                Assert.AreEqual(sku,detail);
            }

        }

        [TestMethod]
        public async Task SgtinDecodeTest()
        {
            var tag = new TestTag("52414D5000A0001000000027");

            Assert.IsFalse(tag.HasSgtin());

            tag.TagNumber = "30340003EB5BAF8000000243";

            Assert.IsTrue(tag.HasSgtin());
        }

        #region Private Methods


        private async Task<AddSkuDto> GenerateRandomSgtinSkuDto()
        {
            var ran = new Random();
            var templates = await _templateRepo.GetTemplatesForType(TemplateFor.Place);
            var template = await _templateRepo.GetById(templates.Items.ElementAt(ran.Next(templates.Items.Count() - 1)).Id);
            var name = "Random - " + template.Name + " " + ran.Next().ToString();
            var description = name + " - Description";
            var companyPrefix = ran.Next(9999).ToString().PadLeft(4);
            var itemReference = ran.Next(9999).ToString().PadLeft(4);

            var addSku = new AddSkuDto(template)
            {
                CompanyPrefix = companyPrefix,
                Description = description,
                ItemReference = itemReference,
                ItemTemplateId = template.Id,
                Name = name
            };

            return addSku;
        }

        #endregion
    }
}
