using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Locafi.Client.Contract.Processors;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Model.Dto.Orders;
using Locafi.Client.Model.Dto.Snapshots;
using Locafi.Client.Processors.Orders.Strategies;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Locafi.Client.UnitTests.Tests.Rian.Orders
{
    [TestClass]
    public class OrderAllocateStrategyTests
    {
        private ITagReservationRepo _tagReservationRepo;
        private ISkuRepo _skuRepo;

        [TestInitialize]
        public void Initialise()
        {
            _tagReservationRepo = WebRepoContainer.TagReservationRepo;
            _skuRepo = WebRepoContainer.SkuRepo;
        }

        [TestMethod]
        public async Task Strategy_AllocateSimple()
        {
            var ran = new Random();
            IProcessSnapshotTagOrderStrategy strategy = new AllocateStrategy();
            var quantity = ran.Next(1, 10);
            var skus = await _skuRepo.GetAllSkus();
            var sku = skus.FirstOrDefault(s => !string.IsNullOrEmpty(s.Gtin));
            var reservation = await _tagReservationRepo.ReserveTagsForSku(sku.Id, quantity);
            Assert.IsTrue(reservation.TagNumbers.Count == quantity, "Tag Reservation and quantity are Equal");
            var order = new OrderDetailDto();
            StrategyState state = new InitStrategyState(null, null);
            order.RequiredSkus.Add(new OrderSkuLineItemDto
            {
                Name = sku.Name,
                PackingSize = 1,
                Quantity = quantity,
                Gtin = sku.Gtin,
            });
            foreach (var tag in reservation.TagNumbers.Select(tagNumber => new SnapshotTagDto(tagNumber)))
            {
                var result = strategy.ProcessTag(tag, order,state );
                Assert.IsTrue(result.IsTagExpected, "Adding the first time");
                state = result.State;
            }
            var badTag = new SnapshotTagDto("xx");
            var badResult = strategy.ProcessTag(badTag, order, state);
            Assert.IsFalse(badResult.IsTagExpected);
            Assert.AreEqual(badResult.ResultCategory, ProcessSnapshotTagResultCategory.UnknownTag);

            // add all a second tiem
            foreach (var tag in reservation.TagNumbers.Select(tagNumber => new SnapshotTagDto(tagNumber)))
            {
                var result = strategy.ProcessTag(tag, order, state);
                Assert.IsTrue(result.IsTagExpected, "Adding a second time");
                state = result.State;
            }
        }
    }
}
