using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Locafi.Client.Contract.Services;
using Locafi.Client.Data;
using Locafi.Client.Model.Dto.Inventory;
using Locafi.Client.Model.Dto.Places;
using Locafi.Client.Model.Dto.Snapshots;
using Locafi.Client.Services.Exceptions;
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
        private IUserRepo _userRepo;
        private IList<Guid> _toCleanup;

        [TestInitialize]
        public void Initialize()
        {
            _inventoryRepo = WebRepoContainer.InventoryRepo;
            _placeRepo = WebRepoContainer.PlaceRepo;
            _snapshotRepo = WebRepoContainer.SnapshotRepo;
            _userRepo = WebRepoContainer.UserRepo;
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
        public async Task Inventory_Create()
        {
            await RandomCreate();
        }

        [TestMethod]
        public async Task Inventory_AddSnapshotSuccess()
        {
            await RandomCreate_AddSnapshot();
        }

        [TestMethod]
        [ExpectedException(typeof(Locafi.Client.Services.Exceptions.InventoryException))]
        public async Task Inventory_AddSnapshotWrongPlace()
        {
            var inventory = await RandomCreate(); // make new
            _toCleanup.Add(inventory.Id); // prepare to cleanup later
            var place = await GetRandomPlace(inventory.PlaceId); // get a place not this palce
            var snapshot = SnapshotGenerator.CreateRandomSnapshotForUpload(place.Id);
            var resultSnapshot = await _snapshotRepo.CreateSnapshot(snapshot);
            Assert.IsNotNull(snapshot);

            var result = await _inventoryRepo.AddSnapshot(inventory, resultSnapshot.Id); // should throw exception

        }

        [TestMethod]
        public async Task Inventory_ResolveSuccess()
        {
            //await RandomCreateAddSnapshot_Resolve();
        }

        [TestMethod]
        public async Task Inventory_Delete()
        {
            var inventory = await RandomCreate(); // create random 
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

        private async Task<InventoryDetailDto> RandomCreateAddSnapshot_Resolve()
        {
            //todo: reasons
            var inventory = await RandomCreate_AddSnapshot();
            
            var result = await _inventoryRepo.Resolve(inventory);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result,typeof(InventoryDetailDto));
           throw new NotImplementedException();
        }

        private async Task<InventoryDetailDto> RandomCreate_AddSnapshot()
        {
            var inventory = await RandomCreate();
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
            return resultInventory;
        }

        private async Task<InventoryDetailDto> RandomCreate()
        {
            var ran = new Random();
            var name = Guid.NewGuid().ToString();
            var places = await _placeRepo.GetAllPlaces();
            var place = places[ran.Next(places.Count - 1)];
            var result = await _inventoryRepo.CreateInventory(name, place.Id);

            Assert.IsInstanceOfType(result, typeof(InventoryDetailDto));
            Assert.IsTrue(string.Equals(result.Name, name));
            Assert.AreEqual(place.Id, result.PlaceId);

            return result;
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
