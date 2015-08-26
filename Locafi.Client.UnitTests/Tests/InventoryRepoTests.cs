using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Locafi.Client.Contract.Services;
using Locafi.Client.Data;
using Locafi.Client.Model.Dto.Places;
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

        [TestInitialize]
        public void Initialize()
        {
            _inventoryRepo = WebRepoContainer.InventoryRepo;
            _placeRepo = WebRepoContainer.PlaceRepo;
            _snapshotRepo = WebRepoContainer.SnapshotRepo;
            _userRepo = WebRepoContainer.UserRepo;
        }

        [TestMethod]
        public async Task Inventory_GetAllInventories()
        {
            var inventories = await _inventoryRepo.GetAllInventories();
            Assert.IsNotNull(inventories);
            Assert.IsInstanceOfType(inventories, typeof(IEnumerable<InventoryDto>));
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
        public async Task Inventory_ResolveSuccess()
        {
            //await RandomCreateAddSnapshot_Resolve();
        }

        private async Task<InventoryDto> RandomCreateAddSnapshot_Resolve()
        {
            //todo: reasons
            var inventory = await RandomCreate_AddSnapshot();
            
            var result = await _inventoryRepo.Resolve(inventory);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result,typeof(InventoryDto));
           throw new NotImplementedException();
        }

        private async Task<InventoryDto> RandomCreate_AddSnapshot()
        {
            var inventory = await RandomCreate();
            var user = await GetRandomUser();
            var localSnapshot = SnapshotGenerator.CreateRandomSnapshot(inventory.PlaceId.ToString(), user.Id.ToString());
            var resultSnapshot = await _snapshotRepo.CreateSnapshot(localSnapshot);
            Assert.IsNotNull(resultSnapshot);
            Assert.IsInstanceOfType(resultSnapshot, typeof(SnapshotDto));

            Assert.IsNotNull(inventory);
            Assert.IsInstanceOfType(inventory, typeof(InventoryDto));
            var resultInventory = await _inventoryRepo.AddSnapshot(inventory, resultSnapshot.Id);

            Assert.IsNotNull(resultInventory);
            Assert.IsInstanceOfType(resultInventory, typeof(InventoryDto));
            Assert.IsTrue(resultInventory.SnapshotIds.Contains(resultSnapshot.Id.ToString()));
            return resultInventory;
        }

        private async Task<InventoryDto> RandomCreate()
        {
            var ran = new Random();
            var name = Guid.NewGuid().ToString();
            var places = await _placeRepo.GetAllPlaces();
            var place = places[ran.Next(places.Count - 1)];
            var result = await _inventoryRepo.CreateInventory(name, place.Id);

            Assert.IsInstanceOfType(result, typeof(InventoryDto));
            Assert.IsTrue(string.Equals(result.Name, name));
            Assert.AreEqual(place.Id, result.PlaceId);
            return result;
        }

        private async Task<PlaceSummaryDto> GetRandomPlace()
        {
            var ran = new Random();
            var allPlaces = await _placeRepo.GetAllPlaces();
            var place = allPlaces[ran.Next(allPlaces.Count - 1)];
            return place;
        }

        private async Task<UserDto> GetRandomUser()
        {
            var ran = new Random();
            var allUsers = await _userRepo.GetAllUsers();
            var user = allUsers[ran.Next(allUsers.Count - 1)];
            return user;
        }

    }
}
