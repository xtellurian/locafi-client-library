using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Exceptions;
using Locafi.Client.Model.Dto.Inventory;
using Locafi.Client.Model.Dto.Items;
using Locafi.Client.Model.Dto.Places;
using Locafi.Client.Model.Dto.Snapshots;
using Locafi.Client.Model.Enums;
using Locafi.Client.Model.Query;
using Locafi.Client.Model.Query.PropertyComparison;
using Locafi.Client.UnitTests.EntityGenerators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Locafi.Client.UnitTests.Tests.Rian
{
    [TestClass]
    public class InventoryProcessRepoTests
    {
        private IInventoryRepo _inventoryRepo;
        private IPlaceRepo _placeRepo;
        private ISnapshotRepo _snapshotRepo;
        private IReasonRepo _reasonRepo;
        private IItemRepo _itemRepo;
        private IUserRepo _userRepo;
        private ISkuRepo _skuRepo;

        [TestInitialize]
        public void Initialize()
        {
            _inventoryRepo = WebRepoContainer.InventoryRepo;
            _placeRepo = WebRepoContainer.PlaceRepo;
            _snapshotRepo = WebRepoContainer.SnapshotRepo;
            _reasonRepo = WebRepoContainer.ReasonRepo;
            _itemRepo = WebRepoContainer.ItemRepo;
            _userRepo = WebRepoContainer.UserRepo;
            _skuRepo = WebRepoContainer.SkuRepo;
        }
  //      [TestMethod]
        public async Task InventoryProcess_AddSnapshotSuccess()
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

  //      [TestMethod]
        [ExpectedException(typeof(InventoryException))]
        public async Task InventoryProcess_AddSnapshotWrongPlace()
        {
            var ran = new Random();
            var name = Guid.NewGuid().ToString();
            var places = await _placeRepo.GetAllPlaces();
            var place = places[ran.Next(places.Count - 1)];
            var inventory = await _inventoryRepo.CreateInventory(name, place.Id);

            place = await GetRandomPlace(inventory.PlaceId); // get a place not this palce
            var snapshot = SnapshotGenerator.CreateRandomSnapshotForUpload(place.Id);
            var resultSnapshot = await _snapshotRepo.CreateSnapshot(snapshot);
            Assert.IsNotNull(snapshot);

            var result = await _inventoryRepo.AddSnapshot(inventory, resultSnapshot.Id); // should throw exception

        }

   //     [TestMethod]
        public async Task InventoryProcess_ResolveSuccess()
        {
            var ran = new Random();
            var name = Guid.NewGuid().ToString();
            var places = await _placeRepo.GetAllPlaces();
            var place = places[ran.Next(places.Count - 1)];
            var otherPlace = places.Where(p => p.Id != place.Id).ToList()[ran.Next(places.Count - 2)];
            var inventory = await _inventoryRepo.CreateInventory(name, place.Id);

            var localSnapshot = await SimulateRealInventorySnapshot(place.Id, otherPlace.Id);//TODO: fix this method
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
                var reasons = await _reasonRepo.GetReasonsFor(ReasonFor.Inventory_UnexpectedItem);
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
            Assert.IsInstanceOfType(resolvedInventory, typeof(InventoryDetailDto));

        }

        [TestCleanup]
        public void Cleanup()
        {
            var q1 = new UserQuery();// get this user
            q1.CreateQuery(u => u.UserName, StringConstants.TestingEmailAddress, ComparisonOperator.Equals);
            var result = _userRepo.QueryUsers(q1).Result;
            var testUser = result.FirstOrDefault();

            if (testUser == null)
            {
                Debug.WriteLine("Couldn't return test user - can't clean up afrter Inventory Process tests");
                return;
            }
            var userId = testUser.Id;

            var q = new InventoryQuery(); // get the items made by this user and delete them
            q.CreateQuery(e => e.CreatedByUserId, userId, ComparisonOperator.Equals);
            var inventories = _inventoryRepo.QueryInventories(q).Result;
            foreach (var inventory in inventories)
            {
                _inventoryRepo.Delete(inventory.Id).Wait();
            }

            var itemQuery = new ItemQuery();
            itemQuery.CreateQuery(i=> i.CreatedByUserId, userId, ComparisonOperator.Equals);
            var items = _itemRepo.QueryItems(itemQuery).Result;
            foreach (var item in items)
            {
                _itemRepo.DeleteItem(item.Id).Wait();
            }
        }


        private async Task<AddSnapshotDto> SimulateRealInventorySnapshot(Guid placeId, Guid otherPlaceId)
        {
            var ran = new Random();
            var skus = await _skuRepo.GetAllSkus();
            var sku = skus[ran.Next(skus.Count - 1)];
            var tag1 = new SnapshotTagDto {TagNumber = Guid.NewGuid().ToString(), TagType = TagType.PassiveRfid};
            var tag2 = new SnapshotTagDto { TagNumber = Guid.NewGuid().ToString(), TagType = TagType.PassiveRfid };
            var addItem = new AddItemDto(sku.Id, placeId, tagNumber: tag1.TagNumber);
            await _itemRepo.CreateItem(addItem);
            addItem.PlaceId = otherPlaceId;
            addItem.TagNumber = tag2.TagNumber;
            await _itemRepo.CreateItem(addItem);

            var snapshot = new AddSnapshotDto(placeId)
            {
                Tags = new List<SnapshotTagDto> {tag1, tag2},
                StartTimeUtc = DateTime.UtcNow,
                EndTimeUtc = DateTime.UtcNow
            };
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
