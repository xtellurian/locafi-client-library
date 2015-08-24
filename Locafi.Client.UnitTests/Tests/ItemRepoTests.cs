using System;
using System.Threading.Tasks;
using Locafi.Client.Contract.Services;
using Locafi.Client.Data;
using Locafi.Client.Model.Dto.Items;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Locafi.Client.UnitTests.Tests
{
    [TestClass]
    public class ItemRepoTests
    {
        private IPlaceRepo _placeRepo;
        private IItemRepo _itemRepo;
        [TestInitialize]
        public void Setup()
        {
            _placeRepo = WebRepoContainer.PlaceRepo;
            _itemRepo = WebRepoContainer.ItemRepo;
        }

        [TestMethod]
        public async Task AddItem()
        {
            var ran = new Random();
            var places = await _placeRepo.GetAllPlaces();
            var place = places[ran.Next(places.Count - 1)]; // picks a random place for the item

            var addItemDto = new AddItemDto
            {
                Description = "",
                ItemExtendedPropertyList = null, // null or empty list?
                Name = "Test Item",
                PlaceId = place.Id,
                SkuId = Guid.Empty,
                TagNumber = "",
                TagType = 0
            };

            var result = await _itemRepo.CreateItem(addItemDto);

            Assert.IsNotNull(result);
            Assert.IsTrue(string.Equals(addItemDto.Name, result.Name));
            Assert.IsTrue(string.Equals(addItemDto.Description, result.Description));
            Assert.AreEqual(addItemDto.PlaceId, result.PlaceId);
            Assert.AreEqual(addItemDto.SkuId, result.SkuId);
        }




    }
}
