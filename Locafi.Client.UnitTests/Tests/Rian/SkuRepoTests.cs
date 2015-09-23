﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Model.Dto.Skus;
using Locafi.Client.Model.Enums;
using Locafi.Client.Model.Query;
using Locafi.Client.Model.Query.PropertyComparison;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Locafi.Client.UnitTests.Tests.Rian
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

    //    [TestMethod]
        public async Task Sku_Create()
        {
            var ran = new Random();
            var companyPrefix = ran.Next(9999).ToString().PadLeft(4);
            var itemReference = ran.Next(9999).ToString().PadLeft(4);
            var description = Guid.NewGuid().ToString();
            var name = Guid.NewGuid().ToString();

            var templates = await _templateRepo.GetTemplatesForType(TemplateFor.Item);
            var template = templates[ran.Next(templates.Count - 1)];
            var templateDetail = await _templateRepo.GetById(template.Id);
            foreach (var extendedProp in templateDetail.TemplateExtendedPropertyList)
            {
                //extendedProp.
            }
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
        public async Task Sku_Query()
        {
            var skus = await _skuRepo.GetAllSkus();
            Assert.IsNotNull(skus);
            Assert.IsTrue(skus.Count > 0); // we have at least 1 sku

            var ran = new Random();
            var sku = skus[ran.Next(skus.Count - 1)];

            var query = new RegularSkuQuery();
            query.CreateQuery(s => s.Name, sku.Name, ComparisonOperator.Equals);
            var result = await _skuRepo.QuerySkus(query);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Contains(sku));

            query.CreateQuery(s => s.TemplateId, sku.TemplateId, ComparisonOperator.Equals);
            result = await _skuRepo.QuerySkus(query);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Contains(sku));

            var withGtin = skus.Where(s => !string.IsNullOrEmpty(s.Gtin));
            var skuWithGin = withGtin.FirstOrDefault();
            Assert.IsNotNull(skuWithGin, "No Gtins :(");
            query.CreateQuery(s=>s.Gtin, skuWithGin.Gtin,ComparisonOperator.Equals);
            result = await _skuRepo.QuerySkus(query);
            Assert.IsTrue(result.Contains(skuWithGin));

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
     
    }
}
