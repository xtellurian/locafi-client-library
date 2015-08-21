using System;
using System.Threading.Tasks;
using Locafi.Client.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Locafi.Client.UnitTests.Tests
{
    [TestClass]
    public class InventoryRepoTests : HttpRepoContainer
    {
        private const string BaseUrl = @"http://legacynavapi.azurewebsites.net/api/";
        private const string UserName = "Rian";
        private const string Password = "Ramp11";
        public InventoryRepoTests() : base(BaseUrl, UserName, Password)
        {
        }

        [TestMethod]
        public async Task AddItem()
        {
            var places = await PlaceRepo.GetAllPlaces();
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

            var result = await ItemRepo.AddItem(item);

            Assert.IsNotNull(result);
        }


    }
}
