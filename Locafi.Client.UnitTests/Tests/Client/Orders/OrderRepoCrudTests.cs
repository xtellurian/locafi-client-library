using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Exceptions;
using Locafi.Client.Model.Dto.Orders;
using Locafi.Client.Model.Query;
using Locafi.Client.Model.Query.PropertyComparison;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Locafi.Client.UnitTests.EntityGenerators;
using Locafi.Client.Model.Dto.Snapshots;
using Locafi.Client.Model.Dto.Skus;
using Locafi.Client.Model.Dto.Tags;
using Locafi.Client.Model.Query.Builder;
using Locafi.Client.Model.Dto.Items;
using Locafi.Client.Model.Dto.Places;
using Locafi.Client.Model;
using Locafi.Client.UnitTests.Validators;
using Locafi.Client.Model.Enums;
using Locafi.Client.UnitTests.Extensions;
using Locafi.Builder;

namespace Locafi.Client.UnitTests.Tests
{
    [TestClass]
    public class OrderRepoCrudTests
    {
        private IPlaceRepo _placeRepo;
        private IPersonRepo _personRepo;
        private IOrderRepo _orderRepo;
        private IUserRepo _userRepo;
        private ISkuRepo _skuRepo;
        private ISnapshotRepo _snapshotRepo;
        private IItemRepo _itemRepo;
        private List<Guid> _ordersToDelete;
        private IList<string> _tagNumbersToDelete;

        [TestInitialize]
        public void Initialize()
        {
            _placeRepo = WebRepoContainer.PlaceRepo;
            _personRepo = WebRepoContainer.PersonRepo;
            _orderRepo = WebRepoContainer.OrderRepo;
            _userRepo = WebRepoContainer.UserRepo;
            _skuRepo = WebRepoContainer.SkuRepo;
            _snapshotRepo = WebRepoContainer.SnapshotRepo;
            _itemRepo = WebRepoContainer.ItemRepo;
            _ordersToDelete = new List<Guid>();
            _tagNumbersToDelete = new List<string>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            // delete all orders that were created
            foreach (var id in _ordersToDelete)
            {
                try {
                    _orderRepo.Cancel(id).Wait();
                }
                catch(Exception e)
                {
                    try
                    {
                        _orderRepo.Receive(id).Wait();
                        _orderRepo.Complete(id).Wait();
                    }
                    catch { }
                }
            }

            // delete items created from tags
            if (_tagNumbersToDelete.Count > 0)
            {
                var query = QueryBuilder<ItemSummaryDto>.NewQuery(i => i.TagNumber, string.Join(",", _tagNumbersToDelete), ComparisonOperator.ContainedIn).Build();
                var itemQuery = _itemRepo.QueryItems(query).Result;
                foreach (var item in itemQuery.Items)
                {
                    if (item != null)
                    {
                        _itemRepo.DeleteItem(item.Id).Wait();
                    }
                }
            }
        }

        [TestMethod]
        public async Task Order_Create_Inbound()
        {
            var addOrderDto = await CreateRandomAddOrderDto(OrderType.Inbound);

            var result = await _orderRepo.Create(addOrderDto);
            _ordersToDelete.AddUnique(result.Id);

            OrderDtoValidator.OrderDetailcheck(result, OrderStateType.Created);
            OrderDtoValidator.OrderCreateComparison(result, addOrderDto);
        }

        [TestMethod]
        public async Task Order_Create_Outbound()
        {
            var addOrderDto = await CreateRandomAddOrderDto(OrderType.Outbound);

            var result = await _orderRepo.Create(addOrderDto);
            _ordersToDelete.AddUnique(result.Id);

            OrderDtoValidator.OrderDetailcheck(result, OrderStateType.Created);
            OrderDtoValidator.OrderCreateComparison(result, addOrderDto);
        }

        [TestMethod]
        public async Task Order_Create_Transfer()
        {
            var addOrderDto = await CreateRandomAddOrderDto(OrderType.Transfer);

            var result = await _orderRepo.Create(addOrderDto);
            _ordersToDelete.AddUnique(result.Id);

            OrderDtoValidator.OrderDetailcheck(result, OrderStateType.Created);
            OrderDtoValidator.OrderCreateComparison(result, addOrderDto);
        }

        [TestMethod]
        public async Task Order_GetAllOrders()
        {
            // create and order so we at least have something to query
            var addOrderDto = await CreateRandomAddOrderDto(OrderType.Transfer);

            var result = await _orderRepo.Create(addOrderDto);
            _ordersToDelete.AddUnique(result.Id);

            var orders = await _orderRepo.QueryOrders();
            Validator.IsNotNull(orders, "OrderRepo.GetAllOrders returned Null");
            Validator.IsInstanceOfType(orders, typeof(PageResult<OrderSummaryDto>));
            Validator.IsTrue(orders.Items.Count() > 0);
        }

        [TestMethod]
        public async Task Order_GetOrderById()
        {
            // create and order so we at least have something to query
            var addOrderDto = await CreateRandomAddOrderDto(OrderType.Transfer);

            var result = await _orderRepo.Create(addOrderDto);
            _ordersToDelete.AddUnique(result.Id);

            // now get the order
            var getOrder = await _orderRepo.GetOrderById(result.Id);

            OrderDtoValidator.OrderDetailcheck(getOrder);
            OrderDtoValidator.OrderDetailComparison(result, getOrder);
        }

        [TestMethod]
        public async Task Order_QueryOrders()
        {
            // create and order so we at least have something to query
            var addOrderDto = await CreateRandomAddOrderDto(OrderType.Transfer);

            var order = await _orderRepo.Create(addOrderDto);
            _ordersToDelete.AddUnique(order.Id);

            // query for order with matching to place id
            var query = QueryBuilder<OrderSummaryDto>.NewQuery(o => o.ToPlaceId, order.ToPlaceId,
                ComparisonOperator.Equals)
                .Build();

            var queryResult = await _orderRepo.QueryOrders(query);
            Assert.IsNotNull(queryResult, "queryResult != null");
            Assert.IsTrue(queryResult.Items.Contains(order));

            // query for order with matching state
            query = QueryBuilder<OrderSummaryDto>.NewQuery(o => o.OrderState, order.OrderState,
                ComparisonOperator.Equals)
                .Build();

            queryResult = await _orderRepo.QueryOrders(query);
            Assert.IsNotNull(queryResult, "queryResult != null");
            Assert.IsTrue(queryResult.Items.Contains(order));
        }

        [TestMethod]
        public async Task Order_Inbound_CompleteProcess_SkuOnly()
        {
            var skusToUse = new List<KeyValuePair<Guid, int>>();
            skusToUse.Add(new KeyValuePair<Guid, int>(WebRepoContainer.Sku1Id, 10));
            skusToUse.Add(new KeyValuePair<Guid, int>(WebRepoContainer.Sku2Id, 5));

            var itemsToUse = new List<KeyValuePair<Guid, string>>();
            //itemsToUse.Add(new KeyValuePair<Guid, string>(WebRepoContainer.Asset1Id, DevEnvironment.Asset1TagNumber));
            //itemsToUse.Add(new KeyValuePair<Guid, string>(WebRepoContainer.Asset2Id, DevEnvironment.Asset2TagNumber));
            //itemsToUse.Add(new KeyValuePair<Guid, string>(WebRepoContainer.Asset3Id, DevEnvironment.Asset3TagNumber));
            //itemsToUse.Add(new KeyValuePair<Guid, string>(WebRepoContainer.Asset4Id, DevEnvironment.Asset4TagNumber));

            // create and order so we at least have something to query
            var addOrderDto = OrderGenerator.CreateAddOrderDto(OrderType.Inbound, skusToUse.ToDictionary(k => k.Key, v => v.Value), itemsToUse.Select(i => i.Key).ToList());

            var orderDetail = await _orderRepo.Create(addOrderDto);
            _ordersToDelete.AddUnique(orderDetail.Id);

            // check response
            OrderDtoValidator.OrderDetailcheck(orderDetail, OrderStateType.Created);
            OrderDtoValidator.OrderCreateComparison(orderDetail, addOrderDto);

            // now test the receive process for the inbound order
            await TestInboundOrderProces(orderDetail, skusToUse, itemsToUse);
        }

        [TestMethod]
        public async Task Order_Inbound_CompleteProcess_UniqueOnly()
        {
            var skusToUse = new List<KeyValuePair<Guid, int>>();
//            skusToUse.Add(new KeyValuePair<Guid, int>(WebRepoContainer.Sku1Id, 10));
//            skusToUse.Add(new KeyValuePair<Guid, int>(WebRepoContainer.Sku2Id, 5));

            var itemsToUse = new List<KeyValuePair<Guid, string>>();
            itemsToUse.Add(new KeyValuePair<Guid, string>(WebRepoContainer.Asset1Id, DevEnvironment.Asset1TagNumber));
            itemsToUse.Add(new KeyValuePair<Guid, string>(WebRepoContainer.Asset2Id, DevEnvironment.Asset2TagNumber));
            itemsToUse.Add(new KeyValuePair<Guid, string>(WebRepoContainer.Asset3Id, DevEnvironment.Asset3TagNumber));
            itemsToUse.Add(new KeyValuePair<Guid, string>(WebRepoContainer.Asset4Id, DevEnvironment.Asset4TagNumber));

            // make sure the items are not already in the to location
            await PrepareItemsForTest(itemsToUse, WebRepoContainer.Place1Id);

            // create and order so we at least have something to query
            var addOrderDto = OrderGenerator.CreateAddOrderDto(OrderType.Inbound, skusToUse.ToDictionary(k => k.Key, v => v.Value), itemsToUse.Select(i => i.Key).ToList());

            var orderDetail = await _orderRepo.Create(addOrderDto);
            _ordersToDelete.AddUnique(orderDetail.Id);

            // check response
            OrderDtoValidator.OrderDetailcheck(orderDetail, OrderStateType.Created);
            OrderDtoValidator.OrderCreateComparison(orderDetail, addOrderDto);

            // now test the receive process for the inbound order
            await TestInboundOrderProces(orderDetail, skusToUse, itemsToUse);
        }

        [TestMethod]
        public async Task Order_Inbound_CompleteProcess_SkuAndUnique()
        {
            var skusToUse = new List<KeyValuePair<Guid, int>>();
            skusToUse.Add(new KeyValuePair<Guid, int>(WebRepoContainer.Sku1Id, 10));
            skusToUse.Add(new KeyValuePair<Guid, int>(WebRepoContainer.Sku2Id, 5));

            var itemsToUse = new List<KeyValuePair<Guid, string>>();
            itemsToUse.Add(new KeyValuePair<Guid, string>(WebRepoContainer.Asset1Id, DevEnvironment.Asset1TagNumber));
            itemsToUse.Add(new KeyValuePair<Guid, string>(WebRepoContainer.Asset2Id, DevEnvironment.Asset2TagNumber));
            itemsToUse.Add(new KeyValuePair<Guid, string>(WebRepoContainer.Asset3Id, DevEnvironment.Asset3TagNumber));
            itemsToUse.Add(new KeyValuePair<Guid, string>(WebRepoContainer.Asset4Id, DevEnvironment.Asset4TagNumber));

            // make sure the items are not already in the to location
            await PrepareItemsForTest(itemsToUse, WebRepoContainer.Place1Id);

            // create and order so we at least have something to query
            var addOrderDto = OrderGenerator.CreateAddOrderDto(OrderType.Inbound, skusToUse.ToDictionary(k => k.Key, v => v.Value), itemsToUse.Select(i => i.Key).ToList());

            var orderDetail = await _orderRepo.Create(addOrderDto);
            _ordersToDelete.AddUnique(orderDetail.Id);

            // check response
            OrderDtoValidator.OrderDetailcheck(orderDetail, OrderStateType.Created);
            OrderDtoValidator.OrderCreateComparison(orderDetail, addOrderDto);

            // now test the receive process for the inbound order
            await TestInboundOrderProces(orderDetail, skusToUse, itemsToUse);
        }

        [TestMethod]
        public async Task Order_Outbound_CompleteProcess_SkuOnly()
        {
            var skusToUse = new List<KeyValuePair<Guid, int>>();
            skusToUse.Add(new KeyValuePair<Guid, int>(WebRepoContainer.Sku1Id, 10));
            skusToUse.Add(new KeyValuePair<Guid, int>(WebRepoContainer.Sku2Id, 5));

            var itemsToUse = new List<KeyValuePair<Guid, string>>();
            //itemsToUse.Add(new KeyValuePair<Guid, string>(WebRepoContainer.Asset1Id, DevEnvironment.Asset1TagNumber));
            //itemsToUse.Add(new KeyValuePair<Guid, string>(WebRepoContainer.Asset2Id, DevEnvironment.Asset2TagNumber));
            //itemsToUse.Add(new KeyValuePair<Guid, string>(WebRepoContainer.Asset3Id, DevEnvironment.Asset3TagNumber));
            //itemsToUse.Add(new KeyValuePair<Guid, string>(WebRepoContainer.Asset4Id, DevEnvironment.Asset4TagNumber));

            // create an order so we at least have something to query
            var addOrderDto = OrderGenerator.CreateAddOrderDto(OrderType.Outbound, skusToUse.ToDictionary(k => k.Key, v => v.Value), itemsToUse.Select(i => i.Key).ToList());

            var orderDetail = await _orderRepo.Create(addOrderDto);
            _ordersToDelete.AddUnique(orderDetail.Id);

            // check response
            OrderDtoValidator.OrderDetailcheck(orderDetail, OrderStateType.Created);
            OrderDtoValidator.OrderCreateComparison(orderDetail, addOrderDto);

            // now test the allocate/dispatch process for the outbound order
            await TestOutboundOrderProces(orderDetail, skusToUse, itemsToUse);
        }

        [TestMethod]
        public async Task Order_Outbound_CompleteProcess_UniqueOnly()
        {
            var skusToUse = new List<KeyValuePair<Guid, int>>();
            //skusToUse.Add(new KeyValuePair<Guid, int>(WebRepoContainer.Sku1Id, 10));
            //skusToUse.Add(new KeyValuePair<Guid, int>(WebRepoContainer.Sku2Id, 5));

            var itemsToUse = new List<KeyValuePair<Guid, string>>();
            itemsToUse.Add(new KeyValuePair<Guid, string>(WebRepoContainer.Asset1Id, DevEnvironment.Asset1TagNumber));
            itemsToUse.Add(new KeyValuePair<Guid, string>(WebRepoContainer.Asset2Id, DevEnvironment.Asset2TagNumber));
            itemsToUse.Add(new KeyValuePair<Guid, string>(WebRepoContainer.Asset3Id, DevEnvironment.Asset3TagNumber));
            itemsToUse.Add(new KeyValuePair<Guid, string>(WebRepoContainer.Asset4Id, DevEnvironment.Asset4TagNumber));

            // make sure the items are not already in the to location
            await PrepareItemsForTest(itemsToUse, WebRepoContainer.Place2Id);   // hack to cleanup states for testing
            // make sure the items are not already in the to location
            await PrepareItemsForTest(itemsToUse, WebRepoContainer.Place1Id);

            // create an order so we at least have something to query
            var addOrderDto = OrderGenerator.CreateAddOrderDto(OrderType.Outbound, skusToUse.ToDictionary(k => k.Key, v => v.Value), itemsToUse.Select(i => i.Key).ToList());

            var orderDetail = await _orderRepo.Create(addOrderDto);
            _ordersToDelete.AddUnique(orderDetail.Id);

            // check response
            OrderDtoValidator.OrderDetailcheck(orderDetail, OrderStateType.Created);
            OrderDtoValidator.OrderCreateComparison(orderDetail, addOrderDto);

            // now test the allocate/dispatch process for the outbound order
            await TestOutboundOrderProces(orderDetail, skusToUse, itemsToUse);
        }

        [TestMethod]
        public async Task Order_Outbound_CompleteProcess_SkuAndUnique()
        {
            var skusToUse = new List<KeyValuePair<Guid, int>>();
            skusToUse.Add(new KeyValuePair<Guid, int>(WebRepoContainer.Sku1Id, 10));
            skusToUse.Add(new KeyValuePair<Guid, int>(WebRepoContainer.Sku2Id, 5));

            var itemsToUse = new List<KeyValuePair<Guid, string>>();
            itemsToUse.Add(new KeyValuePair<Guid, string>(WebRepoContainer.Asset1Id, DevEnvironment.Asset1TagNumber));
            itemsToUse.Add(new KeyValuePair<Guid, string>(WebRepoContainer.Asset2Id, DevEnvironment.Asset2TagNumber));
            itemsToUse.Add(new KeyValuePair<Guid, string>(WebRepoContainer.Asset3Id, DevEnvironment.Asset3TagNumber));
            itemsToUse.Add(new KeyValuePair<Guid, string>(WebRepoContainer.Asset4Id, DevEnvironment.Asset4TagNumber));

            // make sure the items are not already in the to location
            await PrepareItemsForTest(itemsToUse, WebRepoContainer.Place2Id);   // hack to cleanup states for testing
            // make sure the items are not already in the to location
            await PrepareItemsForTest(itemsToUse, WebRepoContainer.Place1Id);

            // create an order so we at least have something to query
            var addOrderDto = OrderGenerator.CreateAddOrderDto(OrderType.Outbound, skusToUse.ToDictionary(k => k.Key, v => v.Value), itemsToUse.Select(i => i.Key).ToList());

            var orderDetail = await _orderRepo.Create(addOrderDto);
            _ordersToDelete.AddUnique(orderDetail.Id);

            // check response
            OrderDtoValidator.OrderDetailcheck(orderDetail, OrderStateType.Created);
            OrderDtoValidator.OrderCreateComparison(orderDetail, addOrderDto);

            // now test the allocate/dispatch process for the outbound order
            await TestOutboundOrderProces(orderDetail, skusToUse, itemsToUse);
        }

        [TestMethod]
        public async Task Order_Transfer_CompleteProcess_SkuOnly()
        {
            var skusToUse = new List<KeyValuePair<Guid, int>>();
            skusToUse.Add(new KeyValuePair<Guid, int>(WebRepoContainer.Sku1Id, 10));
            skusToUse.Add(new KeyValuePair<Guid, int>(WebRepoContainer.Sku2Id, 5));

            var itemsToUse = new List<KeyValuePair<Guid, string>>();
            //itemsToUse.Add(new KeyValuePair<Guid, string>(WebRepoContainer.Asset1Id, DevEnvironment.Asset1TagNumber));
            //itemsToUse.Add(new KeyValuePair<Guid, string>(WebRepoContainer.Asset2Id, DevEnvironment.Asset2TagNumber));
            //itemsToUse.Add(new KeyValuePair<Guid, string>(WebRepoContainer.Asset3Id, DevEnvironment.Asset3TagNumber));
            //itemsToUse.Add(new KeyValuePair<Guid, string>(WebRepoContainer.Asset4Id, DevEnvironment.Asset4TagNumber));

            // create an order so we at least have something to query
            var addOrderDto = OrderGenerator.CreateAddOrderDto(OrderType.Transfer, skusToUse.ToDictionary(k => k.Key, v => v.Value), itemsToUse.Select(i => i.Key).ToList());

            var orderDetail = await _orderRepo.Create(addOrderDto);
            _ordersToDelete.AddUnique(orderDetail.Id);

            // check response
            OrderDtoValidator.OrderDetailcheck(orderDetail, OrderStateType.Created);
            OrderDtoValidator.OrderCreateComparison(orderDetail, addOrderDto);

            // now test the allocate/dispatch process for the outbound portion of the transfer order
            await TestOutboundOrderProces(orderDetail, skusToUse, itemsToUse);

            // now test the receive process for the inbound portion of the transfer order
            await TestInboundOrderProces(orderDetail, skusToUse, itemsToUse);
        }
        ////      [TestMethod]
        //      public async Task OrderCrud_DeleteOrder()
        //      {
        //          // from create success above
        //          var ran = new Random(DateTime.UtcNow.Millisecond);
        //          var places = await GetAvailablePlaces();
        //          var sourcePlace = places.Items.ElementAt(ran.Next(places.Items.Count())); // get random places
        //          var destinationPlace = places.Items.ElementAt(ran.Next(places.Items.Count()));

        //          var persons = await _personRepo.QueryPersons();
        //          var person = persons.Items.FirstOrDefault();

        //          var refNumber = Guid.NewGuid().ToString();
        //          var description = Guid.NewGuid().ToString();
        //          var skuLineItems = await GenerateSomeSkuLineItems();
        //          var itemLineItems = await GenerateSomeItemLineItems();
        //          var addOrder = new AddOrderDto(refNumber, description, sourcePlace.Id,
        //              destinationPlace.Id, skuLineItems, itemLineItems, person.Id);

        //          var order = await _orderRepo.Create(addOrder);
        //          // use above for deletion
        //          Assert.IsNotNull(order);
        //          var success = await _orderRepo.DeleteOrder(order.Id);
        //          Assert.IsTrue(success);
        //          var allOrders = await _orderRepo.QueryOrders();
        //          Assert.IsFalse(allOrders.Contains(order));
        //      }

        //      [TestMethod]
        //      public async Task Order_AllocateOverSkuQuantity()
        //      {
        //          // create the order
        //          var ran = new Random(DateTime.UtcNow.Millisecond);
        //          var places = await GetAvailablePlaces();
        //          var sourcePlace = places.Items.ElementAt(ran.Next(places.Items.Count())); // get random places
        //          var destinationPlace = places.Items.ElementAt(ran.Next(places.Items.Count()));

        //          var persons = await _personRepo.QueryPersons();
        //          var person = persons.Items.FirstOrDefault();

        //          int NumberOfSkuLines = 2;
        //          int QtyPerSkuLine = 10;
        //          int NumberOfItemLines = 2;
        //          var refNumber = Guid.NewGuid().ToString();
        //          var description = Guid.NewGuid().ToString();
        //          var skuLineItems = await GenerateSomeSkuLineItems(NumberOfSkuLines, QtyPerSkuLine);
        //          var itemLineItems = await GenerateSomeItemLineItems(NumberOfItemLines);
        //          var addOrder = new AddOrderDto(refNumber, description, sourcePlace.Id,
        //              destinationPlace.Id, skuLineItems, itemLineItems, person.Id);

        //          var result = await _orderRepo.Create(addOrder);

        //          Assert.IsNotNull(result);
        //          Assert.AreEqual(result.DestinationPlaceId, addOrder.DestinationPlaceId);
        //          Assert.AreEqual(result.SourcePlaceId, addOrder.SourcePlaceId);
        //          Assert.AreEqual(result.DeliverToId, addOrder.DeliverToId);

        //          // create snapshots to fully allocate the order (one for each line item as that is easiest)
        //          var addSsDtos = new List<AddSnapshotDto>();
        //          // create sku line snapshots
        //          foreach (var skuLine in result.ExpectedSkus)
        //          {
        //              addSsDtos.Add(await SnapshotGenerator.CreateExistingGtinSnapshotForUpload(result.SourcePlaceId,skuLine.Quantity,-1,skuLine.SkuId));
        //          }

        //          // create unique item snapshot
        //          if (result.ExpectedItems.Count > 0)
        //              addSsDtos.Add(SnapshotGenerator.CreateSnapshotForUpload(result.SourcePlaceId,result.ExpectedItems.Select(i => i.TagNumber).ToList()));

        //          var t1 = DateTime.UtcNow;
        //          var snapshotDtos = new List<SnapshotDetailDto>();
        //          foreach (var addSsDto in addSsDtos)
        //          {
        //              snapshotDtos.Add(await _snapshotRepo.CreateSnapshot(addSsDto));
        //          }

        //          // add snapshots to the order
        //          OrderActionResponseDto allocateResult = null;
        //          foreach(var ssDto in snapshotDtos)
        //          {
        //              allocateResult = await _orderRepo.Allocate(result, ssDto.Id);
        //          }

        //          Assert.IsNotNull(allocateResult);
        //          Assert.IsTrue(allocateResult.Success);
        //          Assert.AreEqual(allocateResult.OrderDetail.ExpectedSkus.Count, NumberOfSkuLines);   // check that we've still got all the sku lines we should have
        //          Assert.AreEqual(allocateResult.OrderDetail.ExpectedItems.Count, NumberOfItemLines); // check that we've still got all the item lines we should have
        //          foreach (var expectedItem in allocateResult.OrderDetail.ExpectedItems)  // check that all our items are allocated
        //              Assert.IsTrue(expectedItem.IsAllocated);
        //          foreach (var expectedSku in allocateResult.OrderDetail.ExpectedSkus)    // check that all our sku's are allocated
        //              Assert.AreEqual(expectedSku.Quantity, expectedSku.AllocatedTagNumbers.Count);
        //          Assert.AreEqual(allocateResult.OrderDetail.AdditionalItems.Count, 0);   // check that we ahve no additional items
        //          Assert.AreEqual(allocateResult.OrderDetail.AdditionalSkus.Count, 0);    // check that we have no additional sku's
        //          var t2 = DateTime.UtcNow;
        //          var span = t2 - t1;

        //          // now over allocate some sku's
        //          int skuOverAllocateAmount = 1;
        //          // create snapshots for over allocating and removing the over allocating
        //          var skuOverAllocate_AddSnapshotDto = await SnapshotGenerator.CreateNewGtinSnapshotForUpload(result.SourcePlaceId, skuOverAllocateAmount, -1, result.ExpectedSkus[0].SkuId);
        //          var addSkuOverAllocateSnapshot = await _snapshotRepo.CreateSnapshot(skuOverAllocate_AddSnapshotDto);
        //          skuOverAllocate_AddSnapshotDto.SnapshotType = Model.Enums.SnapshotType.Remove;
        //          var removeSkuOverAllocateSnapshot = await _snapshotRepo.CreateSnapshot(skuOverAllocate_AddSnapshotDto);

        //          // add the over allocate snapshot
        //          allocateResult = await _orderRepo.Allocate(result, addSkuOverAllocateSnapshot.Id);

        //          // check that we're over
        //          Assert.IsNotNull(allocateResult);
        //          Assert.IsTrue(allocateResult.Success);
        //          Assert.AreEqual(allocateResult.OrderDetail.ExpectedSkus.Count, NumberOfSkuLines);   // check that we've still got all the sku lines we should have
        //          Assert.AreEqual(allocateResult.OrderDetail.ExpectedItems.Count, NumberOfItemLines); // check that we've still got all the item lines we should have
        //          foreach (var expectedItem in allocateResult.OrderDetail.ExpectedItems)  // check that all our items are allocated
        //              Assert.IsTrue(expectedItem.IsAllocated);
        //          foreach (var expectedSku in allocateResult.OrderDetail.ExpectedSkus)    // check that all our sku's are allocated
        //          {
        //              if(expectedSku.SkuId == result.ExpectedSkus[0].SkuId)
        //                  Assert.AreEqual(expectedSku.Quantity + skuOverAllocateAmount, expectedSku.AllocatedTagNumbers.Count);
        //              else
        //                  Assert.AreEqual(expectedSku.Quantity, expectedSku.AllocatedTagNumbers.Count);
        //          }
        //          Assert.AreEqual(allocateResult.OrderDetail.AdditionalItems.Count, 0);   // check that we ahve no additional items
        //          Assert.AreEqual(allocateResult.OrderDetail.AdditionalSkus.Count, 0);    // check that we have no additional sku's

        //          // add the remove over allocate snapshot
        //          allocateResult = await _orderRepo.Allocate(result, removeSkuOverAllocateSnapshot.Id);

        //          // check that we're not over anymore
        //          Assert.IsNotNull(allocateResult);
        //          Assert.IsTrue(allocateResult.Success);
        //          Assert.AreEqual(allocateResult.OrderDetail.ExpectedSkus.Count, NumberOfSkuLines);   // check that we've still got all the sku lines we should have
        //          Assert.AreEqual(allocateResult.OrderDetail.ExpectedItems.Count, NumberOfItemLines); // check that we've still got all the item lines we should have
        //          foreach (var expectedItem in allocateResult.OrderDetail.ExpectedItems)  // check that all our items are allocated
        //              Assert.IsTrue(expectedItem.IsAllocated);
        //          foreach (var expectedSku in allocateResult.OrderDetail.ExpectedSkus)    // check that all our sku's are allocated
        //              Assert.AreEqual(expectedSku.Quantity, expectedSku.AllocatedTagNumbers.Count);
        //          Assert.AreEqual(allocateResult.OrderDetail.AdditionalItems.Count, 0);   // check that we ahve no additional items
        //          Assert.AreEqual(allocateResult.OrderDetail.AdditionalSkus.Count, 0);    // check that we have no additional sku's
        //      }

        //      [TestMethod]
        //      public async Task Order_AllocateAdditionalItems()
        //      {
        //          // create the order
        //          var ran = new Random(DateTime.UtcNow.Millisecond);
        //          var places = await GetAvailablePlaces();
        //          var sourcePlace = places.Items.ElementAt(ran.Next(places.Items.Count())); // get random places
        //          var destinationPlace = places.Items.ElementAt(ran.Next(places.Items.Count()));

        //          var persons = await _personRepo.QueryPersons();
        //          var person = persons.Items.FirstOrDefault();

        //          int NumberOfSkuLines = 2;
        //          int QtyPerSkuLine = 10;
        //          int NumberOfItemLines = 2;
        //          var refNumber = Guid.NewGuid().ToString();
        //          var description = Guid.NewGuid().ToString();
        //          var skuLineItems = await GenerateSomeSkuLineItems(NumberOfSkuLines, QtyPerSkuLine);
        //          var itemLineItems = await GenerateSomeItemLineItems(NumberOfItemLines);
        //          var addOrder = new AddOrderDto(refNumber, description, sourcePlace.Id,
        //              destinationPlace.Id, skuLineItems, itemLineItems, person.Id);

        //          var result = await _orderRepo.Create(addOrder);

        //          Assert.IsNotNull(result);
        //          Assert.AreEqual(result.DestinationPlaceId, addOrder.DestinationPlaceId);
        //          Assert.AreEqual(result.SourcePlaceId, addOrder.SourcePlaceId);
        //          Assert.AreEqual(result.DeliverToId, addOrder.DeliverToId);

        //          // create snapshots to fully allocate the order (one for each line item as that is easiest)
        //          var addSsDtos = new List<AddSnapshotDto>();
        //          // create sku line snapshots
        //          foreach (var skuLine in result.ExpectedSkus)
        //          {
        //              addSsDtos.Add(await SnapshotGenerator.CreateExistingGtinSnapshotForUpload(result.SourcePlaceId, skuLine.Quantity, -1, skuLine.SkuId));
        //          }

        //          // create unique item snapshot
        //          if (result.ExpectedItems.Count > 0)
        //              addSsDtos.Add(SnapshotGenerator.CreateSnapshotForUpload(result.SourcePlaceId, result.ExpectedItems.Select(i => i.TagNumber).ToList()));

        //          var t1 = DateTime.UtcNow;
        //          var snapshotDtos = new List<SnapshotDetailDto>();
        //          foreach (var addSsDto in addSsDtos)
        //          {
        //              snapshotDtos.Add(await _snapshotRepo.CreateSnapshot(addSsDto));
        //          }

        //          // add snapshots to the order
        //          OrderActionResponseDto allocateResult = null;
        //          foreach (var ssDto in snapshotDtos)
        //          {
        //              allocateResult = await _orderRepo.Allocate(result, ssDto.Id);
        //          }

        //          Assert.IsNotNull(allocateResult);
        //          Assert.IsTrue(allocateResult.Success);
        //          Assert.AreEqual(allocateResult.OrderDetail.ExpectedSkus.Count, NumberOfSkuLines);   // check that we've still got all the sku lines we should have
        //          Assert.AreEqual(allocateResult.OrderDetail.ExpectedItems.Count, NumberOfItemLines); // check that we've still got all the item lines we should have
        //          foreach (var expectedItem in allocateResult.OrderDetail.ExpectedItems)  // check that all our items are allocated
        //              Assert.IsTrue(expectedItem.IsAllocated);
        //          foreach (var expectedSku in allocateResult.OrderDetail.ExpectedSkus)    // check that all our sku's are allocated
        //              Assert.AreEqual(expectedSku.Quantity, expectedSku.AllocatedTagNumbers.Count);
        //          Assert.AreEqual(allocateResult.OrderDetail.AdditionalItems.Count, 0);   // check that we ahve no additional items
        //          Assert.AreEqual(allocateResult.OrderDetail.AdditionalSkus.Count, 0);    // check that we have no additional sku's
        //          var t2 = DateTime.UtcNow;
        //          var span = t2 - t1;

        //          // now over allocate some items (additional items)
        //          int itemOverAllocateAmount = NumberOfItemLines + 1;
        //          // create snapshots for over allocating and removing the over allocating
        //          var items = (await GenerateSomeItemLineItems(itemOverAllocateAmount)).Where(i => !result.ExpectedItems.Select(e => e.ItemId).ToList().Contains(i.ItemId)).Select(i => i.ItemId).ToList();
        //          var itemOverAllocate_AddSnapshotDto = await SnapshotGenerator.CreateExistingItemSnapshotForUpload(result.SourcePlaceId, items);
        //          var itemAddOverAllocateSnapshot = await _snapshotRepo.CreateSnapshot(itemOverAllocate_AddSnapshotDto);
        //          itemOverAllocate_AddSnapshotDto.SnapshotType = Model.Enums.SnapshotType.Remove;
        //          var itemRemoveOverAllocateSnapshot = await _snapshotRepo.CreateSnapshot(itemOverAllocate_AddSnapshotDto);

        //          // add the over allocate snapshot
        //          allocateResult = await _orderRepo.Allocate(result, itemAddOverAllocateSnapshot.Id);

        //          // check that we're over
        //          Assert.IsNotNull(allocateResult);
        //          Assert.IsTrue(allocateResult.Success);
        //          Assert.AreEqual(allocateResult.OrderDetail.ExpectedSkus.Count, NumberOfSkuLines);   // check that we've still got all the sku lines we should have
        //          Assert.AreEqual(allocateResult.OrderDetail.ExpectedItems.Count, NumberOfItemLines); // check that we've still got all the item lines we should have
        //          foreach (var expectedItem in allocateResult.OrderDetail.ExpectedItems)  // check that all our items are allocated
        //              Assert.IsTrue(expectedItem.IsAllocated);
        //          foreach (var expectedSku in allocateResult.OrderDetail.ExpectedSkus)    // check that all our sku's are allocated
        //              Assert.AreEqual(expectedSku.Quantity, expectedSku.AllocatedTagNumbers.Count);
        //          Assert.AreEqual(allocateResult.OrderDetail.AdditionalItems.Count, items.Count); // check that we have the required additional items
        //          foreach (var additionalItem in allocateResult.OrderDetail.AdditionalItems)  // check that all our additional items are allocated
        //              Assert.IsTrue(additionalItem.IsAllocated);
        //          Assert.AreEqual(allocateResult.OrderDetail.AdditionalSkus.Count, 0);    // check that we have no additional sku's

        //          // add the remove over allocate snapshot
        //          allocateResult = await _orderRepo.Allocate(result, itemRemoveOverAllocateSnapshot.Id);

        //          // check that we're not over anymore
        //          Assert.IsNotNull(allocateResult);
        //          Assert.IsTrue(allocateResult.Success);
        //          Assert.AreEqual(allocateResult.OrderDetail.ExpectedSkus.Count, NumberOfSkuLines);   // check that we've still got all the sku lines we should have
        //          Assert.AreEqual(allocateResult.OrderDetail.ExpectedItems.Count, NumberOfItemLines); // check that we've still got all the item lines we should have
        //          foreach (var expectedItem in allocateResult.OrderDetail.ExpectedItems)  // check that all our items are allocated
        //              Assert.IsTrue(expectedItem.IsAllocated);
        //          foreach (var expectedSku in allocateResult.OrderDetail.ExpectedSkus)    // check that all our sku's are allocated
        //              Assert.AreEqual(expectedSku.Quantity, expectedSku.AllocatedTagNumbers.Count);
        //          Assert.AreEqual(allocateResult.OrderDetail.AdditionalItems.Count, 0); // check that our additional items are gone
        //          Assert.AreEqual(allocateResult.OrderDetail.AdditionalSkus.Count, 0);    // check that we have no additional sku's
        //      }

        //      [TestMethod]
        //      public async Task Order_AllocateWithAdditionalSkuLineItems()
        //      {
        //          // create the order
        //          var ran = new Random(DateTime.UtcNow.Millisecond);
        //          var places = await GetAvailablePlaces();
        //          var sourcePlace = places.Items.ElementAt(ran.Next(places.Items.Count())); // get random places
        //          var destinationPlace = places.Items.ElementAt(ran.Next(places.Items.Count()));

        //          var persons = await _personRepo.QueryPersons();
        //          var person = persons.Items.FirstOrDefault();

        //          int NumberOfSkuLines = 2;
        //          int QtyPerSkuLine = 10;
        //          int NumberOfItemLines = 2;

        //          var refNumber = Guid.NewGuid().ToString();
        //          var description = Guid.NewGuid().ToString();
        //          var skuLineItems = await GenerateSomeSkuLineItems(NumberOfSkuLines, QtyPerSkuLine);
        //          var itemLineItems = await GenerateSomeItemLineItems(NumberOfItemLines);
        //          var addOrder = new AddOrderDto(refNumber, description, sourcePlace.Id,
        //              destinationPlace.Id, skuLineItems, itemLineItems, person.Id);

        //          var result = await _orderRepo.Create(addOrder);

        //          Assert.IsNotNull(result);
        //          Assert.AreEqual(result.DestinationPlaceId, addOrder.DestinationPlaceId);
        //          Assert.AreEqual(result.SourcePlaceId, addOrder.SourcePlaceId);
        //          Assert.AreEqual(result.DeliverToId, addOrder.DeliverToId);

        //          // create snapshots to fully allocate the order (one for each line item as that is easiest)
        //          var addSsDtos = new List<AddSnapshotDto>();
        //          // create sku line AddSnapshotDto's
        //          foreach (var skuLine in result.ExpectedSkus)
        //          {
        //              addSsDtos.Add(await SnapshotGenerator.CreateExistingGtinSnapshotForUpload(result.SourcePlaceId, skuLine.Quantity, -1, skuLine.SkuId));
        //          }

        //          // create unique item AddSnapshotDto
        //          if(result.ExpectedItems.Count > 0)
        //              addSsDtos.Add(SnapshotGenerator.CreateSnapshotForUpload(result.SourcePlaceId, result.ExpectedItems.Select(i => i.TagNumber).ToList()));

        //          // create the snapshots
        //          var t1 = DateTime.UtcNow;
        //          var snapshotDtos = new List<SnapshotDetailDto>();
        //          foreach (var addSsDto in addSsDtos)
        //          {
        //              snapshotDtos.Add(await _snapshotRepo.CreateSnapshot(addSsDto));
        //          }

        //          // add snapshots to the order
        //          OrderActionResponseDto allocateResult = null;
        //          foreach (var ssDto in snapshotDtos)
        //          {
        //              allocateResult = await _orderRepo.Allocate(result, ssDto.Id);
        //          }

        //          Assert.IsNotNull(allocateResult);
        //          Assert.IsTrue(allocateResult.Success);
        //          Assert.AreEqual(allocateResult.OrderDetail.ExpectedSkus.Count, NumberOfSkuLines);   // check that we've still got all the sku lines we should have
        //          Assert.AreEqual(allocateResult.OrderDetail.ExpectedItems.Count, NumberOfItemLines); // check that we've still got all the item lines we should have
        //          foreach (var expectedItem in allocateResult.OrderDetail.ExpectedItems)  // check that all our items are allocated
        //              Assert.IsTrue(expectedItem.IsAllocated);
        //          foreach (var expectedSku in allocateResult.OrderDetail.ExpectedSkus)    // check that all our sku's are allocated
        //              Assert.AreEqual(expectedSku.Quantity, expectedSku.AllocatedTagNumbers.Count);
        //          Assert.AreEqual(allocateResult.OrderDetail.AdditionalItems.Count, 0);   // check that we ahve no additional items
        //          Assert.AreEqual(allocateResult.OrderDetail.AdditionalSkus.Count, 0);    // check that we have no additional sku's
        //          foreach (var additionalItem in allocateResult.OrderDetail.AdditionalItems)  // check that all our additional items are allocated
        //              Assert.IsTrue(additionalItem.IsAllocated);
        //          foreach (var additionalSku in allocateResult.OrderDetail.AdditionalSkus)    // check that all our additional sku's are allocated
        //              Assert.AreEqual(QtyPerSkuLine, additionalSku.AllocatedTagNumbers.Count);

        //          // create a AddSnapshotDto for an additional sku line
        //          var skus = (await _skuRepo.QuerySkus()).Where(s => !string.IsNullOrEmpty(s.CompanyPrefix) && !string.IsNullOrEmpty(s.ItemReference) && !result.ExpectedSkus.Select(o => o.SkuId).ToList().Contains(s.Id)).ToList(); // get a different sku
        //          var additionalSku_AddSsDto = await SnapshotGenerator.CreateExistingGtinSnapshotForUpload(result.SourcePlaceId, QtyPerSkuLine, -1, skus[ran.Next(skus.Count - 1)].Id);
        //          var addAdditionalSs = await _snapshotRepo.CreateSnapshot(additionalSku_AddSsDto);
        //          additionalSku_AddSsDto.SnapshotType = Model.Enums.SnapshotType.Remove;
        //          var removeAdditionalSs = await _snapshotRepo.CreateSnapshot(additionalSku_AddSsDto);

        //          // now allocate the additional skus
        //          allocateResult = await _orderRepo.Allocate(result, addAdditionalSs.Id);

        //          // check they're allocated
        //          Assert.IsNotNull(allocateResult);
        //          Assert.IsTrue(allocateResult.Success);
        //          Assert.AreEqual(allocateResult.OrderDetail.ExpectedSkus.Count, NumberOfSkuLines);   // check that we've still got all the sku lines we should have
        //          Assert.AreEqual(allocateResult.OrderDetail.ExpectedItems.Count, NumberOfItemLines); // check that we've still got all the item lines we should have
        //          foreach (var expectedItem in allocateResult.OrderDetail.ExpectedItems)  // check that all our items are allocated
        //              Assert.IsTrue(expectedItem.IsAllocated);
        //          foreach (var expectedSku in allocateResult.OrderDetail.ExpectedSkus)    // check that all our sku's are allocated
        //              Assert.AreEqual(expectedSku.Quantity, expectedSku.AllocatedTagNumbers.Count);
        //          Assert.AreEqual(allocateResult.OrderDetail.AdditionalItems.Count, 0);   // check that we ahve no additional items
        //          Assert.AreEqual(allocateResult.OrderDetail.AdditionalSkus.Count, 1);    // check that we have no additional sku's
        //          foreach (var additionalItem in allocateResult.OrderDetail.AdditionalItems)  // check that all our additional items are allocated
        //              Assert.IsTrue(additionalItem.IsAllocated);
        //          foreach (var additionalSku in allocateResult.OrderDetail.AdditionalSkus)    // check that all our additional sku's are allocated
        //              Assert.AreEqual(QtyPerSkuLine, additionalSku.AllocatedTagNumbers.Count);

        //          // now remove the additional sku's
        //          allocateResult = await _orderRepo.Allocate(result, removeAdditionalSs.Id);

        //          // checked they're removed
        //          Assert.IsNotNull(allocateResult);
        //          Assert.IsTrue(allocateResult.Success);
        //          Assert.AreEqual(allocateResult.OrderDetail.ExpectedSkus.Count, NumberOfSkuLines);   // check that we've still got all the sku lines we should have
        //          Assert.AreEqual(allocateResult.OrderDetail.ExpectedItems.Count, NumberOfItemLines); // check that we've still got all the item lines we should have
        //          foreach (var expectedItem in allocateResult.OrderDetail.ExpectedItems)  // check that all our items are allocated
        //              Assert.IsTrue(expectedItem.IsAllocated);
        //          foreach (var expectedSku in allocateResult.OrderDetail.ExpectedSkus)    // check that all our sku's are allocated
        //              Assert.AreEqual(expectedSku.Quantity, expectedSku.AllocatedTagNumbers.Count);
        //          Assert.AreEqual(allocateResult.OrderDetail.AdditionalItems.Count, 0);   // check that we ahve no additional items
        //          Assert.AreEqual(allocateResult.OrderDetail.AdditionalSkus.Count, 0);    // check that we have no additional sku's
        //          foreach (var additionalItem in allocateResult.OrderDetail.AdditionalItems)  // check that all our additional items are allocated
        //              Assert.IsTrue(additionalItem.IsAllocated);
        //          foreach (var additionalSku in allocateResult.OrderDetail.AdditionalSkus)    // check that all our additional sku's are allocated
        //              Assert.AreEqual(QtyPerSkuLine, additionalSku.AllocatedTagNumbers.Count);

        //          // create the additional snapshot
        //          var t2 = DateTime.UtcNow;
        //          var span = t2 - t1;
        //      }

        //      [TestMethod]
        //      public async Task Order_AllocateWithSkuItemsAsUniqueItems()
        //      {
        //          IItemRepo _itemRepo = WebRepoContainer.ItemRepo;

        //          // create the order
        //          var ran = new Random(DateTime.UtcNow.Millisecond);
        //          var places = await GetAvailablePlaces();
        //          var sourcePlace = places.Items.ElementAt(ran.Next(places.Items.Count())); // get random places
        //          var destinationPlace = places.Items.ElementAt(ran.Next(places.Items.Count()));

        //          var persons = await _personRepo.QueryPersons();
        //          var person = persons.Items.FirstOrDefault();

        //          int NumberOfSkuLines = 2;
        //          int QtyPerSkuLine = 10;
        //          int NumberOfItemLines = 2;
        //          var refNumber = Guid.NewGuid().ToString();
        //          var description = Guid.NewGuid().ToString();
        //          var skuLineItems = await GenerateSomeSkuLineItems(NumberOfSkuLines, QtyPerSkuLine);
        //          var itemLineItems = await GenerateSomeItemLineItems(NumberOfItemLines);

        //          // create some item line items with sku items
        //          var skus = (await _skuRepo.QuerySkus()).Where(s => !string.IsNullOrEmpty(s.CompanyPrefix) && !string.IsNullOrEmpty(s.ItemReference) && !skuLineItems.Select(i => i.SkuId).ToList().Contains(s.Id)).ToList();
        //          var sku = skus[ran.Next(skus.Count)];

        //          var q0 = QueryBuilder<ItemSummaryDto>.NewQuery(i => i.SkuId, sku.Id, ComparisonOperator.Equals).Take(0).Build();
        //          var itemCount = (await _itemRepo.QueryItems(q0)).Count;

        //          for (int count = 0; count < NumberOfItemLines; count++)
        //          {

        //              var q1 = QueryBuilder<ItemSummaryDto>.NewQuery(i => i.SkuId, sku.Id, ComparisonOperator.Equals)
        //                  .And(i => i.TagNumber, null, ComparisonOperator.NotEquals)
        //                  .And(i => i.TagNumber, "", ComparisonOperator.NotEquals)
        //                  .Take(1)
        //                  .Skip(ran.Next((int)itemCount))
        //                  .Build();
        //              var item = await _itemRepo.QueryItemsContinuation(q1);
        //              if (item.Entities.Count > 0 && !itemLineItems.Any(i => i.ItemId == item.Entities.First().Id))
        //              {
        //                  itemLineItems.Add(new AddOrderItemLineItemDto()
        //                  {
        //                      ItemId = item.Entities.First().Id
        //                  });
        //              }
        //              else
        //              {
        //                  count--;
        //                  // change sku because this one has no items
        //                  sku = skus[ran.Next(skus.Count)];
        //                  // get new count
        //                  q0 = QueryBuilder<ItemSummaryDto>.NewQuery(i => i.SkuId, sku.Id, ComparisonOperator.Equals).Take(0).Build();
        //                  itemCount = (await _itemRepo.QueryItems(q0)).Count;
        //              }
        //          }

        //          var addOrder = new AddOrderDto(refNumber, description, sourcePlace.Id,
        //              destinationPlace.Id, skuLineItems, itemLineItems, person.Id);

        //          var result = await _orderRepo.Create(addOrder);

        //          Assert.IsNotNull(result);
        //          Assert.AreEqual(result.DestinationPlaceId, addOrder.DestinationPlaceId);
        //          Assert.AreEqual(result.SourcePlaceId, addOrder.SourcePlaceId);
        //          Assert.AreEqual(result.DeliverToId, addOrder.DeliverToId);

        //          // create snapshots to fully allocate the order (one for each line item as that is easiest)
        //          var addSsDtos = new List<AddSnapshotDto>();
        //          // create sku line snapshots
        //          foreach (var skuLine in result.ExpectedSkus)
        //          {
        //              addSsDtos.Add(await SnapshotGenerator.CreateExistingGtinSnapshotForUpload(result.SourcePlaceId, skuLine.Quantity, -1, skuLine.SkuId));
        //          }

        //          // create unique item snapshot
        //          if (result.ExpectedItems.Count > 0)
        //              addSsDtos.Add(SnapshotGenerator.CreateSnapshotForUpload(result.SourcePlaceId, result.ExpectedItems.Select(i => i.TagNumber).ToList()));

        //          var t1 = DateTime.UtcNow;
        //          var snapshotDtos = new List<SnapshotDetailDto>();
        //          foreach (var addSsDto in addSsDtos)
        //          {
        //              snapshotDtos.Add(await _snapshotRepo.CreateSnapshot(addSsDto));
        //          }

        //          // add snapshots to the order
        //          OrderActionResponseDto allocateResult = null;
        //          foreach (var ssDto in snapshotDtos)
        //          {
        //              allocateResult = await _orderRepo.Allocate(result, ssDto.Id);
        //          }

        //          Assert.IsNotNull(allocateResult);
        //          Assert.IsTrue(allocateResult.Success);
        //          Assert.AreEqual(allocateResult.OrderDetail.ExpectedSkus.Count, NumberOfSkuLines);   // check that we've still got all the sku lines we should have
        //          Assert.AreEqual(allocateResult.OrderDetail.ExpectedItems.Count, NumberOfItemLines * 2); // check that we've still got all the item lines we should have
        //          foreach (var expectedItem in allocateResult.OrderDetail.ExpectedItems)  // check that all our items are allocated
        //              Assert.IsTrue(expectedItem.IsAllocated);
        //          foreach (var expectedSku in allocateResult.OrderDetail.ExpectedSkus)    // check that all our sku's are allocated
        //              Assert.AreEqual(expectedSku.Quantity, expectedSku.AllocatedTagNumbers.Count);
        //          Assert.AreEqual(allocateResult.OrderDetail.AdditionalItems.Count, 0);   // check that we ahve no additional items
        //          Assert.AreEqual(allocateResult.OrderDetail.AdditionalSkus.Count, 0);    // check that we have no additional sku's
        //          var t2 = DateTime.UtcNow;
        //          var span = t2 - t1;

        //          // now over allocate some items (additional items)
        //          int itemOverAllocateAmount = NumberOfItemLines + 1;
        //          // create snapshots for over allocating and removing the over allocating
        //          var items = (await GenerateSomeItemLineItems(itemOverAllocateAmount)).Where(i => !result.ExpectedItems.Select(e => e.ItemId).ToList().Contains(i.ItemId)).Select(i => i.ItemId).ToList();
        //          var itemOverAllocate_AddSnapshotDto = await SnapshotGenerator.CreateExistingItemSnapshotForUpload(result.SourcePlaceId, items);
        //          var itemAddOverAllocateSnapshot = await _snapshotRepo.CreateSnapshot(itemOverAllocate_AddSnapshotDto);
        //          itemOverAllocate_AddSnapshotDto.SnapshotType = Model.Enums.SnapshotType.Remove;
        //          var itemRemoveOverAllocateSnapshot = await _snapshotRepo.CreateSnapshot(itemOverAllocate_AddSnapshotDto);

        //          // add the over allocate snapshot
        //          allocateResult = await _orderRepo.Allocate(result, itemAddOverAllocateSnapshot.Id);

        //          // check that we're over
        //          Assert.IsNotNull(allocateResult);
        //          Assert.IsTrue(allocateResult.Success);
        //          Assert.AreEqual(allocateResult.OrderDetail.ExpectedSkus.Count, NumberOfSkuLines);   // check that we've still got all the sku lines we should have
        //          Assert.AreEqual(allocateResult.OrderDetail.ExpectedItems.Count, NumberOfItemLines * 2); // check that we've still got all the item lines we should have
        //          foreach (var expectedItem in allocateResult.OrderDetail.ExpectedItems)  // check that all our items are allocated
        //              Assert.IsTrue(expectedItem.IsAllocated);
        //          foreach (var expectedSku in allocateResult.OrderDetail.ExpectedSkus)    // check that all our sku's are allocated
        //              Assert.AreEqual(expectedSku.Quantity, expectedSku.AllocatedTagNumbers.Count);
        //          Assert.AreEqual(allocateResult.OrderDetail.AdditionalItems.Count, items.Count); // check that we have the required additional items
        //          foreach (var additionalItem in allocateResult.OrderDetail.AdditionalItems)  // check that all our additional items are allocated
        //              Assert.IsTrue(additionalItem.IsAllocated);
        //          Assert.AreEqual(allocateResult.OrderDetail.AdditionalSkus.Count, 0);    // check that we have no additional sku's

        //          // add the remove over allocate snapshot
        //          allocateResult = await _orderRepo.Allocate(result, itemRemoveOverAllocateSnapshot.Id);

        //          // check that we're not over anymore
        //          Assert.IsNotNull(allocateResult);
        //          Assert.IsTrue(allocateResult.Success);
        //          Assert.AreEqual(allocateResult.OrderDetail.ExpectedSkus.Count, NumberOfSkuLines);   // check that we've still got all the sku lines we should have
        //          Assert.AreEqual(allocateResult.OrderDetail.ExpectedItems.Count, NumberOfItemLines * 2); // check that we've still got all the item lines we should have
        //          foreach (var expectedItem in allocateResult.OrderDetail.ExpectedItems)  // check that all our items are allocated
        //              Assert.IsTrue(expectedItem.IsAllocated);
        //          foreach (var expectedSku in allocateResult.OrderDetail.ExpectedSkus)    // check that all our sku's are allocated
        //              Assert.AreEqual(expectedSku.Quantity, expectedSku.AllocatedTagNumbers.Count);
        //          Assert.AreEqual(allocateResult.OrderDetail.AdditionalItems.Count, 0); // check that our additional items are gone
        //          Assert.AreEqual(allocateResult.OrderDetail.AdditionalSkus.Count, 0);    // check that we have no additional sku's
        //      }

        //      private async Task<PageResult<PlaceSummaryDto>> GetAvailablePlaces()
        //      {
        //          var placeQuery = QueryBuilder<PlaceSummaryDto>.NewQuery(p => p.Name, "N/A", ComparisonOperator.NotEquals)
        //              .And(p => p.Name, "New Tags", ComparisonOperator.NotEquals)
        //              .And(p => p.Name, "In-Transit", ComparisonOperator.NotEquals)
        //              .Build();
        //          var places = await _placeRepo.QueryPlaces(placeQuery);
        //          return places;
        //      }

        //      [TestMethod]
        //      public async Task Order_ReceiveOverSkuQuantity()
        //      {
        //          // create the order
        //          var ran = new Random(DateTime.UtcNow.Millisecond);
        //          var places = await GetAvailablePlaces();
        //          var sourcePlace = places.Items.ElementAt(ran.Next(places.Items.Count())); // get random places
        //          var destinationPlace = places.Items.ElementAt(ran.Next(places.Items.Count()));

        //          var persons = await _personRepo.QueryPersons();
        //          var person = persons.Items.FirstOrDefault();

        //          int NumberOfSkuLines = 2;
        //          int QtyPerSkuLine = 10;
        //          int NumberOfItemLines = 2;
        //          var refNumber = Guid.NewGuid().ToString();
        //          var description = Guid.NewGuid().ToString();
        //          var skuLineItems = await GenerateSomeSkuLineItems(NumberOfSkuLines, QtyPerSkuLine);
        //          var itemLineItems = await GenerateSomeItemLineItems(NumberOfItemLines);
        //          var addOrder = new AddOrderDto(refNumber, description, sourcePlace.Id,
        //              destinationPlace.Id, skuLineItems, itemLineItems, person.Id);

        //          var result = await _orderRepo.Create(addOrder);

        //          Assert.IsNotNull(result);
        //          Assert.AreEqual(result.DestinationPlaceId, addOrder.DestinationPlaceId);
        //          Assert.AreEqual(result.SourcePlaceId, addOrder.SourcePlaceId);
        //          Assert.AreEqual(result.DeliverToId, addOrder.DeliverToId);

        //          // create snapshots to fully allocate the order (one for each line item as that is easiest)
        //          var addSsDtos = new List<AddSnapshotDto>();
        //          // create sku line snapshots
        //          foreach (var skuLine in result.ExpectedSkus)
        //          {
        //              addSsDtos.Add(await SnapshotGenerator.CreateExistingGtinSnapshotForUpload(result.DestinationPlaceId, skuLine.Quantity, -1, skuLine.SkuId));
        //          }

        //          // create unique item snapshot
        //          if (result.ExpectedItems.Count > 0)
        //              addSsDtos.Add(SnapshotGenerator.CreateSnapshotForUpload(result.DestinationPlaceId, result.ExpectedItems.Select(i => i.TagNumber).ToList()));

        //          var t1 = DateTime.UtcNow;
        //          var snapshotDtos = new List<SnapshotDetailDto>();
        //          foreach (var addSsDto in addSsDtos)
        //          {
        //              snapshotDtos.Add(await _snapshotRepo.CreateSnapshot(addSsDto));
        //          }

        //          // must dispatch the order before it can be received
        //          var response = await _orderRepo.Dispatch(result);

        //          // add snapshots to the order
        //          OrderActionResponseDto receiveResult = null;
        //          foreach (var ssDto in snapshotDtos)
        //          {
        //              receiveResult = await _orderRepo.Receive(result, ssDto.Id);
        //          }

        //          Assert.IsNotNull(receiveResult);
        //          Assert.IsTrue(receiveResult.Success);
        //          Assert.AreEqual(receiveResult.OrderDetail.ExpectedSkus.Count, NumberOfSkuLines);   // check that we've still got all the sku lines we should have
        //          Assert.AreEqual(receiveResult.OrderDetail.ExpectedItems.Count, NumberOfItemLines); // check that we've still got all the item lines we should have
        //          foreach (var expectedItem in receiveResult.OrderDetail.ExpectedItems)  // check that all our items are allocated
        //              Assert.IsTrue(expectedItem.IsReceived);
        //          foreach (var expectedSku in receiveResult.OrderDetail.ExpectedSkus)    // check that all our sku's are allocated
        //              Assert.AreEqual(expectedSku.Quantity, expectedSku.ReceivedTagNumbers.Count);
        //          Assert.AreEqual(receiveResult.OrderDetail.AdditionalItems.Count, 0);   // check that we ahve no additional items
        //          Assert.AreEqual(receiveResult.OrderDetail.AdditionalSkus.Count, 0);    // check that we have no additional sku's
        //          var t2 = DateTime.UtcNow;
        //          var span = t2 - t1;

        //          // now over Receive some sku's
        //          int skuOverReceiveAmount = 1;
        //          // create snapshots for over allocating and removing the over allocating
        //          var skuOverReceive_AddSnapshotDto = await SnapshotGenerator.CreateNewGtinSnapshotForUpload(result.DestinationPlaceId, skuOverReceiveAmount, -1, result.ExpectedSkus[0].SkuId);
        //          var addSkuOverReceiveSnapshot = await _snapshotRepo.CreateSnapshot(skuOverReceive_AddSnapshotDto);
        //          skuOverReceive_AddSnapshotDto.SnapshotType = Model.Enums.SnapshotType.Remove;
        //          var removeSkuOverReceiveSnapshot = await _snapshotRepo.CreateSnapshot(skuOverReceive_AddSnapshotDto);

        //          // add the over Receive snapshot
        //          receiveResult = await _orderRepo.Receive(result, addSkuOverReceiveSnapshot.Id);

        //          // check that we're over
        //          Assert.IsNotNull(receiveResult);
        //          Assert.IsTrue(receiveResult.Success);
        //          Assert.AreEqual(receiveResult.OrderDetail.ExpectedSkus.Count, NumberOfSkuLines);   // check that we've still got all the sku lines we should have
        //          Assert.AreEqual(receiveResult.OrderDetail.ExpectedItems.Count, NumberOfItemLines); // check that we've still got all the item lines we should have
        //          foreach (var expectedItem in receiveResult.OrderDetail.ExpectedItems)  // check that all our items are allocated
        //              Assert.IsTrue(expectedItem.IsReceived);
        //          foreach (var expectedSku in receiveResult.OrderDetail.ExpectedSkus)    // check that all our sku's are allocated
        //          {
        //              if (expectedSku.SkuId == result.ExpectedSkus[0].SkuId)
        //                  Assert.AreEqual(expectedSku.Quantity + skuOverReceiveAmount, expectedSku.ReceivedTagNumbers.Count);
        //              else
        //                  Assert.AreEqual(expectedSku.Quantity, expectedSku.ReceivedTagNumbers.Count);
        //          }
        //          Assert.AreEqual(receiveResult.OrderDetail.AdditionalItems.Count, 0);   // check that we ahve no additional items
        //          Assert.AreEqual(receiveResult.OrderDetail.AdditionalSkus.Count, 0);    // check that we have no additional sku's

        //          // add the remove over Receive snapshot
        //          receiveResult = await _orderRepo.Receive(result, removeSkuOverReceiveSnapshot.Id);

        //          // check that we're not over anymore
        //          Assert.IsNotNull(receiveResult);
        //          Assert.IsTrue(receiveResult.Success);
        //          Assert.AreEqual(receiveResult.OrderDetail.ExpectedSkus.Count, NumberOfSkuLines);   // check that we've still got all the sku lines we should have
        //          Assert.AreEqual(receiveResult.OrderDetail.ExpectedItems.Count, NumberOfItemLines); // check that we've still got all the item lines we should have
        //          foreach (var expectedItem in receiveResult.OrderDetail.ExpectedItems)  // check that all our items are allocated
        //              Assert.IsTrue(expectedItem.IsReceived);
        //          foreach (var expectedSku in receiveResult.OrderDetail.ExpectedSkus)    // check that all our sku's are allocated
        //              Assert.AreEqual(expectedSku.Quantity, expectedSku.ReceivedTagNumbers.Count);
        //          Assert.AreEqual(receiveResult.OrderDetail.AdditionalItems.Count, 0);   // check that we ahve no additional items
        //          Assert.AreEqual(receiveResult.OrderDetail.AdditionalSkus.Count, 0);    // check that we have no additional sku's
        //      }

        //      [TestMethod]
        //      public async Task Order_CompleteOrderCycleExactQuantities()
        //      {
        //          // create the order
        //          var ran = new Random(DateTime.UtcNow.Millisecond);
        //          var places = await GetAvailablePlaces();
        //          var sourcePlace = places.Items.ElementAt(ran.Next(places.Items.Count())); // get random places
        //          var destinationPlace = places.Items.ElementAt(ran.Next(places.Items.Count()));

        //          var persons = await _personRepo.QueryPersons();
        //          var person = persons.Items.FirstOrDefault();

        //          int NumberOfSkuLines = 2;
        //          int QtyPerSkuLine = 10;
        //          int NumberOfItemLines = 2;
        //          var refNumber = Guid.NewGuid().ToString();
        //          var description = Guid.NewGuid().ToString();
        //          var skuLineItems = await GenerateSomeSkuLineItems(NumberOfSkuLines, QtyPerSkuLine);
        //          var itemLineItems = await GenerateSomeItemLineItems(NumberOfItemLines);
        //          var addOrder = new AddOrderDto(refNumber, description, sourcePlace.Id,
        //              destinationPlace.Id, skuLineItems, itemLineItems, person.Id);

        //          var result = await _orderRepo.Create(addOrder);

        //          Assert.IsNotNull(result);
        //          Assert.AreEqual(result.DestinationPlaceId, addOrder.DestinationPlaceId);
        //          Assert.AreEqual(result.SourcePlaceId, addOrder.SourcePlaceId);
        //          Assert.AreEqual(result.DeliverToId, addOrder.DeliverToId);

        //          // create snapshots to fully allocate the order (one for each line item as that is easiest)
        //          var addSsDtos = new List<AddSnapshotDto>();
        //          // create sku line AddSnapshotDto's
        //          foreach (var skuLine in result.ExpectedSkus)
        //          {
        //              addSsDtos.Add(await SnapshotGenerator.CreateExistingGtinSnapshotForUpload(result.SourcePlaceId, skuLine.Quantity, -1, skuLine.SkuId));
        //          }

        //          // create unique item AddSnapshotDto
        //          if (result.ExpectedItems.Count > 0)
        //              addSsDtos.Add(SnapshotGenerator.CreateSnapshotForUpload(result.SourcePlaceId, result.ExpectedItems.Select(i => i.TagNumber).ToList()));

        //          // create the snapshots
        //          var snapshotDtos = new List<SnapshotDetailDto>();
        //          foreach (var addSsDto in addSsDtos)
        //          {
        //              snapshotDtos.Add(await _snapshotRepo.CreateSnapshot(addSsDto));
        //          }

        //          // add snapshots to the order
        //          OrderActionResponseDto allocateResult = null;
        //          foreach (var ssDto in snapshotDtos)
        //          {
        //              allocateResult = await _orderRepo.Allocate(result, ssDto.Id);
        //          }

        //          Assert.IsNotNull(allocateResult);
        //          Assert.IsTrue(allocateResult.Success);
        //          Assert.AreEqual(allocateResult.OrderDetail.State, Model.Enums.OrderState.Allocatable);
        //          Assert.AreEqual(allocateResult.OrderDetail.ExpectedSkus.Count, NumberOfSkuLines);   // check that we've still got all the sku lines we should have
        //          Assert.AreEqual(allocateResult.OrderDetail.ExpectedItems.Count, NumberOfItemLines); // check that we've still got all the item lines we should have
        //          foreach (var expectedItem in allocateResult.OrderDetail.ExpectedItems)  // check that all our items are allocated
        //              Assert.IsTrue(expectedItem.IsAllocated);
        //          foreach (var expectedSku in allocateResult.OrderDetail.ExpectedSkus)    // check that all our sku's are allocated
        //              Assert.AreEqual(expectedSku.Quantity, expectedSku.AllocatedTagNumbers.Count);
        //          Assert.AreEqual(allocateResult.OrderDetail.AdditionalItems.Count, 0);   // check that we ahve no additional items
        //          Assert.AreEqual(allocateResult.OrderDetail.AdditionalSkus.Count, 0);    // check that we have no additional sku's
        //          foreach (var additionalItem in allocateResult.OrderDetail.AdditionalItems)  // check that all our additional items are allocated
        //              Assert.IsTrue(additionalItem.IsAllocated);
        //          foreach (var additionalSku in allocateResult.OrderDetail.AdditionalSkus)    // check that all our additional sku's are allocated
        //              Assert.AreEqual(QtyPerSkuLine, additionalSku.AllocatedTagNumbers.Count);

        //          // Now dispatch the order
        //          OrderActionResponseDto dispatchResult =  await _orderRepo.Dispatch(result);

        //          Assert.IsNotNull(dispatchResult);
        //          Assert.IsTrue(dispatchResult.Success);
        //          Assert.AreEqual(dispatchResult.OrderDetail.State, Model.Enums.OrderState.Receivable);
        //          Assert.AreEqual(dispatchResult.OrderDetail.ExpectedSkus.Count, NumberOfSkuLines);   // check that we've still got all the sku lines we should have
        //          Assert.AreEqual(dispatchResult.OrderDetail.ExpectedItems.Count, NumberOfItemLines); // check that we've still got all the item lines we should have
        //          foreach (var expectedItem in dispatchResult.OrderDetail.ExpectedItems)  // check that all our items are allocated
        //              Assert.IsTrue(expectedItem.IsAllocated);
        //          foreach (var expectedSku in dispatchResult.OrderDetail.ExpectedSkus)    // check that all our sku's are allocated
        //              Assert.AreEqual(expectedSku.Quantity, expectedSku.AllocatedTagNumbers.Count);
        //          Assert.AreEqual(dispatchResult.OrderDetail.AdditionalItems.Count, 0);   // check that we ahve no additional items
        //          Assert.AreEqual(dispatchResult.OrderDetail.AdditionalSkus.Count, 0);    // check that we have no additional sku's
        //          foreach (var additionalItem in dispatchResult.OrderDetail.AdditionalItems)  // check that all our additional items are allocated
        //              Assert.IsTrue(additionalItem.IsAllocated);
        //          foreach (var additionalSku in dispatchResult.OrderDetail.AdditionalSkus)    // check that all our additional sku's are allocated
        //              Assert.AreEqual(QtyPerSkuLine, additionalSku.AllocatedTagNumbers.Count);

        //          // create snapshots to fully receive the order (one for each line item as that is easiest)
        //          var addReceiveSsDtos = new List<AddSnapshotDto>();
        //          // create sku line snapshots
        //          foreach (var skuLine in result.ExpectedSkus)
        //          {
        //              addReceiveSsDtos.Add(await SnapshotGenerator.CreateExistingGtinSnapshotForUpload(result.DestinationPlaceId, skuLine.Quantity, -1, skuLine.SkuId));
        //          }

        //          // create unique item snapshot
        //          if (result.ExpectedItems.Count > 0)
        //              addReceiveSsDtos.Add(SnapshotGenerator.CreateSnapshotForUpload(result.DestinationPlaceId, result.ExpectedItems.Select(i => i.TagNumber).ToList()));

        //          var receiveSnapshotDtos = new List<SnapshotDetailDto>();
        //          foreach (var addSsDto in addReceiveSsDtos)
        //          {
        //              receiveSnapshotDtos.Add(await _snapshotRepo.CreateSnapshot(addSsDto));
        //          }

        //          // add snapshots to the order
        //          OrderActionResponseDto receiveResult = null;
        //          foreach (var ssDto in receiveSnapshotDtos)
        //          {
        //              receiveResult = await _orderRepo.Receive(result, ssDto.Id);
        //          }

        //          Assert.IsNotNull(receiveResult);
        //          Assert.IsTrue(receiveResult.Success);
        //          Assert.AreEqual(receiveResult.OrderDetail.ExpectedSkus.Count, NumberOfSkuLines);   // check that we've still got all the sku lines we should have
        //          Assert.AreEqual(receiveResult.OrderDetail.ExpectedItems.Count, NumberOfItemLines); // check that we've still got all the item lines we should have
        //          foreach (var expectedItem in receiveResult.OrderDetail.ExpectedItems)  // check that all our items are allocated
        //              Assert.IsTrue(expectedItem.IsReceived);
        //          foreach (var expectedSku in receiveResult.OrderDetail.ExpectedSkus)    // check that all our sku's are allocated
        //              Assert.AreEqual(expectedSku.Quantity, expectedSku.ReceivedTagNumbers.Count);
        //          Assert.AreEqual(receiveResult.OrderDetail.AdditionalItems.Count, 0);   // check that we ahve no additional items
        //          Assert.AreEqual(receiveResult.OrderDetail.AdditionalSkus.Count, 0);    // check that we have no additional sku's

        //          // now over Receive some sku's
        //          int skuOverReceiveAmount = 1;
        //          // create snapshots for over allocating and removing the over allocating
        //          var skuOverReceive_AddSnapshotDto = await SnapshotGenerator.CreateNewGtinSnapshotForUpload(result.DestinationPlaceId, skuOverReceiveAmount, -1, result.ExpectedSkus[0].SkuId);
        //          var addSkuOverReceiveSnapshot = await _snapshotRepo.CreateSnapshot(skuOverReceive_AddSnapshotDto);
        //          skuOverReceive_AddSnapshotDto.SnapshotType = Model.Enums.SnapshotType.Remove;
        //          var removeSkuOverReceiveSnapshot = await _snapshotRepo.CreateSnapshot(skuOverReceive_AddSnapshotDto);

        //          // add the over Receive snapshot
        //          receiveResult = await _orderRepo.Receive(result, addSkuOverReceiveSnapshot.Id);

        //          // check that we're over
        //          Assert.IsNotNull(receiveResult);
        //          Assert.IsTrue(receiveResult.Success);
        //          Assert.AreEqual(receiveResult.OrderDetail.ExpectedSkus.Count, NumberOfSkuLines);   // check that we've still got all the sku lines we should have
        //          Assert.AreEqual(receiveResult.OrderDetail.ExpectedItems.Count, NumberOfItemLines); // check that we've still got all the item lines we should have
        //          foreach (var expectedItem in receiveResult.OrderDetail.ExpectedItems)  // check that all our items are allocated
        //              Assert.IsTrue(expectedItem.IsReceived);
        //          foreach (var expectedSku in receiveResult.OrderDetail.ExpectedSkus)    // check that all our sku's are allocated
        //          {
        //              if (expectedSku.SkuId == result.ExpectedSkus[0].SkuId)
        //                  Assert.AreEqual(expectedSku.Quantity + skuOverReceiveAmount, expectedSku.ReceivedTagNumbers.Count);
        //              else
        //                  Assert.AreEqual(expectedSku.Quantity, expectedSku.ReceivedTagNumbers.Count);
        //          }
        //          Assert.AreEqual(receiveResult.OrderDetail.AdditionalItems.Count, 0);   // check that we ahve no additional items
        //          Assert.AreEqual(receiveResult.OrderDetail.AdditionalSkus.Count, 0);    // check that we have no additional sku's

        //          // add the remove over Receive snapshot
        //          receiveResult = await _orderRepo.Receive(result, removeSkuOverReceiveSnapshot.Id);

        //          // check that we're not over anymore
        //          Assert.IsNotNull(receiveResult);
        //          Assert.IsTrue(receiveResult.Success);
        //          Assert.AreEqual(receiveResult.OrderDetail.ExpectedSkus.Count, NumberOfSkuLines);   // check that we've still got all the sku lines we should have
        //          Assert.AreEqual(receiveResult.OrderDetail.ExpectedItems.Count, NumberOfItemLines); // check that we've still got all the item lines we should have
        //          foreach (var expectedItem in receiveResult.OrderDetail.ExpectedItems)  // check that all our items are allocated
        //              Assert.IsTrue(expectedItem.IsReceived);
        //          foreach (var expectedSku in receiveResult.OrderDetail.ExpectedSkus)    // check that all our sku's are allocated
        //              Assert.AreEqual(expectedSku.Quantity, expectedSku.ReceivedTagNumbers.Count);
        //          Assert.AreEqual(receiveResult.OrderDetail.AdditionalItems.Count, 0);   // check that we ahve no additional items
        //          Assert.AreEqual(receiveResult.OrderDetail.AdditionalSkus.Count, 0);    // check that we have no additional sku's

        //          // now complete the order
        //          OrderActionResponseDto completeResult = await _orderRepo.Complete(result);

        //          Assert.IsNotNull(completeResult);
        //          Assert.IsTrue(completeResult.Success);
        //          Assert.AreEqual(completeResult.OrderDetail.State, Model.Enums.OrderState.Closed);
        //          Assert.AreEqual(completeResult.OrderDetail.ExpectedSkus.Count, NumberOfSkuLines);   // check that we've still got all the sku lines we should have
        //          Assert.AreEqual(completeResult.OrderDetail.ExpectedItems.Count, NumberOfItemLines); // check that we've still got all the item lines we should have
        //          foreach (var expectedItem in completeResult.OrderDetail.ExpectedItems)  // check that all our items are allocated
        //              Assert.IsTrue(expectedItem.IsAllocated);
        //          foreach (var expectedSku in completeResult.OrderDetail.ExpectedSkus)    // check that all our sku's are allocated
        //              Assert.AreEqual(expectedSku.Quantity, expectedSku.AllocatedTagNumbers.Count);
        //          Assert.AreEqual(completeResult.OrderDetail.AdditionalItems.Count, 0);   // check that we ahve no additional items
        //          Assert.AreEqual(completeResult.OrderDetail.AdditionalSkus.Count, 0);    // check that we have no additional sku's
        //          foreach (var additionalItem in completeResult.OrderDetail.AdditionalItems)  // check that all our additional items are allocated
        //              Assert.IsTrue(additionalItem.IsAllocated);
        //          foreach (var additionalSku in completeResult.OrderDetail.AdditionalSkus)    // check that all our additional sku's are allocated
        //              Assert.AreEqual(QtyPerSkuLine, additionalSku.AllocatedTagNumbers.Count);
        //      }

        #region Private_Methods
        private async Task<AddOrderDto> CreateRandomAddOrderDto(OrderType orderType, Guid? fromPlaceId = null, Guid? toPlaceId = null, Guid? personId = null, Guid? customerId = null)
        {
            var ran = new Random(DateTime.UtcNow.Millisecond);

            switch(orderType)
            {
                case OrderType.Inbound:
                    if (toPlaceId == null)
                        toPlaceId = WebRepoContainer.Place2Id;
                    break;
                case OrderType.Outbound:
                    if (fromPlaceId == null)
                        fromPlaceId = WebRepoContainer.Place2Id;
                    break;
                case OrderType.Transfer:
                    if (toPlaceId == null || fromPlaceId == null)
                    {
                        toPlaceId = WebRepoContainer.Place2Id;
                        fromPlaceId = WebRepoContainer.Place1Id;
                    }
                    break;
                case OrderType.Return:
                    throw new NotImplementedException("Return orders not supported yet");
                    break;
                case OrderType.Loan:
                    throw new NotImplementedException("Loans not supported yet");
                    break;
            }

            var refNumber = Guid.NewGuid().ToString();
            var description = Guid.NewGuid().ToString();
            var skuLineItems = await GenerateSomeSkuLineItems();
            var itemLineItems = await GenerateSomeItemLineItems();
            var addOrderDto = new AddOrderDto(orderType, refNumber, description, fromPlaceId,
                toPlaceId, skuLineItems, itemLineItems, personId, customerId);

            return addOrderDto;
        }

        

        private async Task<IList<AddOrderSkuDto>> GenerateSomeSkuLineItems(int numLines = 2, int qtyPerLine = 2)
        {
            var ran = new Random(DateTime.UtcNow.Millisecond);
            var skus = (await _skuRepo.QuerySkus()).Where(s => !string.IsNullOrEmpty(s.CompanyPrefix) && !string.IsNullOrEmpty(s.ItemReference)).ToList();

            // check that we have enough skus for the line items that we want
            Assert.IsTrue(numLines < skus.Count);

            var result = new List<AddOrderSkuDto>();

            while (result.Count < numLines)
            {
                var sku = skus[ran.Next(skus.Count - 1)];
                skus.Remove(sku);
                result.Add(new AddOrderSkuDto
                {
                    RequiredCount = qtyPerLine,
                    SkuId = sku.Id
                });
            }

            return result;
        }

        

        private async Task<IList<AddOrderUniqueItemDto>> GenerateSomeItemLineItems(int numItems = 2, Guid? placeId = null)
        {
            IItemRepo _itemRepo = WebRepoContainer.ItemRepo;
            var ran = new Random(DateTime.UtcNow.Millisecond);
            var items = new List<AddOrderUniqueItemDto>();

            // get skus that aren't sgtin
            var skus = (await _skuRepo.QuerySkus()).Where(s => !s.IsSgtin).ToList();
            // choose a random one to use
            SkuSummaryDto sku;
            int availableItems = 0;
            int skuSearchTries = 0;
            do
            {
                sku = skus[ran.Next(skus.Count - 1)];

                //var q0 = new ItemQuery();
                //q0.CreateQuery(i => i.SkuId, sku.Id, ComparisonOperator.Equals,0);
                var q0 = QueryBuilder<ItemSummaryDto>.NewQuery(i => i.SkuId, sku.Id, ComparisonOperator.Equals)
                    .And(i => i.TagNumber, null, ComparisonOperator.NotEquals)
                    .And(i => i.TagNumber, "", ComparisonOperator.NotEquals)
                    .Build();
                availableItems = (int)(await _itemRepo.QueryItems(q0)).Count;

                skuSearchTries++;

            } while (availableItems < numItems && skuSearchTries < (numItems * 2));

            // don't try and get more than there are available, create new items here
            if (availableItems < numItems)
            {
                // get a place to use to create the items
                if (placeId == null)
                    placeId = WebRepoContainer.Place1Id;
                var skuDetail = await _skuRepo.GetSku(sku.Id);
                while (availableItems < numItems)
                {
                    // create a new item 
                    var item = await _itemRepo.CreateItem(new AddItemDto(skuDetail, (Guid)placeId, "Test Generated Item - " + Guid.NewGuid().ToString().Substring(0, 8))
                    {
                        ItemTagList = new List<WriteTagDto>() { new WriteTagDto() { TagNumber = Guid.NewGuid().ToString(), TagType = Model.Enums.TagType.PassiveRfid } }
                    });
                    // we need this one in our list so add it straight in
                    items.Add(new AddOrderUniqueItemDto() { ItemId = item.Id });

                    availableItems++;
                }
            }

            // get existing items of the sku and add them to the list
            while (items.Count < numItems)
            {
                //var q1 = new ItemQuery();
                //q1.CreateQuery(i => i.SkuId, sku.Id, ComparisonOperator.Equals, 1, ran.Next(availableItems));
                var q1 = QueryBuilder<ItemSummaryDto>.NewQuery(i => i.SkuId, sku.Id, ComparisonOperator.Equals)
                    .And(i => i.TagNumber, null, ComparisonOperator.NotEquals)
                    .And(i => i.TagNumber, "", ComparisonOperator.NotEquals)
                    .Take(1)
                    .Skip(ran.Next(availableItems))
                    .Build();
                var item = await _itemRepo.QueryItemsContinuation(q1);

                if (item.Entities.Count > 0 && !items.Any(i => i.ItemId == item.Entities.First().Id))
                {
                    items.Add(new AddOrderUniqueItemDto()
                    {
                        ItemId = item.Entities.First().Id
                    });
                }
            }

            return items;
        }

        

        private async Task TestInboundOrderProces(OrderDetailDto orderDetail, List<KeyValuePair<Guid, int>> skusToUse, List<KeyValuePair<Guid, string>> itemsToUse)
        {
            if (skusToUse == null)
                skusToUse = new List<KeyValuePair<Guid, int>>();

            if (itemsToUse == null)
                itemsToUse = new List<KeyValuePair<Guid, string>>();

            var ran = new Random(DateTime.UtcNow.Millisecond);
            var skuPartialLineCount = skusToUse.Count > 0 ? ran.Next(1, skusToUse.Count - 1) : 0;
            var itemPartialLineCount = itemsToUse.Count > 0 ? ran.Next(1, itemsToUse.Count - 1) : 0;

            //// create and order so we at least have something to query
            //var addOrderDto = CreateAddOrderDto(OrderType.Inbound, skusToUse.ToDictionary(k => k.Key, v => v.Value), itemsToUse.Select(i => i.Key).ToList());

            //var orderDetail = await _orderRepo.Create(addOrderDto);
            //_ordersToDelete.AddUnique(orderDetail.Id);

            //// check response
            //OrderDtoValidator.OrderDetailcheck(orderDetail, OrderStateType.Created);
            //OrderDtoValidator.OrderCreateComparison(orderDetail, addOrderDto);

            // now receive partially, some sku line items and some item line items if present
            //var snapshotDto = await SnapshotGenerator.CreateNewGtinSnapshotForUpload(skusToUse[0].Value, skusToUse[0].Key);
            var snapshotDto = await SnapshotGenerator.GenerateSgtinSnapshot(skusToUse.Take(skuPartialLineCount).ToDictionary(k => k.Key, v => v.Value), itemsToUse.Take(itemPartialLineCount).Select(i => i.Value).ToList());
            _tagNumbersToDelete.AddRangeUnique(snapshotDto.Tags.Where(t => !itemsToUse.Select(i => i.Value).Contains(t.TagNumber)).Select(t => t.TagNumber));
            var addOrderSnapshotDto1 = new AddOrderSnapshotDto(orderDetail.Id, (Guid)orderDetail.ToPlaceId, snapshotDto);
            var responseDto = await _orderRepo.AddSnapshotToOrder(addOrderSnapshotDto1);
            var orderProgressDetail = responseDto.OrderDto;
            // check no notifications
            Validator.IsTrue(responseDto.Notifications.Count <= 0, "Notifications in response");

            // check that we are partially received
            OrderDtoValidator.OrderDetailcheck(orderProgressDetail, OrderStateType.Receiving);
            OrderDtoValidator.OrderDetailComparison(orderDetail, orderProgressDetail);
            // check that the correct sku's have been allocated
            foreach (var skuLine in skusToUse.Take(skuPartialLineCount))
            {
                Validator.IsTrue(skuLine.Value == orderProgressDetail.OrderSkuList.Where(s => s.Id == skuLine.Key).First().ReceivedTagNumbers.Count, "");
            }
            foreach (var skuLine in skusToUse.Skip(skuPartialLineCount))
            {
                Validator.IsTrue(0 == orderProgressDetail.OrderSkuList.Where(s => s.Id == skuLine.Key).First().ReceivedTagNumbers.Count, "");
            }
            // check that the correct items have been allocated
            foreach (var itemLine in itemsToUse.Take(itemPartialLineCount))
            {
                Validator.IsTrue(orderProgressDetail.OrderItemList.Where(s => s.Id == itemLine.Key).First().IsReceived, "");
                // check that they have been moved
                Validator.IsTrue(orderDetail.ToPlaceId == orderProgressDetail.OrderItemList.Where(s => s.Id == itemLine.Key).First().PlaceId);
            }
            foreach (var itemLine in itemsToUse.Skip(itemPartialLineCount))
            {
                Validator.IsTrue(!orderProgressDetail.OrderItemList.Where(s => s.Id == itemLine.Key).First().IsReceived, "");
            }

            // now allocate the rest fully
            //snapshotDto = await SnapshotGenerator.CreateNewGtinSnapshotForUpload(skusToUse[1].Value, skusToUse[1].Key);
            snapshotDto = await SnapshotGenerator.GenerateSgtinSnapshot(skusToUse.Skip(skuPartialLineCount).ToDictionary(k => k.Key, v => v.Value), itemsToUse.Skip(itemPartialLineCount).Select(i => i.Value).ToList());
            _tagNumbersToDelete.AddRangeUnique(snapshotDto.Tags.Where(t => !itemsToUse.Select(i => i.Value).Contains(t.TagNumber)).Select(t => t.TagNumber));
            var addOrderSnapshotDto2 = new AddOrderSnapshotDto(orderDetail.Id, (Guid)orderDetail.ToPlaceId, snapshotDto);
            responseDto = await _orderRepo.AddSnapshotToOrder(addOrderSnapshotDto2);
            orderProgressDetail = responseDto.OrderDto;
            // check no notifications
            Validator.IsTrue(responseDto.Notifications.Count <= 0, "Notifications in response");

            // check that the order is fully received
            OrderDtoValidator.OrderDetailcheck(orderProgressDetail, OrderStateType.Receiving);
            OrderDtoValidator.OrderDetailComparison(orderDetail, orderProgressDetail);
            // check that all skus have been allocated
            foreach (var skuLine in skusToUse)
            {
                Validator.IsTrue(skuLine.Value == orderProgressDetail.OrderSkuList.Where(s => s.Id == skuLine.Key).First().ReceivedTagNumbers.Count, "");
            }
            // check that the correct items have been allocated
            foreach (var itemLine in itemsToUse)
            {
                Validator.IsTrue(orderProgressDetail.OrderItemList.Where(s => s.Id == itemLine.Key).First().IsReceived, "");
                // check that they have been moved
//                Validator.IsTrue(orderDetail.ToPlaceId == orderProgressDetail.OrderItemList.Where(s => s.Id == itemLine.Key).First().PlaceId);
            }

            // now receive the order
            orderProgressDetail = await _orderRepo.Receive(orderProgressDetail.Id);
            OrderDtoValidator.OrderDetailcheck(orderProgressDetail, OrderStateType.Received);
            OrderDtoValidator.OrderDetailComparison(orderDetail, orderProgressDetail);

            // now complete the order
            orderProgressDetail = await _orderRepo.Complete(orderProgressDetail.Id);

            // check that the order is complete
            OrderDtoValidator.OrderDetailcheck(orderProgressDetail, OrderStateType.Completed);
            OrderDtoValidator.OrderDetailComparison(orderDetail, orderProgressDetail);

            // check all items have been moved
            foreach (var itemLine in itemsToUse)
            {
                // check that they have been moved
//                Validator.IsTrue(orderDetail.ToPlaceId == orderProgressDetail.OrderItemList.Where(s => s.Id == itemLine.Key).First().PlaceId);
            }
        }

        private async Task TestOutboundOrderProces(OrderDetailDto orderDetail, List<KeyValuePair<Guid, int>> skusToUse, List<KeyValuePair<Guid, string>> itemsToUse)
        {
            if (skusToUse == null)
                skusToUse = new List<KeyValuePair<Guid, int>>();

            if (itemsToUse == null)
                itemsToUse = new List<KeyValuePair<Guid, string>>();

            var ran = new Random(DateTime.UtcNow.Millisecond);
            var skuPartialLineCount = skusToUse.Count > 0 ? ran.Next(1, skusToUse.Count - 1) : 0;
            var itemPartialLineCount = itemsToUse.Count > 0 ? ran.Next(1, itemsToUse.Count - 1) : 0;

            //// create and order so we at least have something to query
            //var addOrderDto = CreateAddOrderDto(OrderType.Inbound, skusToUse.ToDictionary(k => k.Key, v => v.Value), itemsToUse.Select(i => i.Key).ToList());

            //var orderDetail = await _orderRepo.Create(addOrderDto);
            //_ordersToDelete.AddUnique(orderDetail.Id);

            //// check response
            //OrderDtoValidator.OrderDetailcheck(orderDetail, OrderStateType.Created);
            //OrderDtoValidator.OrderCreateComparison(orderDetail, addOrderDto);

            // now receive partially, some sku line items and some item line items if present
            //var snapshotDto = await SnapshotGenerator.CreateNewGtinSnapshotForUpload(skusToUse[0].Value, skusToUse[0].Key);
            var snapshotDto = await SnapshotGenerator.GenerateSgtinSnapshot(skusToUse.Take(skuPartialLineCount).ToDictionary(k => k.Key, v => v.Value), itemsToUse.Take(itemPartialLineCount).Select(i => i.Value).ToList());
            _tagNumbersToDelete.AddRangeUnique(snapshotDto.Tags.Where(t => !itemsToUse.Select(i => i.Value).Contains(t.TagNumber)).Select(t => t.TagNumber));
            var addOrderSnapshotDto1 = new AddOrderSnapshotDto(orderDetail.Id, (Guid)orderDetail.FromPlaceId, snapshotDto);
            var responseDto = await _orderRepo.AddSnapshotToOrder(addOrderSnapshotDto1);
            var orderProgressDetail = responseDto.OrderDto;
            // check no notifications
            Validator.IsTrue(responseDto.Notifications.Count <= 0, "Notifications in response");

            // check that we are partially allocated
            OrderDtoValidator.OrderDetailcheck(orderProgressDetail, OrderStateType.Allocating);
            OrderDtoValidator.OrderDetailComparison(orderDetail, orderProgressDetail);
            // check that the correct sku's have been allocated
            foreach (var skuLine in skusToUse.Take(skuPartialLineCount))
            {
                Validator.IsTrue(skuLine.Value == orderProgressDetail.OrderSkuList.Where(s => s.Id == skuLine.Key).First().AllocatedTagNumbers.Count, "");
            }
            foreach (var skuLine in skusToUse.Skip(skuPartialLineCount))
            {
                Validator.IsTrue(0 == orderProgressDetail.OrderSkuList.Where(s => s.Id == skuLine.Key).First().AllocatedTagNumbers.Count, "");
            }
            // check that the correct items have been allocated
            foreach (var itemLine in itemsToUse.Take(itemPartialLineCount))
            {
                Validator.IsTrue(orderProgressDetail.OrderItemList.Where(s => s.Id == itemLine.Key).First().IsAllocated, "");
            }
            foreach (var itemLine in itemsToUse.Skip(itemPartialLineCount))
            {
                Validator.IsTrue(!orderProgressDetail.OrderItemList.Where(s => s.Id == itemLine.Key).First().IsAllocated, "");
            }

            // now allocate the rest fully
            //snapshotDto = await SnapshotGenerator.CreateNewGtinSnapshotForUpload(skusToUse[1].Value, skusToUse[1].Key);
            snapshotDto = await SnapshotGenerator.GenerateSgtinSnapshot(skusToUse.Skip(skuPartialLineCount).ToDictionary(k => k.Key, v => v.Value), itemsToUse.Skip(itemPartialLineCount).Select(i => i.Value).ToList());
            _tagNumbersToDelete.AddRangeUnique(snapshotDto.Tags.Where(t => !itemsToUse.Select(i => i.Value).Contains(t.TagNumber)).Select(t => t.TagNumber));
            var addOrderSnapshotDto2 = new AddOrderSnapshotDto(orderDetail.Id, (Guid)orderDetail.FromPlaceId, snapshotDto);
            responseDto = await _orderRepo.AddSnapshotToOrder(addOrderSnapshotDto2);
            orderProgressDetail = responseDto.OrderDto;
            // check no notifications
            Validator.IsTrue(responseDto.Notifications.Count <= 0, "Notifications in response");

            // check that the order is fully received
            OrderDtoValidator.OrderDetailcheck(orderProgressDetail, OrderStateType.Allocating);
            OrderDtoValidator.OrderDetailComparison(orderDetail, orderProgressDetail);
            // check that all skus have been allocated
            foreach (var skuLine in skusToUse)
            {
                Validator.IsTrue(skuLine.Value == orderProgressDetail.OrderSkuList.Where(s => s.Id == skuLine.Key).First().AllocatedTagNumbers.Count, "");
            }
            // check that the correct items have been allocated
            foreach (var itemLine in itemsToUse)
            {
                Validator.IsTrue(orderProgressDetail.OrderItemList.Where(s => s.Id == itemLine.Key).First().IsAllocated, "");
            }

            // now allocate the order
            orderProgressDetail = await _orderRepo.Allocate(orderProgressDetail.Id);
            OrderDtoValidator.OrderDetailcheck(orderProgressDetail, OrderStateType.Allocated);
            OrderDtoValidator.OrderDetailComparison(orderDetail, orderProgressDetail);

            // check all items have had their state updated
            foreach (var itemLine in itemsToUse)
            {
                // check that they have been moved
                Validator.IsTrue(ItemStateType.Allocated == orderProgressDetail.OrderItemList.Where(s => s.Id == itemLine.Key).First().State);
            }

            // now Dispatch the order
            orderProgressDetail = await _orderRepo.Dispatch(orderProgressDetail.Id);

            // check that the order is dispatched
            OrderDtoValidator.OrderDetailcheck(orderProgressDetail, OrderStateType.Dispatched);
            OrderDtoValidator.OrderDetailComparison(orderDetail, orderProgressDetail);

            // check all items have had their state updated
            foreach (var itemLine in itemsToUse)
            {
                // check that they have been moved
//                Validator.IsTrue(ItemStateType.Dispatched == orderProgressDetail.OrderItemList.Where(s => s.Id == itemLine.Key).First().State);
            }
        }

        private async Task PrepareItemsForTest(List<KeyValuePair<Guid, string>> itemsToUse, Guid placeId)
        {
            // check each item and make sure it is in the correct location

            // get all items not currently in the required location
            var query = QueryBuilder<ItemSummaryDto>.NewQuery(i => i.TagNumber, string.Join(",", itemsToUse.Select(i => i.Value)), ComparisonOperator.ContainedIn)
                .And(i => i.PlaceId,placeId,ComparisonOperator.NotEquals)
                .Build();
            var items = await _itemRepo.QueryItems(query);

            // move all these items to the correct location
            foreach(var item in items.Items)
            {
                var moveDto = new UpdateItemPlaceDto()
                {
                    Id = item.Id,
                    NewPlaceId = placeId
                };
                var result = _itemRepo.UpdateItemPlace(moveDto);
            }
        }

        #endregion
    }
}

