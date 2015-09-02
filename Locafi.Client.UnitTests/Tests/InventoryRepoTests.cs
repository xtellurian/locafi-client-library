using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Data;
using Locafi.Client.Exceptions;
using Locafi.Client.Model.Dto.Inventory;
using Locafi.Client.Model.Dto.Places;
using Locafi.Client.Model.Dto.Snapshots;
using Locafi.Client.Model.Enums;
using Locafi.Client.Model.Query;
using Locafi.Client.Model.Query.PropertyComparison;
using Locafi.Client.UnitTests.EntityGenerators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Locafi.Client.UnitTests.Tests
{
    [TestClass]
    public class InventoryRepoTests
    {
        private IInventoryRepo _inventoryRepo;
        private IPlaceRepo _placeRepo;
        private ISnapshotRepo _snapshotRepo;
        private IReasonRepo _reasonRepo;
        private IItemRepo _itemRepo;
        private IList<Guid> _toCleanup;

        [TestInitialize]
        public void Initialize()
        {
            _inventoryRepo = WebRepoContainer.InventoryRepo;
            _placeRepo = WebRepoContainer.PlaceRepo;
            _snapshotRepo = WebRepoContainer.SnapshotRepo;
            _reasonRepo = WebRepoContainer.ReasonRepo;
            _itemRepo = WebRepoContainer.ItemRepo;
            _toCleanup = new List<Guid>();
        }

        [TestMethod]
        public async Task Inventory_GetAllInventories()
        {
            var inventories = await _inventoryRepo.GetAllInventories();
            Assert.IsNotNull(inventories);
            Assert.IsInstanceOfType(inventories, typeof(IEnumerable<InventorySummaryDto>));
        }
        [TestMethod]
        public async Task Inventory_GetDetail()
        {
            var inventories = await _inventoryRepo.GetAllInventories();
            Assert.IsNotNull(inventories, "inventories != null");
            foreach (var inventory in inventories)
            {
                var detail = await _inventoryRepo.GetInventory(inventory.Id);
                Assert.IsNotNull(detail, $"{inventory.Id} got null detail");
                Assert.IsInstanceOfType(detail,typeof(InventoryDetailDto));
                Assert.AreEqual(inventory,detail);
            }
        }

        [TestMethod]
        public async Task Inventory_Create()
        {
            var ran = new Random();
            var name = Guid.NewGuid().ToString();
            var places = await _placeRepo.GetAllPlaces();
            var place = places[ran.Next(places.Count - 1)];
            var result = await _inventoryRepo.CreateInventory(name, place.Id);

            Assert.IsInstanceOfType(result, typeof(InventoryDetailDto));
            Assert.IsTrue(string.Equals(result.Name, name));
            Assert.AreEqual(place.Id, result.PlaceId);
        }

        [TestMethod]
        public async Task Inventory_AddSnapshotSuccess()
        {
            var ran = new Random();
            var name = Guid.NewGuid().ToString();
            var places = await _placeRepo.GetAllPlaces();
            var place = places[ran.Next(places.Count - 1)];
            var inventory = await _inventoryRepo.CreateInventory(name, place.Id);

            var localSnapshot = SnapshotGenerator.CreateRandomSnapshotForUpload(inventory.PlaceId);
            var resultSnapshot = await _snapshotRepo.CreateSnapshot(localSnapshot);
            Assert.IsNotNull(resultSnapshot);
            Assert.IsInstanceOfType(resultSnapshot, typeof(SnapshotDetailDto));

            Assert.IsNotNull(inventory);
            Assert.IsInstanceOfType(inventory, typeof(InventoryDetailDto));
            var resultInventory = await _inventoryRepo.AddSnapshot(inventory, resultSnapshot.Id);

            Assert.IsNotNull(resultInventory);
            Assert.IsInstanceOfType(resultInventory, typeof(InventoryDetailDto));
            Assert.IsTrue(resultInventory.SnapshotIds.Contains(resultSnapshot.Id));
        }

        [TestMethod]
        [ExpectedException(typeof(InventoryException))]
        public async Task Inventory_AddSnapshotWrongPlace()
        {
            var ran = new Random();
            var name = Guid.NewGuid().ToString();
            var places = await _placeRepo.GetAllPlaces();
            var place = places[ran.Next(places.Count - 1)];
            var inventory = await _inventoryRepo.CreateInventory(name, place.Id);

            _toCleanup.Add(inventory.Id); // prepare to cleanup later
            place = await GetRandomPlace(inventory.PlaceId); // get a place not this palce
            var snapshot = SnapshotGenerator.CreateRandomSnapshotForUpload(place.Id);
            var resultSnapshot = await _snapshotRepo.CreateSnapshot(snapshot);
            Assert.IsNotNull(snapshot);

            var result = await _inventoryRepo.AddSnapshot(inventory, resultSnapshot.Id); // should throw exception
            
        }

        [TestMethod]
        public async Task Inventory_ResolveSuccess()
        {
            var ran = new Random();
            var name = Guid.NewGuid().ToString();
            var places = await _placeRepo.GetAllPlaces();
            var place = places[ran.Next(places.Count - 1)];
            var otherPlace = places.Where(p => p.Id != place.Id).ToList()[ran.Next(places.Count - 2)];
            var inventory = await _inventoryRepo.CreateInventory(name, place.Id);
            _toCleanup.Add(inventory.Id);

            var localSnapshot = await SimulateRealInventorySnapshot(place.Id, otherPlace.Id);
            var resultSnapshot = await _snapshotRepo.CreateSnapshot(localSnapshot);
            Assert.IsNotNull(resultSnapshot, "Failed to creat snapshot");
            var resultInventory = await _inventoryRepo.AddSnapshot(inventory, resultSnapshot.Id);
            Assert.IsNotNull(resultInventory, "Couldn't add snapshot");
            var resolution = new ResolveInventoryDto();
            foreach (var id in resultInventory.FoundItemsExpected) //TODO: Add Real Items to Inventory
            {
                var reasons = await _reasonRepo.GetReasonsFor(ReasonFor.Inventory_ExpectedItem);
                var reason = reasons[ran.Next(reasons.Count - 1)];
                resolution.Reasons.Add(id, reason.Id);
            }

            foreach (var id in resultInventory.FoundItemsUnexpected)
            {
                var reasons = await _reasonRepo.GetReasonsFor(ReasonFor.Inventory_ExpectedItem);
                var reason = reasons[ran.Next(reasons.Count - 1)];
                resolution.Reasons.Add(id, reason.Id);
            }

            foreach (var id in resultInventory.MissingItems)
            {
                var reasons = await _reasonRepo.GetReasonsFor(ReasonFor.Inventory_ExpectedItem);
                var reason = reasons[ran.Next(reasons.Count - 1)];
                resolution.Reasons.Add(id, reason.Id);
            }
            

            var resolvedInventory = await _inventoryRepo.Resolve(resultInventory.Id, resolution);

            Assert.IsNotNull(resolvedInventory);
            Assert.IsInstanceOfType(resolvedInventory,typeof(InventoryDetailDto));
            
        }



        [TestMethod]
        public async Task Inventory_Delete()
        {
            var ran = new Random();
            var name = Guid.NewGuid().ToString();
            var places = await _placeRepo.GetAllPlaces();
            var place = places[ran.Next(places.Count - 1)];
            var inventory = await _inventoryRepo.CreateInventory(name, place.Id);

            var inventories = await _inventoryRepo.GetAllInventories(); // get all
            Assert.IsTrue(inventories.Contains(inventory),"inventories.Contains(inventory)"); // assert all contains the one we just made

            await _inventoryRepo.Delete(inventory.Id); // delete the one we just made
            inventories = await _inventoryRepo.GetAllInventories(); // get all again
            Assert.IsFalse(inventories.Contains(inventory), "inventories.Contains(inventory)"); // assert the one we made no longer exists
        }

        [TestCleanup]
        public async void Cleanup()
        {
            foreach (var id in _toCleanup)
            {
                await _inventoryRepo.Delete(id);
            }
        }

        private async Task<AddSnapshotDto> SimulateRealInventorySnapshot(Guid placeId, Guid otherPlaceId)
        {
            var ran = new Random();
            var snapshot = new AddSnapshotDto();
            var query = new ItemQuery();
            // get all items in this place
            query.CreateQuery(i => i.PlaceId, placeId, ComparisonOperator.Equals);
            var items = await _itemRepo.QueryItems(query);
            //remove some random items
            items.RemoveAt(ran.Next(items.Count - 1));
            items.RemoveAt(ran.Next(items.Count - 1));
            foreach (var item in items)
            {
                if (string.IsNullOrEmpty(item.TagNumber)) continue;
                var tag = new SnapshotTagDto
                {
                    TagNumber = item.TagNumber
                };
                snapshot.Tags.Add(tag);
            }

            //add some random items
            query.CreateQuery(i=>i.PlaceId, otherPlaceId, ComparisonOperator.Equals);
            items = await _itemRepo.QueryItems(query);
            int newTags = 0;
            foreach (var item in items)
            {
                if (string.IsNullOrEmpty(item.TagNumber)) continue;
                var tag = new SnapshotTagDto
                {
                    TagNumber = item.TagNumber
                };
                snapshot.Tags.Add(tag);
                newTags ++;
                if (newTags > 3) break;
            }

            return snapshot;
        }

        private async Task<PlaceSummaryDto> GetRandomPlace(Guid notThisId)
        {
            var ran = new Random();
            var allPlaces = await _placeRepo.GetAllPlaces();
            PlaceSummaryDto place = null;
            while (place?.Id.Equals(notThisId) ?? true)
            {
                place = allPlaces[ran.Next(allPlaces.Count - 1)];
            }
            
            return place;
        }

    }
}
