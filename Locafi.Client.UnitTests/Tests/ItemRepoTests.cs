using System;
using System.Threading.Tasks;
using Locafi.Client.Data;
using Locafi.Client.Model.Dto.Items;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Locafi.Client.UnitTests.Tests
{
    [TestClass]
    public class ItemRepoTests 
    {

        [TestMethod]
        public async Task AddItem()
        {
            var places = await WebRepoContainer.PlaceRepo.GetAllPlaces();
            var place1 = places[0];

            var item = new AddItemDto
            {
                Description = "",
                ItemExtendedPropertyList = null, // null or empty list?
                Name = "Test Item",
                PlaceId = place1.Id,
                SkuId = Guid.Empty,
                TagNumber = "",
                TagType = 0
            };

            var result = await WebRepoContainer.ItemRepo.CreateItem(item);

            Assert.IsNotNull(result);
            Assert.IsTrue(string.Equals(item.Name, result.Name));
            Assert.IsTrue(string.Equals(item.Description, result.Description));
            Assert.AreEqual(item.PlaceId, result.PlaceId);
            Assert.AreEqual(item.SkuId, result.SkuId);
        }


    }
}
