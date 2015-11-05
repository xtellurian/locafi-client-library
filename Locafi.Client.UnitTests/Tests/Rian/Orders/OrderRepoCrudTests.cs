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

namespace Locafi.Client.UnitTests.Tests.Rian
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

        [TestInitialize]
        public void Initialize()
        {
            _placeRepo = WebRepoContainer.PlaceRepo;
            _personRepo = WebRepoContainer.PersonRepo;
            _orderRepo = WebRepoContainer.OrderRepo;
            _userRepo = WebRepoContainer.UserRepo;
            _skuRepo = WebRepoContainer.SkuRepo;
            _snapshotRepo = WebRepoContainer.SnapshotRepo;
        }

        [TestMethod]
        public async Task OrderCrud_CreateSuccess()
        {
            var ran = new Random();
            var places = await _placeRepo.GetAllPlaces();
            var numPlaces = places.Count;
            var sourcePlace = places[ran.Next(numPlaces - 1)]; // get random places
            var destinationPlace = places[ran.Next(numPlaces - 1)];

            var persons = await _personRepo.GetAllPersons();
            var person = persons[0];

            var refNumber = Guid.NewGuid().ToString();
            var description = Guid.NewGuid().ToString();
            var skuLineItems = await GenerateSomeSkuLineItems();
            var itemLineItems = await GenerateSomeItemLineItems();
            var addOrder = new AddOrderDto(refNumber, description, sourcePlace.Id, 
                destinationPlace.Id,skuLineItems, itemLineItems, person.Id);

            var result = await _orderRepo.Create(addOrder);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.DestinationPlaceId, addOrder.DestinationPlaceId);
            Assert.AreEqual(result.SourcePlaceId, addOrder.SourcePlaceId);
            Assert.AreEqual(result.DeliverToId, addOrder.DeliverToId);

        }

        private async Task<IList<AddOrderSkuLineItemDto>> GenerateSomeSkuLineItems(int numLines = 2, int qtyPerLine = 2)
        {
            var ran = new Random();
            var skus = (await _skuRepo.GetAllSkus()).Where(s => !string.IsNullOrEmpty(s.Gtin) && s.Gtin.Length == 13).ToList();
            var sku = skus[ran.Next(skus.Count - 1)];
            skus.Remove(sku);
            var sku2 = skus[ran.Next(skus.Count - 1)];

            // check that we have enough skus for the line items that we want
            Assert.IsTrue(numLines < skus.Count);

            var result = new List<AddOrderSkuLineItemDto>();

            for (int i = 0; i < numLines; i++)
            {
                result.Add(new AddOrderSkuLineItemDto
                {
                    PackingSize = 1,
                    Quantity = qtyPerLine,
                    SkuId = skus[ran.Next(skus.Count - 1)].Id
                });
            }

            return result;
        }

        private async Task<IList<AddOrderItemLineItemDto>> GenerateSomeItemLineItems(int numItems = 2)
        {
            IItemRepo _itemRepo = WebRepoContainer.ItemRepo;
            var ran = new Random();
            var items = new List<AddOrderItemLineItemDto>();

            // get skus that aren't sgtin
            var skus = (await _skuRepo.GetAllSkus()).Where(s => string.IsNullOrEmpty(s.Gtin) || s.Gtin.Length != 13).ToList();
            // choose a random one to use
            SkuSummaryDto sku;
            int availableItems = 0;
            int skuSearchTries = 0;
            do {
                sku = skus[ran.Next(skus.Count - 1)];

                var q0 = new ItemQuery();
                q0.CreateQuery(i => i.SkuId, sku.Id, ComparisonOperator.Equals);
                availableItems = await _itemRepo.GetItemCount(q0);

                skuSearchTries++;

            } while (availableItems < numItems && skuSearchTries < (numItems * 2));

            // don't try and get more than there are available, create new items here
            if (availableItems < numItems)
            {
                // get a place to use to create the items
                var places = await WebRepoContainer.PlaceRepo.GetAllPlaces();
                var place = places[ran.Next(places.Count - 1)];
                while (availableItems < numItems)
                {
                    // create a new item 
                    var item = await _itemRepo.CreateItem(new Model.Dto.Items.AddItemDto(sku.Id, place.Id, "Test Generated Item - " + Guid.NewGuid().ToString().Substring(0, 8)) { TagNumber = Guid.NewGuid().ToString() });
                    // we need this one in our list so add it straight in
                    items.Add(new AddOrderItemLineItemDto() { ItemId = item.Id });

                    availableItems++;
                }
            }
            
            while (items.Count < numItems)
            {
                var q1 = new ItemQuery();
                q1.CreateQuery(i => i.SkuId, sku.Id, ComparisonOperator.Equals, 1, ran.Next(availableItems));
                var item = await _itemRepo.QueryItemsAsync(q1);

                if (item.Entities.Count > 0 && !items.Any(i => i.ItemId == item.Entities.First().Id))
                {
                    items.Add(new AddOrderItemLineItemDto()
                    {
                        ItemId = item.Entities.First().Id
                    });
                }
            }

            return items; //TODO: actually make some
        }

        [TestMethod]
        public async Task OrderCrud_GetAllOrders()
        {
            var orders = await _orderRepo.GetAllOrders();
            Assert.IsNotNull(orders, "OrderRepo.GetAllOrders returned Null");
            Assert.IsInstanceOfType(orders,typeof(IEnumerable<OrderSummaryDto>));
        }

        [TestMethod]
        public async Task OrderCrud_GetOrderById()
        {
            var orders = await _orderRepo.GetAllOrders();
            Assert.IsNotNull(orders, "OrderRepo.GetAllOrders returned Null");

            foreach (var order in orders)
            {
                var result = await _orderRepo.GetOrderById(order.Id);
                Assert.IsNotNull(result);
                Assert.AreEqual(order,result);
            }
        }
  //      [TestMethod]
        public async Task OrderCrud_DeleteOrder()
        {
            // from create success above
            var ran = new Random();
            var places = await _placeRepo.GetAllPlaces();
            var numPlaces = places.Count;
            var sourcePlace = places[ran.Next(numPlaces - 1)]; // get random places
            var destinationPlace = places[ran.Next(numPlaces - 1)];

            var persons = await _personRepo.GetAllPersons();
            var person = persons[0];

            var refNumber = Guid.NewGuid().ToString();
            var description = Guid.NewGuid().ToString();
            var skuLineItems = await GenerateSomeSkuLineItems();
            var itemLineItems = await GenerateSomeItemLineItems();
            var addOrder = new AddOrderDto(refNumber, description, sourcePlace.Id,
                destinationPlace.Id, skuLineItems, itemLineItems, person.Id);

            var order = await _orderRepo.Create(addOrder);
            // use above for deletion
            Assert.IsNotNull(order);
            var success = await _orderRepo.DeleteOrder(order.Id);
            Assert.IsTrue(success);
            var allOrders = await _orderRepo.GetAllOrders();
            Assert.IsFalse(allOrders.Contains(order));
        }


        [TestMethod]
        public async Task OrderCrud_QueryOrders()
        {
            var ran = new Random();
            var places = await _placeRepo.GetAllPlaces();
            var numPlaces = places.Count;
            var sourcePlace = places[ran.Next(numPlaces - 1)]; // get random places
            var destinationPlace = places[ran.Next(numPlaces - 1)];
            var persons = await _personRepo.GetAllPersons();
            var person = persons[0];
            var refNumber = Guid.NewGuid().ToString();
            var description = Guid.NewGuid().ToString();
            var skuLineItems = await GenerateSomeSkuLineItems();
            var itemLineItems = await GenerateSomeItemLineItems();
            var addOrder = new AddOrderDto(refNumber, description, sourcePlace.Id, destinationPlace.Id,
                skuLineItems, itemLineItems, person.Id);
            var order = await _orderRepo.Create(addOrder);
            Assert.IsNotNull(order);

            var query = OrderQuery.NewQuery(o => o.DestinationPlaceId, order.DestinationPlaceId,
                ComparisonOperator.Equals);

            var queryResult = await _orderRepo.QueryOrdersAsync(query);
            Assert.IsNotNull(queryResult, "queryResult != null");
            Assert.IsTrue(queryResult.Entities.Contains(order));
        }

        [TestMethod]
        public async Task Order_AllocateOverSkuQuantity()
        {
            // create the order
            var ran = new Random();
            var places = await _placeRepo.GetAllPlaces();
            var numPlaces = places.Count;
            var sourcePlace = places[ran.Next(numPlaces - 1)]; // get random places
            var destinationPlace = places[ran.Next(numPlaces - 1)];

            var persons = await _personRepo.GetAllPersons();
            var person = persons[0];

            int NumberOfSkuLines = 2;
            int QtyPerSkuLine = 10;
            int NumberOfItemLines = 2;
            var refNumber = Guid.NewGuid().ToString();
            var description = Guid.NewGuid().ToString();
            var skuLineItems = await GenerateSomeSkuLineItems(NumberOfSkuLines, QtyPerSkuLine);
            var itemLineItems = await GenerateSomeItemLineItems(NumberOfItemLines);
            var addOrder = new AddOrderDto(refNumber, description, sourcePlace.Id,
                destinationPlace.Id, skuLineItems, itemLineItems, person.Id);

            var result = await _orderRepo.Create(addOrder);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.DestinationPlaceId, addOrder.DestinationPlaceId);
            Assert.AreEqual(result.SourcePlaceId, addOrder.SourcePlaceId);
            Assert.AreEqual(result.DeliverToId, addOrder.DeliverToId);

            // create snapshots to fully allocate the order (one for each line item as that is easiest)
            var addSsDtos = new List<AddSnapshotDto>();
            // create sku line snapshots
            foreach (var skuLine in result.ExpectedSkus)
            {
                addSsDtos.Add(await SnapshotGenerator.CreateExistingGtinSnapshotForUpload(result.SourcePlaceId,skuLine.Quantity,-1,skuLine.SkuId));
            }

            // create unique item snapshot
            if (result.ExpectedItems.Count > 0)
                addSsDtos.Add(SnapshotGenerator.CreateSnapshotForUpload(result.SourcePlaceId,result.ExpectedItems.Select(i => i.TagNumber).ToList()));

            var t1 = DateTime.UtcNow;
            var snapshotDtos = new List<SnapshotDetailDto>();
            foreach (var addSsDto in addSsDtos)
            {
                snapshotDtos.Add(await _snapshotRepo.CreateSnapshot(addSsDto));
            }

            // add snapshots to the order
            OrderActionResponseDto allocateResult = null;
            foreach(var ssDto in snapshotDtos)
            {
                allocateResult = await _orderRepo.Allocate(result, ssDto.Id);
            }

            Assert.IsNotNull(allocateResult);
            Assert.IsTrue(allocateResult.Success);
            Assert.AreEqual(allocateResult.OrderDetail.ExpectedSkus.Count, NumberOfSkuLines);   // check that we've still got all the sku lines we should have
            Assert.AreEqual(allocateResult.OrderDetail.ExpectedItems.Count, NumberOfItemLines); // check that we've still got all the item lines we should have
            foreach (var expectedItem in allocateResult.OrderDetail.ExpectedItems)  // check that all our items are allocated
                Assert.IsTrue(expectedItem.IsAllocated);
            foreach (var expectedSku in allocateResult.OrderDetail.ExpectedSkus)    // check that all our sku's are allocated
                Assert.AreEqual(expectedSku.Quantity, expectedSku.AllocatedTagNumbers.Count);
            Assert.AreEqual(allocateResult.OrderDetail.AdditionalItems.Count, 0);   // check that we ahve no additional items
            Assert.AreEqual(allocateResult.OrderDetail.AdditionalSkus.Count, 0);    // check that we have no additional sku's
            var t2 = DateTime.UtcNow;
            var span = t2 - t1;

            // now over allocate some sku's
            int skuOverAllocateAmount = 1;
            // create snapshots for over allocating and removing the over allocating
            var skuOverAllocate_AddSnapshotDto = await SnapshotGenerator.CreateNewGtinSnapshotForUpload(result.SourcePlaceId, skuOverAllocateAmount, -1, result.ExpectedSkus[0].SkuId);
            var addSkuOverAllocateSnapshot = await _snapshotRepo.CreateSnapshot(skuOverAllocate_AddSnapshotDto);
            skuOverAllocate_AddSnapshotDto.SnapshotType = Model.Enums.SnapshotType.Remove;
            var removeSkuOverAllocateSnapshot = await _snapshotRepo.CreateSnapshot(skuOverAllocate_AddSnapshotDto);

            // add the over allocate snapshot
            allocateResult = await _orderRepo.Allocate(result, addSkuOverAllocateSnapshot.Id);

            // check that we're over
            Assert.IsNotNull(allocateResult);
            Assert.IsTrue(allocateResult.Success);
            Assert.AreEqual(allocateResult.OrderDetail.ExpectedSkus.Count, NumberOfSkuLines);   // check that we've still got all the sku lines we should have
            Assert.AreEqual(allocateResult.OrderDetail.ExpectedItems.Count, NumberOfItemLines); // check that we've still got all the item lines we should have
            foreach (var expectedItem in allocateResult.OrderDetail.ExpectedItems)  // check that all our items are allocated
                Assert.IsTrue(expectedItem.IsAllocated);
            foreach (var expectedSku in allocateResult.OrderDetail.ExpectedSkus)    // check that all our sku's are allocated
            {
                if(expectedSku.SkuId == result.ExpectedSkus[0].SkuId)
                    Assert.AreEqual(expectedSku.Quantity + skuOverAllocateAmount, expectedSku.AllocatedTagNumbers.Count);
                else
                    Assert.AreEqual(expectedSku.Quantity, expectedSku.AllocatedTagNumbers.Count);
            }
            Assert.AreEqual(allocateResult.OrderDetail.AdditionalItems.Count, 0);   // check that we ahve no additional items
            Assert.AreEqual(allocateResult.OrderDetail.AdditionalSkus.Count, 0);    // check that we have no additional sku's

            // add the remove over allocate snapshot
            allocateResult = await _orderRepo.Allocate(result, removeSkuOverAllocateSnapshot.Id);

            // check that we're not over anymore
            Assert.IsNotNull(allocateResult);
            Assert.IsTrue(allocateResult.Success);
            Assert.AreEqual(allocateResult.OrderDetail.ExpectedSkus.Count, NumberOfSkuLines);   // check that we've still got all the sku lines we should have
            Assert.AreEqual(allocateResult.OrderDetail.ExpectedItems.Count, NumberOfItemLines); // check that we've still got all the item lines we should have
            foreach (var expectedItem in allocateResult.OrderDetail.ExpectedItems)  // check that all our items are allocated
                Assert.IsTrue(expectedItem.IsAllocated);
            foreach (var expectedSku in allocateResult.OrderDetail.ExpectedSkus)    // check that all our sku's are allocated
                Assert.AreEqual(expectedSku.Quantity, expectedSku.AllocatedTagNumbers.Count);
            Assert.AreEqual(allocateResult.OrderDetail.AdditionalItems.Count, 0);   // check that we ahve no additional items
            Assert.AreEqual(allocateResult.OrderDetail.AdditionalSkus.Count, 0);    // check that we have no additional sku's
        }

        [TestMethod]
        public async Task Order_AllocateAdditionalItems()
        {
            // create the order
            var ran = new Random();
            var places = await _placeRepo.GetAllPlaces();
            var numPlaces = places.Count;
            var sourcePlace = places[ran.Next(numPlaces - 1)]; // get random places
            var destinationPlace = places[ran.Next(numPlaces - 1)];

            var persons = await _personRepo.GetAllPersons();
            var person = persons[0];

            int NumberOfSkuLines = 2;
            int QtyPerSkuLine = 10;
            int NumberOfItemLines = 2;
            var refNumber = Guid.NewGuid().ToString();
            var description = Guid.NewGuid().ToString();
            var skuLineItems = await GenerateSomeSkuLineItems(NumberOfSkuLines, QtyPerSkuLine);
            var itemLineItems = await GenerateSomeItemLineItems(NumberOfItemLines);
            var addOrder = new AddOrderDto(refNumber, description, sourcePlace.Id,
                destinationPlace.Id, skuLineItems, itemLineItems, person.Id);

            var result = await _orderRepo.Create(addOrder);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.DestinationPlaceId, addOrder.DestinationPlaceId);
            Assert.AreEqual(result.SourcePlaceId, addOrder.SourcePlaceId);
            Assert.AreEqual(result.DeliverToId, addOrder.DeliverToId);

            // create snapshots to fully allocate the order (one for each line item as that is easiest)
            var addSsDtos = new List<AddSnapshotDto>();
            // create sku line snapshots
            foreach (var skuLine in result.ExpectedSkus)
            {
                addSsDtos.Add(await SnapshotGenerator.CreateExistingGtinSnapshotForUpload(result.SourcePlaceId, skuLine.Quantity, -1, skuLine.SkuId));
            }

            // create unique item snapshot
            if (result.ExpectedItems.Count > 0)
                addSsDtos.Add(SnapshotGenerator.CreateSnapshotForUpload(result.SourcePlaceId, result.ExpectedItems.Select(i => i.TagNumber).ToList()));

            var t1 = DateTime.UtcNow;
            var snapshotDtos = new List<SnapshotDetailDto>();
            foreach (var addSsDto in addSsDtos)
            {
                snapshotDtos.Add(await _snapshotRepo.CreateSnapshot(addSsDto));
            }

            // add snapshots to the order
            OrderActionResponseDto allocateResult = null;
            foreach (var ssDto in snapshotDtos)
            {
                allocateResult = await _orderRepo.Allocate(result, ssDto.Id);
            }

            Assert.IsNotNull(allocateResult);
            Assert.IsTrue(allocateResult.Success);
            Assert.AreEqual(allocateResult.OrderDetail.ExpectedSkus.Count, NumberOfSkuLines);   // check that we've still got all the sku lines we should have
            Assert.AreEqual(allocateResult.OrderDetail.ExpectedItems.Count, NumberOfItemLines); // check that we've still got all the item lines we should have
            foreach (var expectedItem in allocateResult.OrderDetail.ExpectedItems)  // check that all our items are allocated
                Assert.IsTrue(expectedItem.IsAllocated);
            foreach (var expectedSku in allocateResult.OrderDetail.ExpectedSkus)    // check that all our sku's are allocated
                Assert.AreEqual(expectedSku.Quantity, expectedSku.AllocatedTagNumbers.Count);
            Assert.AreEqual(allocateResult.OrderDetail.AdditionalItems.Count, 0);   // check that we ahve no additional items
            Assert.AreEqual(allocateResult.OrderDetail.AdditionalSkus.Count, 0);    // check that we have no additional sku's
            var t2 = DateTime.UtcNow;
            var span = t2 - t1;

            // now over allocate some items (additional items)
            int itemOverAllocateAmount = NumberOfItemLines + 1;
            // create snapshots for over allocating and removing the over allocating
            var items = (await GenerateSomeItemLineItems(itemOverAllocateAmount)).Where(i => !result.ExpectedItems.Select(e => e.ItemId).ToList().Contains(i.ItemId)).Select(i => i.ItemId).ToList();
            var itemOverAllocate_AddSnapshotDto = await SnapshotGenerator.CreateExistingItemSnapshotForUpload(result.SourcePlaceId, items);
            var itemAddOverAllocateSnapshot = await _snapshotRepo.CreateSnapshot(itemOverAllocate_AddSnapshotDto);
            itemOverAllocate_AddSnapshotDto.SnapshotType = Model.Enums.SnapshotType.Remove;
            var itemRemoveOverAllocateSnapshot = await _snapshotRepo.CreateSnapshot(itemOverAllocate_AddSnapshotDto);

            // add the over allocate snapshot
            allocateResult = await _orderRepo.Allocate(result, itemAddOverAllocateSnapshot.Id);

            // check that we're over
            Assert.IsNotNull(allocateResult);
            Assert.IsTrue(allocateResult.Success);
            Assert.AreEqual(allocateResult.OrderDetail.ExpectedSkus.Count, NumberOfSkuLines);   // check that we've still got all the sku lines we should have
            Assert.AreEqual(allocateResult.OrderDetail.ExpectedItems.Count, NumberOfItemLines); // check that we've still got all the item lines we should have
            foreach (var expectedItem in allocateResult.OrderDetail.ExpectedItems)  // check that all our items are allocated
                Assert.IsTrue(expectedItem.IsAllocated);
            foreach (var expectedSku in allocateResult.OrderDetail.ExpectedSkus)    // check that all our sku's are allocated
                Assert.AreEqual(expectedSku.Quantity, expectedSku.AllocatedTagNumbers.Count);
            Assert.AreEqual(allocateResult.OrderDetail.AdditionalItems.Count, items.Count); // check that we have the required additional items
            foreach (var additionalItem in allocateResult.OrderDetail.AdditionalItems)  // check that all our additional items are allocated
                Assert.IsTrue(additionalItem.IsAllocated);
            Assert.AreEqual(allocateResult.OrderDetail.AdditionalSkus.Count, 0);    // check that we have no additional sku's

            // add the remove over allocate snapshot
            allocateResult = await _orderRepo.Allocate(result, itemRemoveOverAllocateSnapshot.Id);

            // check that we're not over anymore
            Assert.IsNotNull(allocateResult);
            Assert.IsTrue(allocateResult.Success);
            Assert.AreEqual(allocateResult.OrderDetail.ExpectedSkus.Count, NumberOfSkuLines);   // check that we've still got all the sku lines we should have
            Assert.AreEqual(allocateResult.OrderDetail.ExpectedItems.Count, NumberOfItemLines); // check that we've still got all the item lines we should have
            foreach (var expectedItem in allocateResult.OrderDetail.ExpectedItems)  // check that all our items are allocated
                Assert.IsTrue(expectedItem.IsAllocated);
            foreach (var expectedSku in allocateResult.OrderDetail.ExpectedSkus)    // check that all our sku's are allocated
                Assert.AreEqual(expectedSku.Quantity, expectedSku.AllocatedTagNumbers.Count);
            Assert.AreEqual(allocateResult.OrderDetail.AdditionalItems.Count, 0); // check that our additional items are gone
            Assert.AreEqual(allocateResult.OrderDetail.AdditionalSkus.Count, 0);    // check that we have no additional sku's
        }

        [TestMethod]
        public async Task Order_AllocateWithAdditionalSkuLineItems()
        {
            // create the order
            var ran = new Random();
            var places = await _placeRepo.GetAllPlaces();
            var numPlaces = places.Count;
            var sourcePlace = places[ran.Next(numPlaces - 1)]; // get random places
            var destinationPlace = places[ran.Next(numPlaces - 1)];

            var persons = await _personRepo.GetAllPersons();
            var person = persons[0];

            int NumberOfSkuLines = 2;
            int QtyPerSkuLine = 10;
            int NumberOfItemLines = 2;

            var refNumber = Guid.NewGuid().ToString();
            var description = Guid.NewGuid().ToString();
            var skuLineItems = await GenerateSomeSkuLineItems(NumberOfSkuLines, QtyPerSkuLine);
            var itemLineItems = await GenerateSomeItemLineItems(NumberOfItemLines);
            var addOrder = new AddOrderDto(refNumber, description, sourcePlace.Id,
                destinationPlace.Id, skuLineItems, itemLineItems, person.Id);

            var result = await _orderRepo.Create(addOrder);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.DestinationPlaceId, addOrder.DestinationPlaceId);
            Assert.AreEqual(result.SourcePlaceId, addOrder.SourcePlaceId);
            Assert.AreEqual(result.DeliverToId, addOrder.DeliverToId);

            // create snapshots to fully allocate the order (one for each line item as that is easiest)
            var addSsDtos = new List<AddSnapshotDto>();
            // create sku line AddSnapshotDto's
            foreach (var skuLine in result.ExpectedSkus)
            {
                addSsDtos.Add(await SnapshotGenerator.CreateExistingGtinSnapshotForUpload(result.SourcePlaceId, skuLine.Quantity, -1, skuLine.SkuId));
            }

            // create unique item AddSnapshotDto
            if(result.ExpectedItems.Count > 0)
                addSsDtos.Add(SnapshotGenerator.CreateSnapshotForUpload(result.SourcePlaceId, result.ExpectedItems.Select(i => i.TagNumber).ToList()));

            // create the snapshots
            var t1 = DateTime.UtcNow;
            var snapshotDtos = new List<SnapshotDetailDto>();
            foreach (var addSsDto in addSsDtos)
            {
                snapshotDtos.Add(await _snapshotRepo.CreateSnapshot(addSsDto));
            }

            // add snapshots to the order
            OrderActionResponseDto allocateResult = null;
            foreach (var ssDto in snapshotDtos)
            {
                allocateResult = await _orderRepo.Allocate(result, ssDto.Id);
            }

            Assert.IsNotNull(allocateResult);
            Assert.IsTrue(allocateResult.Success);
            Assert.AreEqual(allocateResult.OrderDetail.ExpectedSkus.Count, NumberOfSkuLines);   // check that we've still got all the sku lines we should have
            Assert.AreEqual(allocateResult.OrderDetail.ExpectedItems.Count, NumberOfItemLines); // check that we've still got all the item lines we should have
            foreach (var expectedItem in allocateResult.OrderDetail.ExpectedItems)  // check that all our items are allocated
                Assert.IsTrue(expectedItem.IsAllocated);
            foreach (var expectedSku in allocateResult.OrderDetail.ExpectedSkus)    // check that all our sku's are allocated
                Assert.AreEqual(expectedSku.Quantity, expectedSku.AllocatedTagNumbers.Count);
            Assert.AreEqual(allocateResult.OrderDetail.AdditionalItems.Count, 0);   // check that we ahve no additional items
            Assert.AreEqual(allocateResult.OrderDetail.AdditionalSkus.Count, 0);    // check that we have no additional sku's
            foreach (var additionalItem in allocateResult.OrderDetail.AdditionalItems)  // check that all our additional items are allocated
                Assert.IsTrue(additionalItem.IsAllocated);
            foreach (var additionalSku in allocateResult.OrderDetail.AdditionalSkus)    // check that all our additional sku's are allocated
                Assert.AreEqual(QtyPerSkuLine, additionalSku.AllocatedTagNumbers.Count);

            // create a AddSnapshotDto for an additional sku line
            var skus = (await _skuRepo.GetAllSkus()).Where(s => !string.IsNullOrEmpty(s.Gtin) && s.Gtin.Length == 13 && !result.ExpectedSkus.Select(o => o.SkuId).ToList().Contains(s.Id)).ToList(); // get a different sku
            var additionalSku_AddSsDto = await SnapshotGenerator.CreateExistingGtinSnapshotForUpload(result.SourcePlaceId, QtyPerSkuLine, -1, skus[ran.Next(skus.Count - 1)].Id);
            var addAdditionalSs = await _snapshotRepo.CreateSnapshot(additionalSku_AddSsDto);
            additionalSku_AddSsDto.SnapshotType = Model.Enums.SnapshotType.Remove;
            var removeAdditionalSs = await _snapshotRepo.CreateSnapshot(additionalSku_AddSsDto);

            // now allocate the additional skus
            allocateResult = await _orderRepo.Allocate(result, addAdditionalSs.Id);

            // check they're allocated
            Assert.IsNotNull(allocateResult);
            Assert.IsTrue(allocateResult.Success);
            Assert.AreEqual(allocateResult.OrderDetail.ExpectedSkus.Count, NumberOfSkuLines);   // check that we've still got all the sku lines we should have
            Assert.AreEqual(allocateResult.OrderDetail.ExpectedItems.Count, NumberOfItemLines); // check that we've still got all the item lines we should have
            foreach (var expectedItem in allocateResult.OrderDetail.ExpectedItems)  // check that all our items are allocated
                Assert.IsTrue(expectedItem.IsAllocated);
            foreach (var expectedSku in allocateResult.OrderDetail.ExpectedSkus)    // check that all our sku's are allocated
                Assert.AreEqual(expectedSku.Quantity, expectedSku.AllocatedTagNumbers.Count);
            Assert.AreEqual(allocateResult.OrderDetail.AdditionalItems.Count, 0);   // check that we ahve no additional items
            Assert.AreEqual(allocateResult.OrderDetail.AdditionalSkus.Count, 1);    // check that we have no additional sku's
            foreach (var additionalItem in allocateResult.OrderDetail.AdditionalItems)  // check that all our additional items are allocated
                Assert.IsTrue(additionalItem.IsAllocated);
            foreach (var additionalSku in allocateResult.OrderDetail.AdditionalSkus)    // check that all our additional sku's are allocated
                Assert.AreEqual(QtyPerSkuLine, additionalSku.AllocatedTagNumbers.Count);

            // now remove the additional sku's
            allocateResult = await _orderRepo.Allocate(result, removeAdditionalSs.Id);

            // checked they're removed
            Assert.IsNotNull(allocateResult);
            Assert.IsTrue(allocateResult.Success);
            Assert.AreEqual(allocateResult.OrderDetail.ExpectedSkus.Count, NumberOfSkuLines);   // check that we've still got all the sku lines we should have
            Assert.AreEqual(allocateResult.OrderDetail.ExpectedItems.Count, NumberOfItemLines); // check that we've still got all the item lines we should have
            foreach (var expectedItem in allocateResult.OrderDetail.ExpectedItems)  // check that all our items are allocated
                Assert.IsTrue(expectedItem.IsAllocated);
            foreach (var expectedSku in allocateResult.OrderDetail.ExpectedSkus)    // check that all our sku's are allocated
                Assert.AreEqual(expectedSku.Quantity, expectedSku.AllocatedTagNumbers.Count);
            Assert.AreEqual(allocateResult.OrderDetail.AdditionalItems.Count, 0);   // check that we ahve no additional items
            Assert.AreEqual(allocateResult.OrderDetail.AdditionalSkus.Count, 0);    // check that we have no additional sku's
            foreach (var additionalItem in allocateResult.OrderDetail.AdditionalItems)  // check that all our additional items are allocated
                Assert.IsTrue(additionalItem.IsAllocated);
            foreach (var additionalSku in allocateResult.OrderDetail.AdditionalSkus)    // check that all our additional sku's are allocated
                Assert.AreEqual(QtyPerSkuLine, additionalSku.AllocatedTagNumbers.Count);

            // create the additional snapshot
            var t2 = DateTime.UtcNow;
            var span = t2 - t1;
        }

        [TestMethod]
        public async Task Order_AllocateWithSkuItemsAsUniqueItems()
        {
            IItemRepo _itemRepo = WebRepoContainer.ItemRepo;

            // create the order
            var ran = new Random();
            var places = await _placeRepo.GetAllPlaces();
            var numPlaces = places.Count;
            var sourcePlace = places[ran.Next(numPlaces - 1)]; // get random places
            var destinationPlace = places[ran.Next(numPlaces - 1)];

            var persons = await _personRepo.GetAllPersons();
            var person = persons[0];

            int NumberOfSkuLines = 2;
            int QtyPerSkuLine = 10;
            int NumberOfItemLines = 2;
            var refNumber = Guid.NewGuid().ToString();
            var description = Guid.NewGuid().ToString();
            var skuLineItems = await GenerateSomeSkuLineItems(NumberOfSkuLines, QtyPerSkuLine);
            var itemLineItems = await GenerateSomeItemLineItems(NumberOfItemLines);

            // create some item line items with sku items
            var skus = (await _skuRepo.GetAllSkus()).Where(s => !string.IsNullOrEmpty(s.Gtin) && s.Gtin.Length == 13 && !skuLineItems.Select(i => i.SkuId).ToList().Contains(s.Id)).ToList();
            var sku = skus[ran.Next(skus.Count - 1)];
            for (int count = 0; count < NumberOfItemLines; count++)
            {
                var q1 = new ItemQuery();
                q1.CreateQuery(i => i.SkuId, sku.Id, ComparisonOperator.Equals, 1, ran.Next(100));
                var item = await _itemRepo.QueryItemsAsync(q1);

                if (item.Entities.Count > 0 && !itemLineItems.Any(i => i.ItemId == item.Entities.First().Id))
                {
                    itemLineItems.Add(new AddOrderItemLineItemDto()
                    {
                        ItemId = item.Entities.First().Id
                    });
                }
                else
                    count--;
            }

            var addOrder = new AddOrderDto(refNumber, description, sourcePlace.Id,
                destinationPlace.Id, skuLineItems, itemLineItems, person.Id);

            var result = await _orderRepo.Create(addOrder);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.DestinationPlaceId, addOrder.DestinationPlaceId);
            Assert.AreEqual(result.SourcePlaceId, addOrder.SourcePlaceId);
            Assert.AreEqual(result.DeliverToId, addOrder.DeliverToId);

            // create snapshots to fully allocate the order (one for each line item as that is easiest)
            var addSsDtos = new List<AddSnapshotDto>();
            // create sku line snapshots
            foreach (var skuLine in result.ExpectedSkus)
            {
                addSsDtos.Add(await SnapshotGenerator.CreateExistingGtinSnapshotForUpload(result.SourcePlaceId, skuLine.Quantity, -1, skuLine.SkuId));
            }

            // create unique item snapshot
            if (result.ExpectedItems.Count > 0)
                addSsDtos.Add(SnapshotGenerator.CreateSnapshotForUpload(result.SourcePlaceId, result.ExpectedItems.Select(i => i.TagNumber).ToList()));

            var t1 = DateTime.UtcNow;
            var snapshotDtos = new List<SnapshotDetailDto>();
            foreach (var addSsDto in addSsDtos)
            {
                snapshotDtos.Add(await _snapshotRepo.CreateSnapshot(addSsDto));
            }

            // add snapshots to the order
            OrderActionResponseDto allocateResult = null;
            foreach (var ssDto in snapshotDtos)
            {
                allocateResult = await _orderRepo.Allocate(result, ssDto.Id);
            }

            Assert.IsNotNull(allocateResult);
            Assert.IsTrue(allocateResult.Success);
            Assert.AreEqual(allocateResult.OrderDetail.ExpectedSkus.Count, NumberOfSkuLines);   // check that we've still got all the sku lines we should have
            Assert.AreEqual(allocateResult.OrderDetail.ExpectedItems.Count, NumberOfItemLines * 2); // check that we've still got all the item lines we should have
            foreach (var expectedItem in allocateResult.OrderDetail.ExpectedItems)  // check that all our items are allocated
                Assert.IsTrue(expectedItem.IsAllocated);
            foreach (var expectedSku in allocateResult.OrderDetail.ExpectedSkus)    // check that all our sku's are allocated
                Assert.AreEqual(expectedSku.Quantity, expectedSku.AllocatedTagNumbers.Count);
            Assert.AreEqual(allocateResult.OrderDetail.AdditionalItems.Count, 0);   // check that we ahve no additional items
            Assert.AreEqual(allocateResult.OrderDetail.AdditionalSkus.Count, 0);    // check that we have no additional sku's
            var t2 = DateTime.UtcNow;
            var span = t2 - t1;

            // now over allocate some items (additional items)
            int itemOverAllocateAmount = NumberOfItemLines + 1;
            // create snapshots for over allocating and removing the over allocating
            var items = (await GenerateSomeItemLineItems(itemOverAllocateAmount)).Where(i => !result.ExpectedItems.Select(e => e.ItemId).ToList().Contains(i.ItemId)).Select(i => i.ItemId).ToList();
            var itemOverAllocate_AddSnapshotDto = await SnapshotGenerator.CreateExistingItemSnapshotForUpload(result.SourcePlaceId, items);
            var itemAddOverAllocateSnapshot = await _snapshotRepo.CreateSnapshot(itemOverAllocate_AddSnapshotDto);
            itemOverAllocate_AddSnapshotDto.SnapshotType = Model.Enums.SnapshotType.Remove;
            var itemRemoveOverAllocateSnapshot = await _snapshotRepo.CreateSnapshot(itemOverAllocate_AddSnapshotDto);

            // add the over allocate snapshot
            allocateResult = await _orderRepo.Allocate(result, itemAddOverAllocateSnapshot.Id);

            // check that we're over
            Assert.IsNotNull(allocateResult);
            Assert.IsTrue(allocateResult.Success);
            Assert.AreEqual(allocateResult.OrderDetail.ExpectedSkus.Count, NumberOfSkuLines);   // check that we've still got all the sku lines we should have
            Assert.AreEqual(allocateResult.OrderDetail.ExpectedItems.Count, NumberOfItemLines * 2); // check that we've still got all the item lines we should have
            foreach (var expectedItem in allocateResult.OrderDetail.ExpectedItems)  // check that all our items are allocated
                Assert.IsTrue(expectedItem.IsAllocated);
            foreach (var expectedSku in allocateResult.OrderDetail.ExpectedSkus)    // check that all our sku's are allocated
                Assert.AreEqual(expectedSku.Quantity, expectedSku.AllocatedTagNumbers.Count);
            Assert.AreEqual(allocateResult.OrderDetail.AdditionalItems.Count, items.Count); // check that we have the required additional items
            foreach (var additionalItem in allocateResult.OrderDetail.AdditionalItems)  // check that all our additional items are allocated
                Assert.IsTrue(additionalItem.IsAllocated);
            Assert.AreEqual(allocateResult.OrderDetail.AdditionalSkus.Count, 0);    // check that we have no additional sku's

            // add the remove over allocate snapshot
            allocateResult = await _orderRepo.Allocate(result, itemRemoveOverAllocateSnapshot.Id);

            // check that we're not over anymore
            Assert.IsNotNull(allocateResult);
            Assert.IsTrue(allocateResult.Success);
            Assert.AreEqual(allocateResult.OrderDetail.ExpectedSkus.Count, NumberOfSkuLines);   // check that we've still got all the sku lines we should have
            Assert.AreEqual(allocateResult.OrderDetail.ExpectedItems.Count, NumberOfItemLines * 2); // check that we've still got all the item lines we should have
            foreach (var expectedItem in allocateResult.OrderDetail.ExpectedItems)  // check that all our items are allocated
                Assert.IsTrue(expectedItem.IsAllocated);
            foreach (var expectedSku in allocateResult.OrderDetail.ExpectedSkus)    // check that all our sku's are allocated
                Assert.AreEqual(expectedSku.Quantity, expectedSku.AllocatedTagNumbers.Count);
            Assert.AreEqual(allocateResult.OrderDetail.AdditionalItems.Count, 0); // check that our additional items are gone
            Assert.AreEqual(allocateResult.OrderDetail.AdditionalSkus.Count, 0);    // check that we have no additional sku's
        }
    }
}

