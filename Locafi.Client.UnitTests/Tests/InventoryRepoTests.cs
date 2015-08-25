using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Locafi.Client.Contract.Services;
using Locafi.Client.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Locafi.Client.UnitTests.Tests
{
    [TestClass]
    public class InventoryRepoTests
    {
        private IInventoryRepo _inventoryRepo;
        private IPlaceRepo _placeRepo;
        [TestInitialize]
        public void Initialize()
        {
            _inventoryRepo = WebRepoContainer.InventoryRepo;
            _placeRepo = WebRepoContainer.PlaceRepo;
        }

        [TestMethod]
        public async Task GetAllInventories()
        {
            var inventories = await _inventoryRepo.GetAllInventories();
            Assert.IsNotNull(inventories);
            Assert.IsInstanceOfType(inventories, typeof(IEnumerable<InventoryDto>));
        }

        [TestMethod]
        public async Task CreateInventory()
        {
            var ran = new Random();
            var name = Guid.NewGuid().ToString();
            var places = await _placeRepo.GetAllPlaces();
            var place = places[ran.Next(places.Count - 1)];
            var result = await _inventoryRepo.CreateInventory(name, place.Id);

            Assert.IsInstanceOfType(result,typeof(InventoryDto));
            Assert.IsTrue(string.Equals(result.Name,name));
            Assert.AreEqual(place.Id,result.PlaceId);
        }
       


    }
}
