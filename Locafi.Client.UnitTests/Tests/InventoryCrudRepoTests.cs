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
    public class InventoryCrudRepoTests
    {
        private IInventoryRepo _inventoryRepo;
        private IPlaceRepo _placeRepo;
        private ISnapshotRepo _snapshotRepo;
        private IReasonRepo _reasonRepo;
        private IItemRepo _itemRepo;
        private IUserRepo _userRepo;

        [TestInitialize]
        public void Initialize()
        {
            _inventoryRepo = WebRepoContainer.InventoryRepo;
            _placeRepo = WebRepoContainer.PlaceRepo;
            _snapshotRepo = WebRepoContainer.SnapshotRepo;
            _reasonRepo = WebRepoContainer.ReasonRepo;
            _itemRepo = WebRepoContainer.ItemRepo;
            _userRepo = WebRepoContainer.UserRepo;
        }

        [TestMethod]
        public async Task InventoryCrud_GetAllInventories()
        {
            var inventories = await _inventoryRepo.GetAllInventories();
            Assert.IsNotNull(inventories);
            Assert.IsInstanceOfType(inventories, typeof(IEnumerable<InventorySummaryDto>));
        }

        [TestMethod]
        public async Task InventoryCrud_QueryInventories()
        {
            // Create Inventory for us to Query
            var ran = new Random();
            var name = Guid.NewGuid().ToString();
            var places = await _placeRepo.GetAllPlaces();
            var place = places[ran.Next(places.Count - 1)];
            var inventory = await _inventoryRepo.CreateInventory(name, place.Id);
            Assert.IsNotNull(inventory,"Couldn't create that inventory");

            var query = new InventoryQuery();
            //Query on Name
            query.CreateQuery(q=>q.Name, inventory.Name, ComparisonOperator.Equals);
            var result = await _inventoryRepo.QueryInventories(query);
            Assert.IsNotNull(result, "Result was null on Name query");
            Assert.IsTrue(result.Contains(inventory));

            query.CreateQuery(q => q.PlaceId, inventory.PlaceId, ComparisonOperator.Equals);
            result = await _inventoryRepo.QueryInventories(query);
            Assert.IsNotNull(result, "Result was null on PlaceId query");
            Assert.IsTrue(result.Contains(inventory));
        }

        [TestMethod]
        public async Task InventoryCrud_GetDetail()
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
        public async Task InventoryCrud_Create()
        {
            var ran = new Random();
            var name = Guid.NewGuid().ToString();
            var places = await _placeRepo.GetAllPlaces();
            var place = places[ran.Next(places.Count - 1)];
            var result = await _inventoryRepo.CreateInventory(name, place.Id);

            Assert.IsInstanceOfType(result, typeof (InventoryDetailDto));
            Assert.IsTrue(string.Equals(result.Name, name));
            Assert.AreEqual(place.Id, result.PlaceId);
        }



        [TestMethod]
        public async Task InventoryCrud_Delete()
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
        public void Cleanup()
        {
            var q1 = new UserQuery();// get this user
            q1.CreateQuery(u => u.UserName, StringConstants.TestingUserName, ComparisonOperator.Equals);
            var result = _userRepo.QueryUsers(q1).Result;
            var testUser = result.FirstOrDefault();
            
            var userId = testUser.Id;

            var q = new InventoryQuery(); // get the items made by this user and delete them
            q.CreateQuery(e => e.CreatedByUserId, userId, ComparisonOperator.Equals);
            var inventories = _inventoryRepo.QueryInventories(q).Result;
            foreach (var inventory in inventories)
            {
               _inventoryRepo.Delete(inventory.Id).Wait();
            }
        }

      

    }
}
