using System;
using System.Linq;
using System.Threading.Tasks;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Model.Dto.Orders;
using Locafi.Client.Processors.Orders;
using Locafi.Client.UnitTests.Implementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Locafi.Builder;
using Locafi.Client.Model.Enums;
using Locafi.Client.UnitTests.EntityGenerators;
using Locafi.Client.UnitTests.Validators;

namespace Locafi.Client.UnitTests.Tests.Orders
{
    [TestClass]
    public class OrderProcessorReceiveTests
    {
        //private ISkuRepo _skuRepo;
        //private ITagReservationRepo _tagReservationRepo;

        //[TestInitialize]
        //public void Initialise()
        //{

        //    _skuRepo = WebRepoContainer.SkuRepo;
        //    _tagReservationRepo = WebRepoContainer.TagReservationRepo;

        //}

        //[TestMethod]
        //public async Task OrderProcessor_ReceiveExact()
        //{
        //    var ran = new Random(DateTime.UtcNow.Millisecond);

        //    var quantity = ran.Next(1, 10);
        //    var skus = await _skuRepo.QuerySkus();
        //    var sku = skus.FirstOrDefault(s => !string.IsNullOrEmpty(s.CompanyPrefix) && !string.IsNullOrEmpty(s.ItemReference));
        //    var reservation = await _tagReservationRepo.ReserveTagsForSku(sku.Id, quantity);

        //    var order = new OrderDetailDto();

        //    order.ExpectedSkus.Add(new OrderSkuLineItemDto
        //    {
        //        Name = sku.Name,
        //        PackingSize = 1,
        //        Quantity = quantity,
        //        Gtin = "",
        //    });

        //    var processor = new OrderReceiver(order);

        //    foreach (var number in reservation.TagNumbers)
        //    {
        //        var result = processor.Add(new TestTag(number));
        //        Assert.IsTrue(result.IsRecognised, "Tag Recognised");
        //    }

        //    Assert.IsTrue(processor.GetAddTags().Count == quantity);
        //    Assert.IsTrue(processor.OrderDetail.UnknownTags.Count == 0);
        //    Assert.IsTrue(order.ExpectedSkus.FirstOrDefault().ReceivedTagNumbers.Count == quantity, "Allocated Count == quantity");
        //    Assert.AreEqual(order, processor.OrderDetail);
        //}

        [TestMethod]
        public async Task OrderProcessor_ReceiveExact()
        {
            var ran = new Random(DateTime.UtcNow.Millisecond);

            var skusToUse = new List<KeyValuePair<Guid, int>>();
            skusToUse.Add(new KeyValuePair<Guid, int>(WebRepoContainer.Sku1Id, 10));
            skusToUse.Add(new KeyValuePair<Guid, int>(WebRepoContainer.Sku2Id, 5));

            var itemsToUse = new List<KeyValuePair<Guid, string>>();
            itemsToUse.Add(new KeyValuePair<Guid, string>(WebRepoContainer.Asset1Id, DevEnvironment.Asset1TagNumber));
            itemsToUse.Add(new KeyValuePair<Guid, string>(WebRepoContainer.Asset2Id, DevEnvironment.Asset2TagNumber));
            itemsToUse.Add(new KeyValuePair<Guid, string>(WebRepoContainer.Asset3Id, DevEnvironment.Asset3TagNumber));
            itemsToUse.Add(new KeyValuePair<Guid, string>(WebRepoContainer.Asset4Id, DevEnvironment.Asset4TagNumber));

            // create an order so we at least have something to query
            var addOrderDto = OrderGenerator.CreateAddOrderDto(OrderType.Inbound, skusToUse.ToDictionary(k => k.Key, v => v.Value), itemsToUse.Select(i => i.Key).ToList());
            //var orderDetail = await _orderRepo.Create(addOrderDto);
            //_ordersToDelete.AddUnique(orderDetail.Id);

            var order = await OrderGenerator.ToOrderDetailDto(addOrderDto);

            var processor = new OrderReceiver(order);

            // generate a snapshot to allocate the order
            var snapshotDto = await SnapshotGenerator.GenerateSgtinSnapshot(skusToUse.ToDictionary(k => k.Key, v => v.Value), itemsToUse.Select(i => i.Value).ToList());

            foreach (var number in snapshotDto.Tags.Select(t => t.TagNumber))
            {
                var result = processor.Add(new TestTag(number));
                Assert.IsTrue(result.IsRecognised, "Tag Recognised");
            }

            Assert.IsTrue(processor.GetAddTags().Count == snapshotDto.Tags.Count);
            // check each sku line is allocated
            foreach (var skuLine in skusToUse)
                Validator.IsTrue(order.OrderSkuList.FirstOrDefault(s => s.Id == skuLine.Key).ReceivedTagNumbers.Count == skuLine.Value, "Received Count == quantity");
            // check each item line is allocated
            foreach (var itemLine in itemsToUse)
                Validator.IsTrue(order.OrderItemList.FirstOrDefault(s => s.Id == itemLine.Key).IsReceived, "IsReceived == false");
            Validator.AreEqual(order, processor.OrderDetail);
        }


    }
}
