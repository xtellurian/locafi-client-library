using System;
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
    public class OrderReceiveStrategyTests
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
        public async Task Strategy_ReceiveSimple()
        {
            var ran = new Random();
            IProcessSnapshotTagOrderStrategy strategy = new ReceiveStrategy();
            var quantity = ran.Next(1, 10);
            var skus = await _skuRepo.GetAllSkus();
            var sku = skus.FirstOrDefault(s => !string.IsNullOrEmpty(s.Gtin));
            var reservation = await _tagReservationRepo.ReserveTagsForSku(sku.Id, quantity);
            var extraReservation = await _tagReservationRepo.ReserveTagsForSku(sku.Id, 1);
            Assert.IsTrue(reservation.TagNumbers.Count == quantity, "Tag Reservation and quantity are Equal");
            var order = new OrderDetailDto();
            var tagsAllocated = reservation.TagNumbers.Select(tagNumber => new SnapshotTagDto(tagNumber)).ToList(); // add expected tags
            tagsAllocated.Add(new SnapshotTagDto(extraReservation.TagNumbers.FirstOrDefault())); // add extra tag
            StrategyState state = new InitStrategyState(tagsAllocated, null);
            order.RequiredSkus.Add(new OrderSkuLineItemDto
            {
                Name = sku.Name,
                PackingSize = 1,
                Quantity = quantity,
                Gtin = sku.Gtin,
            });
            foreach (var tag in reservation.TagNumbers.Select(tagNumber => new SnapshotTagDto(tagNumber)))
            {
                var result = strategy.ProcessTag(tag, order, state);
                Assert.IsTrue(result.IsTagExpected, "Adding the first time");
                state = result.State;
            }
            var badTag = new SnapshotTagDto("xx");
            var badResult = strategy.ProcessTag(badTag, order, state);
            Assert.IsFalse(badResult.IsTagExpected, "xx tag");
            Assert.AreEqual(ProcessSnapshotTagResultCategory.UnknownTag, badResult.ResultCategory);

            // add all a second tiem
            foreach (var tag in reservation.TagNumbers.Select(tagNumber => new SnapshotTagDto(tagNumber)))
            {
                var result = strategy.ProcessTag(tag, order, state);
                Assert.IsTrue(result.IsTagExpected, "Adding a second time");
                state = result.State;
            }

            var nextResult = strategy.ProcessTag(new SnapshotTagDto(extraReservation.TagNumbers.FirstOrDefault()), order,
                state);
            Assert.IsTrue(nextResult.IsTagExpected, "tag in allocation, but over allocated");
            Assert.AreEqual(ProcessSnapshotTagResultCategory.Ok, nextResult.ResultCategory); // because tag was never allocated
            state = nextResult.State;

            // get one more tag for and add to receive, a tag that was never allocated
            var nextResveration = await _tagReservationRepo.ReserveTagsForSku(sku.Id, 1);
            nextResult = strategy.ProcessTag(new SnapshotTagDto(nextResveration.TagNumbers.FirstOrDefault()), order,
                state);
            Assert.IsFalse(nextResult.IsTagExpected);
            Assert.AreEqual(ProcessSnapshotTagResultCategory.TagNumberMismatch, nextResult.ResultCategory); // because tag was never allocated
            state = nextResult.State;
        }


    }
}
