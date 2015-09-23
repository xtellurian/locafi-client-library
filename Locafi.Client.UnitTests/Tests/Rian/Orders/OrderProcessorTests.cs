using System;
using System.Linq;
using System.Threading.Tasks;
using Locafi.Client.Contract.Processors;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Exceptions;
using Locafi.Client.Model.Dto.Orders;
using Locafi.Client.Model.Dto.Snapshots;
using Locafi.Client.Processors.Orders;
using Locafi.Client.Processors.Orders.Strategies;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Locafi.Client.UnitTests.Tests.Rian.Orders
{
    [TestClass]
    public class OrderProcessorTests
    {
        private IItemRepo _itemRepo;
        private ISkuRepo _skuRepo;
        private ITagReservationRepo _tagReservationRepo;
        private ISnapshotRepo _snapshotRepo;

        [TestInitialize]
        public void Initialise()
        {
            _itemRepo = WebRepoContainer.ItemRepo;
            _skuRepo = WebRepoContainer.SkuRepo;
            _tagReservationRepo = WebRepoContainer.TagReservationRepo;
            _snapshotRepo = WebRepoContainer.SnapshotRepo;
        }
        [TestMethod]
        public async Task OrderProcessor_AllocateExact()
        {
            var ran = new Random();
            IProcessSnapshotTagOrderStrategy allocateStrategy = new AllocateStrategy();
            var quantity = ran.Next(1, 10);
            var skus = await _skuRepo.GetAllSkus();
            var sku = skus.FirstOrDefault(s => !string.IsNullOrEmpty(s.Gtin));
            var reservation = await _tagReservationRepo.ReserveTagsForSku(sku.Id, quantity);

            var order = new OrderDetailDto();
            StrategyState state = new InitStrategyState(null, null);
            order.RequiredSkus.Add(new OrderSkuLineItemDto
            {
                Name = sku.Name,
                PackingSize = 1,
                Quantity = quantity,
                SgtinRef = sku.Gtin,
            });
            var processor = new OrderProcessor(_itemRepo, order, allocateStrategy);

            await processor.InitialiseState(_snapshotRepo);

            foreach (var number in reservation.TagNumbers)
            {
                var snapTag = new SnapshotTagDto(number);
                await processor.AddSnapshotTag(snapTag);
            }

            Assert.IsTrue(processor.Tags.Count == quantity);
            Assert.IsTrue(processor.UnknownItems.Count == 0);
        }

        [ExpectedException(typeof(OrderProcessException))]
        [TestMethod]
        public async Task OrderProcess_OverAllocateWithException()
        {
            IProcessSnapshotTagOrderStrategy allocateStrategy = new AllocateStrategy();
            var quantity = 3;
            var skus = await _skuRepo.GetAllSkus();
            var sku = skus.FirstOrDefault(s => !string.IsNullOrEmpty(s.Gtin));
            var reservation = await _tagReservationRepo.ReserveTagsForSku(sku.Id, quantity + 1);

            var order = new OrderDetailDto();

            order.RequiredSkus.Add(new OrderSkuLineItemDto
            {
                Name = sku.Name,
                PackingSize = 1,
                Quantity = quantity,
                SgtinRef = sku.Gtin,
            });
            var processor = new OrderProcessor(_itemRepo, order, allocateStrategy);

            await processor.InitialiseState(_snapshotRepo);

            foreach (var number in reservation.TagNumbers)
            {
                var snapTag = new SnapshotTagDto(number);
                await processor.AddSnapshotTag(snapTag);
            }

        }
    }
}
