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
using System.Collections.Generic;
using Locafi.Builder;
using Locafi.Client.UnitTests.EntityGenerators;
using Locafi.Client.Model.Enums;
using Locafi.Client.UnitTests.Validators;

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
            var addOrderDto = OrderGenerator.CreateAddOrderDto(OrderType.Outbound, skusToUse.ToDictionary(k => k.Key, v => v.Value), itemsToUse.Select(i => i.Key).ToList());
            //var orderDetail = await _orderRepo.Create(addOrderDto);
            //_ordersToDelete.AddUnique(orderDetail.Id);

            var order = await OrderGenerator.ToOrderDetailDto(addOrderDto);

            var processor = new OrderAllocator(order);

            // generate a snapshot to allocate the order
            var snapshotDto = await SnapshotGenerator.GenerateSgtinSnapshot(skusToUse.ToDictionary(k => k.Key, v => v.Value), itemsToUse.Select(i => i.Value).ToList());

            foreach (var number in snapshotDto.Tags.Select(t => t.TagNumber))
            {
                var result = processor.Add(new TestTag(number));
                Assert.IsTrue(result.IsRecognised, "Tag Recognised");
            }

            Assert.IsTrue(processor.GetAddTags().Count == snapshotDto.Tags.Count);
            // check each sku line is allocated
            foreach(var skuLine in skusToUse)
                Validator.IsTrue(order.OrderSkuList.FirstOrDefault(s => s.Id == skuLine.Key).AllocatedTagNumbers.Count == skuLine.Value, "Allocated Count == quantity");
            // check each item line is allocated
            foreach (var itemLine in itemsToUse)
                Validator.IsNotNull(order.OrderItemList.FirstOrDefault(s => s.Id == itemLine.Key).IsAllocated, "IsAllocated == false");
            Validator.AreEqual(order, processor.OrderDetail);
        }

        //[TestMethod]
        //public async Task OrderProcessor_AllocateDuplicates()
        //{
        //    var quantity = 1;
        //    var skus = await _skuRepo.QuerySkus();
        //    var sku = skus.FirstOrDefault(s => !string.IsNullOrEmpty(s.CompanyPrefix) && !string.IsNullOrEmpty(s.ItemReference));
        //    var reservation = await _tagReservationRepo.ReserveTagsForSku(sku.Id, quantity);


        //    var order = new OrderDetailDto();
        //    var tagNumber = reservation.TagNumbers.FirstOrDefault();

        //    order.ExpectedSkus.Add(new OrderSkuLineItemDto
        //    {
        //        Name = sku.Name,
        //        PackingSize = 1,
        //        Quantity = quantity,
        //        Gtin = ""
        //    });
        //    var processor = new OrderAllocator(order);

        //    for (var i = 0; i < 10; i++)
        //    {
        //        var result = processor.Add(new TestTag(tagNumber));
        //        Assert.IsTrue(result.IsRecognised, "Tag Recognised");
        //    }
        //    Assert.IsTrue(processor.GetAddTags().Count == quantity, "got 1 snapshot tag" );
        //    Assert.IsTrue(order.ExpectedSkus.FirstOrDefault().AllocatedTagNumbers.Count == 1, "Got one tag");

        //    Assert.IsTrue(processor.OrderDetail.UnknownTags.Count == 0, "no Unknown tags");
        //}

        //[TestMethod]
        //public async Task OrderProcessor_RemoveAllocate()
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
        //    var processor = new OrderAllocator(order);

        //    foreach (var number in reservation.TagNumbers)
        //    {
        //        var result = processor.Add(new TestTag(number));
        //        Assert.IsTrue(result.IsRecognised, "Tag Recognised");
        //    }

        //    Assert.IsTrue(processor.GetAddTags().Count == quantity);
        //    Assert.IsTrue(processor.OrderDetail.UnknownTags.Count == 0);
        //    Assert.IsTrue(order.ExpectedSkus.FirstOrDefault().AllocatedTagNumbers.Count == quantity, "Allocated Count == quantity");
        //    Assert.AreEqual(order, processor.OrderDetail);
        //    foreach (var number in reservation.TagNumbers)
        //    {
        //        var result = processor.Remove(new TestTag(number));
        //        Assert.IsTrue(result.IsRecognised);
        //    }

        //    Assert.IsTrue(processor.GetRemoveTags().Count == quantity);

        //}


        //[TestMethod]
        //public async Task OrderProcessor_OverAllocate()
        //{
        //    var quantity = 3;
        //    var skus = await _skuRepo.QuerySkus();
        //    var sku = skus.FirstOrDefault(s => !string.IsNullOrEmpty(s.CompanyPrefix) && !string.IsNullOrEmpty(s.ItemReference));
        //    var reservation = await _tagReservationRepo.ReserveTagsForSku(sku.Id, quantity + 1);

        //    var order = new OrderDetailDto();

        //    order.ExpectedSkus.Add(new OrderSkuLineItemDto
        //    {
        //        Name = sku.Name,
        //        PackingSize = 1,
        //        Quantity = quantity,
        //        Gtin = "",
        //    });
        //    var processor = new OrderAllocator( order);


        //    for(var i = 0; i< quantity; i++)
        //    {
        //        var number = reservation.TagNumbers[i];
        //        var result = processor.Add(new TestTag(number));
        //        Assert.IsTrue(result.IsRecognised, "Tag Recognised");
        //    }

        //    var lastNumber = reservation.TagNumbers[quantity];
        //    var lastResult = processor.Add(new TestTag(lastNumber));
        //    Assert.IsTrue(lastResult.IsRecognised, "Dispute Required");
        //    Assert.IsTrue(lastResult.SkuLineItem.AllocatedTagNumbers.Count > lastResult.SkuLineItem.Quantity);

        //}
    }
}
