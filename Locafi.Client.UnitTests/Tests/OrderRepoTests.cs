using System.Threading.Tasks;
using Locafi.Client.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Locafi.Client.UnitTests.Tests
{
    [TestClass]
    public class OrderRepoTests : HttpRepoContainer
    {
        private const string BaseUrl = @"http://legacynavapi.azurewebsites.net/api/";
        private const string UserName = "Rian";
        private const string Password = "Ramp11";

        public OrderRepoTests() : base(BaseUrl, UserName, Password)
        {
        }

        [TestMethod]
        public async Task CreateOrderAndCheckExists()
        {
            var places = await PlaceRepo.GetAllPlaces();
            var place1 = places[0];
            var place2 = places[1];
            var persons = await PersonRepo.GetAllPersons();
            var person = persons[0];

            var order = new OrderDto
            {
                SourcePlaceId = place1.Id.ToString(),
                DestinationPlaceId = place2.Id.ToString(),
                DeliverToId = person.Id.ToString()
            };

            var result = await OrderRepo.Create(order);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.DestinationPlaceId,order.DestinationPlaceId);
            Assert.AreEqual(result.SourcePlaceId, order.SourcePlaceId);
            Assert.AreEqual(result.DeliverToId, order.DeliverToId);

        }


    }
}
