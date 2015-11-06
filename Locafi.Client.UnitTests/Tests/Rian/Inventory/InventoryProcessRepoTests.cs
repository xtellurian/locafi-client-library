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
using Locafi.Client.Model.Dto.Skus;
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
        private ITagReservationRepo _tagReservationRepo;

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
            _tagReservationRepo = WebRepoContainer.TagReservationRepo;
        }
        [TestMethod]
        public async Task InventoryProcess_AddRandomSnapshotSuccess()
        {
            var ran = new Random();
            var name = Guid.NewGuid().ToString();
            var places = await _placeRepo.GetAllPlaces();
            var place = places[ran.Next(places.Count - 1)];
//            var inventory = await _inventoryRepo.CreateInventory(name, place.Id);
            var inventory = await _inventoryRepo.CreateInventory(name, new Guid("00000000-0000-0000-0000-000000060556"));

            var localSnapshot = SnapshotGenerator.CreateRandomSnapshotForUpload(inventory.PlaceId, 100);
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
        public async Task InventoryProcess_AddNewGtinSnapshotSuccess()
        {
            var ran = new Random();
            var name = Guid.NewGuid().ToString();
            var places = await _placeRepo.GetAllPlaces();
            var place = places[ran.Next(places.Count - 1)];
            var inventory = await _inventoryRepo.CreateInventory(name, place.Id);
//            var inventory = await _inventoryRepo.CreateInventory(name, new Guid("00000000-0000-0000-0000-000000060556"));


            var localSnapshot = await SnapshotGenerator.CreateNewGtinSnapshotForUpload(inventory.PlaceId,5000);
            var t1 = DateTime.UtcNow;
            var resultSnapshot = await _snapshotRepo.CreateSnapshot(localSnapshot);
            Assert.IsNotNull(resultSnapshot);
            Assert.IsInstanceOfType(resultSnapshot, typeof(SnapshotDetailDto));

            // add the snapshot
            Assert.IsNotNull(inventory);
            Assert.IsInstanceOfType(inventory, typeof(InventoryDetailDto));
            var resultInventory = await _inventoryRepo.AddSnapshot(inventory, resultSnapshot.Id);

            Assert.IsNotNull(resultInventory);
            Assert.IsInstanceOfType(resultInventory, typeof(InventoryDetailDto));
            Assert.IsTrue(resultInventory.SnapshotIds.Contains(resultSnapshot.Id));

            // resolve the inventory
            _reasonRepo = WebRepoContainer.ReasonRepo;
            var reasons = await _reasonRepo.GetAllReasons();
            var unknownReason = reasons.Where(r => r.Name == "Unknown").FirstOrDefault();
            var resolveInventoryDto = new ResolveInventoryDto();
            foreach (var item in resultInventory.MissingItems)
            {
                resolveInventoryDto.Reasons.Add(item, unknownReason.Id);
            }
            foreach (var item in resultInventory.FoundItemsUnexpected)
            {
                resolveInventoryDto.Reasons.Add(item, unknownReason.Id);
            }
            var resolveResult = await _inventoryRepo.Resolve(inventory.Id, resolveInventoryDto);
            Assert.IsNotNull(resolveResult);
            Assert.IsInstanceOfType(resolveResult, typeof(InventoryDetailDto));

            // complete the inventory
            var completeResult = await _inventoryRepo.Complete(inventory.Id);
            Assert.IsNotNull(completeResult);
            Assert.IsInstanceOfType(completeResult, typeof(InventoryDetailDto));
            Assert.IsTrue(completeResult.Complete);
            var t2 = DateTime.UtcNow;
            var span = t2 - t1;
        }

        [TestMethod]
        public async Task InventoryProcess_AddExistingGtinSnapshotSuccess()
        {
            // create the inventory
            var ran = new Random();
            var name = Guid.NewGuid().ToString();
            var places = await _placeRepo.GetAllPlaces();
            var place = places[ran.Next(places.Count - 1)];
            var inventory = await _inventoryRepo.CreateInventory(name, place.Id);
            //var inventory = await _inventoryRepo.CreateInventory(name, new Guid("00000000-0000-0000-0000-000000060556"));

            // create a snapshot
            var localSnapshot = await SnapshotGenerator.CreateExistingGtinSnapshotForUpload(inventory.PlaceId, 5000);
            var t1 = DateTime.UtcNow;
            var resultSnapshot = await _snapshotRepo.CreateSnapshot(localSnapshot);
            Assert.IsNotNull(resultSnapshot);
            Assert.IsInstanceOfType(resultSnapshot, typeof(SnapshotDetailDto));

            // add the snapshot
            Assert.IsNotNull(inventory);
            Assert.IsInstanceOfType(inventory, typeof(InventoryDetailDto));
            var resultInventory = await _inventoryRepo.AddSnapshot(inventory, resultSnapshot.Id);

            Assert.IsNotNull(resultInventory);
            Assert.IsInstanceOfType(resultInventory, typeof(InventoryDetailDto));
            Assert.IsTrue(resultInventory.SnapshotIds.Contains(resultSnapshot.Id));

            // resolve the inventory
            var reasons = await _reasonRepo.GetAllReasons();
            var unknownReason = reasons.Where(r => r.Name == "Unknown").FirstOrDefault();
            var resolveInventoryDto = new ResolveInventoryDto();
            foreach(var item in resultInventory.MissingItems)
            {
                resolveInventoryDto.Reasons.Add(item, unknownReason.Id);
            }
            foreach(var item in resultInventory.FoundItemsUnexpected)
            {
                resolveInventoryDto.Reasons.Add(item, unknownReason.Id);
            }
            var resolveResult = await _inventoryRepo.Resolve(inventory.Id, resolveInventoryDto);
            Assert.IsNotNull(resolveResult);
            Assert.IsInstanceOfType(resolveResult, typeof(InventoryDetailDto));

            // complete the inventory
            var completeResult = await _inventoryRepo.Complete(inventory.Id);
            Assert.IsNotNull(completeResult);
            Assert.IsInstanceOfType(completeResult, typeof(InventoryDetailDto));
            Assert.IsTrue(completeResult.Complete);
            var t2 = DateTime.UtcNow;
            var span = t2 - t1;
        }

        [TestMethod]
        public async Task InventoryProcess_LoadTestSnapshot()
        {
            var ran = new Random();
            var name = "Load test " + Guid.NewGuid();
            var places = await _placeRepo.GetAllPlaces();
            var place = places[ran.Next(places.Count - 1)];
            var inventory = await _inventoryRepo.CreateInventory(name, place.Id);

            
            var localSnapshot = SnapshotGenerator.CreateRandomSnapshotForUpload(inventory.PlaceId, 2000);
            var startTime = DateTime.Now;
            var resultSnapshot = await _snapshotRepo.CreateSnapshot(localSnapshot);
            Debug.WriteLine("Snap upload: " + (DateTime.Now - startTime).TotalMilliseconds + "ms");
            Assert.IsNotNull(resultSnapshot);
            Assert.IsInstanceOfType(resultSnapshot, typeof(SnapshotDetailDto));

            Assert.IsNotNull(inventory);
            Assert.IsInstanceOfType(inventory, typeof(InventoryDetailDto));
            startTime = DateTime.Now;
            var resultInventory = await _inventoryRepo.AddSnapshot(inventory, resultSnapshot.Id);
            Debug.WriteLine("Add Snap 2 Inv: " + (DateTime.Now - startTime).TotalMilliseconds + "ms");
            Assert.IsNotNull(resultInventory);
            Assert.IsInstanceOfType(resultInventory, typeof(InventoryDetailDto));
            Assert.IsTrue(resultInventory.SnapshotIds.Contains(resultSnapshot.Id));
        }
  //      [TestMethod]
        public async Task InventoryProcess_UploadingNewSkuTagsInSnapshot()
        {
            var ran = new Random();
            var numRealItems = ran.Next(100);
            var place = await GetRandomPlace(Guid.Empty);
            // pick a (valid) sku
            var sku = await GetSkuWithValidCompanyAndItem();
            // reserve some tag numbers 
            var reservedTags = await _tagReservationRepo.ReserveTagsForSku(sku.Id, numRealItems);
            var addSnapshot = new AddSnapshotDto(place.Id);
            foreach (var tagNumber in reservedTags.TagNumbers)
            {
                addSnapshot.Tags.Add(new SnapshotTagDto {TagNumber = tagNumber, TagType = TagType.PassiveRfid});
            }
            // create some random tag numbers
            addSnapshot = AddRandomTagsToSnapshot(addSnapshot, ran.Next(100));
            // create and send up snapshot
            var resultSnap = await _snapshotRepo.CreateSnapshot(addSnapshot);
            Assert.IsNotNull(resultSnap, "Result snapshot was null");
            // assert we made the right number of items
            Assert.AreEqual(numRealItems, resultSnap.Items.Count, "Incorrect number of items created");
            // verify all new items in snapshot return
            foreach (var itemId in resultSnap.Items)
            {
                var item = await _itemRepo.GetItemDetail(itemId);
                Assert.IsTrue(reservedTags.TagNumbers.Contains(item.TagNumber));
            }
        }

        private async Task<SkuDetailDto> GetSkuWithValidCompanyAndItem()
        {
            var ran = new Random();
            var skus = await _skuRepo.GetAllSkus();
            var count = 0;
            SkuDetailDto result = null;
            while (true)
            {
                count++;
                var skuSummary = skus[ran.Next(skus.Count - 1)];
                var detail = await _skuRepo.GetSkuDetail(skuSummary.Id);
                if (!string.IsNullOrEmpty(detail.CompanyPrefix) && !string.IsNullOrEmpty(detail.ItemReference))
                {
                    result = detail;
                    break;
                }
                if (count > 100) break; // probably won't find a valid sku
            }
            return result;
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

            var localSnapshot = await SimulateRealInventorySnapshot(place.Id, otherPlace.Id); //TODO: fix this method
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
            Assert.IsInstanceOfType(resolvedInventory, typeof (InventoryDetailDto));

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

        private AddSnapshotDto AddRandomTagsToSnapshot(AddSnapshotDto snapshot, int number)
        {
            for (var i = 0; i < number; i++)
            {
                snapshot.Tags.Add(new SnapshotTagDto {TagNumber = Guid.NewGuid().ToString(),TagType = TagType.PassiveRfid});
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
