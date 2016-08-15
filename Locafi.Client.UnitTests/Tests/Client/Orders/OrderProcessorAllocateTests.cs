using System;
using System.Linq;
using System.Threading.Tasks;
using Locafi.Client.Contract.Processors;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Exceptions;
using Locafi.Client.Model.Dto.Orders;
using Locafi.Client.Model.Dto.Snapshots;
using Locafi.Client.Processors.Orders;
using Locafi.Client.UnitTests.Implementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Locafi.Client.UnitTests.Tests.Orders
{
    [TestClass]
    public class OrderProcessorAllocateTests
    {

        private ISkuRepo _skuRepo;
        private ITagReservationRepo _tagReservationRepo;

        [TestInitialize]
        public void Initialise()
        {

            _skuRepo = WebRepoContainer.SkuRepo;
            _tagReservationRepo = WebRepoContainer.TagReservationRepo;

        }
        [TestMethod]
        public async Task OrderProcessor_AllocateExact()
        {
            var ran = new Random();

            var quantity = ran.Next(1, 10);
            var skus = await _skuRepo.QuerySkus();
            var sku = skus.FirstOrDefault(s => !string.IsNullOrEmpty(s.Gtin));
            var reservation = await _tagReservationRepo.ReserveTagsForSku(sku.Id, quantity);

            var order = new OrderDetailDto();

            order.ExpectedSkus.Add(new OrderSkuLineItemDto
            {
                Name = sku.Name,
                PackingSize = 1,
                Quantity = quantity,
                Gtin = sku.Gtin,
            });
            var processor = new OrderAllocator(order);

            foreach (var number in reservation.TagNumbers)
            {
                var result = processor.Add(new TestTag(number));
                Assert.IsTrue(result.IsRecognised, "Tag Recognised");
            }

            Assert.IsTrue(processor.GetAddTags().Count == quantity);
            Assert.IsTrue(processor.OrderDetail.UnknownTags.Count == 0);
            Assert.IsTrue(order.ExpectedSkus.FirstOrDefault().AllocatedTagNumbers.Count == quantity, "Allocated Count == quantity");
            Assert.AreEqual(order, processor.OrderDetail);
        }

        [TestMethod]
        public async Task OrderProcessor_AllocateDuplicates()
        {
            var quantity = 1;
            var skus = await _skuRepo.QuerySkus();
            var sku = skus.FirstOrDefault(s => !string.IsNullOrEmpty(s.Gtin));
            var reservation = await _tagReservationRepo.ReserveTagsForSku(sku.Id, quantity);


            var order = new OrderDetailDto();
            var tagNumber = reservation.TagNumbers.FirstOrDefault();

            order.ExpectedSkus.Add(new OrderSkuLineItemDto
            {
                Name = sku.Name,
                PackingSize = 1,
                Quantity = quantity,
                Gtin = sku.Gtin
            });
            var processor = new OrderAllocator(order);

            for (var i = 0; i < 10; i++)
            {
                var result = processor.Add(new TestTag(tagNumber));
                Assert.IsTrue(result.IsRecognised, "Tag Recognised");
            }
            Assert.IsTrue(processor.GetAddTags().Count == quantity, "got 1 snapshot tag" );
            Assert.IsTrue(order.ExpectedSkus.FirstOrDefault().AllocatedTagNumbers.Count == 1, "Got one tag");

            Assert.IsTrue(processor.OrderDetail.UnknownTags.Count == 0, "no Unknown tags");
        }

        [TestMethod]
        public async Task OrderProcessor_RemoveAllocate()
        {
            var ran = new Random();

            var quantity = ran.Next(1, 10);
            var skus = await _skuRepo.QuerySkus();
            var sku = skus.FirstOrDefault(s => !string.IsNullOrEmpty(s.Gtin));
            var reservation = await _tagReservationRepo.ReserveTagsForSku(sku.Id, quantity);

            var order = new OrderDetailDto();

            order.ExpectedSkus.Add(new OrderSkuLineItemDto
            {
                Name = sku.Name,
                PackingSize = 1,
                Quantity = quantity,
                Gtin = sku.Gtin,
            });
            var processor = new OrderAllocator(order);

            foreach (var number in reservation.TagNumbers)
            {
                var result = processor.Add(new TestTag(number));
                Assert.IsTrue(result.IsRecognised, "Tag Recognised");
            }

            Assert.IsTrue(processor.GetAddTags().Count == quantity);
            Assert.IsTrue(processor.OrderDetail.UnknownTags.Count == 0);
            Assert.IsTrue(order.ExpectedSkus.FirstOrDefault().AllocatedTagNumbers.Count == quantity, "Allocated Count == quantity");
            Assert.AreEqual(order, processor.OrderDetail);
            foreach (var number in reservation.TagNumbers)
            {
                var result = processor.Remove(new TestTag(number));
                Assert.IsTrue(result.IsRecognised);
            }

            Assert.IsTrue(processor.GetRemoveTags().Count == quantity);

        }


        [TestMethod]
        public async Task OrderProcessor_OverAllocate()
        {
            var quantity = 3;
            var skus = await _skuRepo.QuerySkus();
            var sku = skus.FirstOrDefault(s => !string.IsNullOrEmpty(s.Gtin));
            var reservation = await _tagReservationRepo.ReserveTagsForSku(sku.Id, quantity + 1);

            var order = new OrderDetailDto();

            order.ExpectedSkus.Add(new OrderSkuLineItemDto
            {
                Name = sku.Name,
                PackingSize = 1,
                Quantity = quantity,
                Gtin = sku.Gtin,
            });
            var processor = new OrderAllocator( order);


            for(var i = 0; i< quantity; i++)
            {
                var number = reservation.TagNumbers[i];
                var result = processor.Add(new TestTag(number));
                Assert.IsTrue(result.IsRecognised, "Tag Recognised");
            }

            var lastNumber = reservation.TagNumbers[quantity];
            var lastResult = processor.Add(new TestTag(lastNumber));
            Assert.IsTrue(lastResult.IsRecognised, "Dispute Required");
            Assert.IsTrue(lastResult.SkuLineItem.AllocatedTagNumbers.Count > lastResult.SkuLineItem.Quantity);

        }
    }
}
