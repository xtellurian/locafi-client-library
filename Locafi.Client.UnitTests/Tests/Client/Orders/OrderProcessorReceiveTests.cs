using System;
using System.Linq;
using System.Threading.Tasks;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Model.Dto.Orders;
using Locafi.Client.Processors.Orders;
using Locafi.Client.UnitTests.Implementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Locafi.Client.UnitTests.Tests.Orders
{
    [TestClass]
    public class OrderProcessorReceiveTests
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
        public async Task OrderProcessor_ReceiveExact()
        {
            var ran = new Random(DateTime.UtcNow.Millisecond);

            var quantity = ran.Next(1, 10);
            var skus = await _skuRepo.QuerySkus();
            var sku = skus.FirstOrDefault(s => !string.IsNullOrEmpty(s.CompanyPrefix) && !string.IsNullOrEmpty(s.ItemReference));
            var reservation = await _tagReservationRepo.ReserveTagsForSku(sku.Id, quantity);

            var order = new OrderDetailDto();

            order.ExpectedSkus.Add(new OrderSkuLineItemDto
            {
                Name = sku.Name,
                PackingSize = 1,
                Quantity = quantity,
                Gtin = "",
            });

            var processor = new OrderReceiver(order);

            foreach (var number in reservation.TagNumbers)
            {
                var result = processor.Add(new TestTag(number));
                Assert.IsTrue(result.IsRecognised, "Tag Recognised");
            }

            Assert.IsTrue(processor.GetAddTags().Count == quantity);
            Assert.IsTrue(processor.OrderDetail.UnknownTags.Count == 0);
            Assert.IsTrue(order.ExpectedSkus.FirstOrDefault().ReceivedTagNumbers.Count == quantity, "Allocated Count == quantity");
            Assert.AreEqual(order, processor.OrderDetail);
        }

      
    }
}
