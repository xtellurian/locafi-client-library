using System.Threading.Tasks;
using Locafi.Client.Model.Dto.Orders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Locafi.Client.UnitTests.Tests
{
    [TestClass]
    public class OrderRepoTests 
    {
        [TestMethod]
        public async Task CreateOrderAndCheckExists()
        {
            var places = await WebRepoContainer.PlaceRepo.GetAllPlaces();
            var place1 = places[0];
            var place2 = places[1];
            var persons = await WebRepoContainer.PersonRepo.GetAllPersons();
            var person = persons[0];

            var order = new OrderDto
            {
                SourcePlaceId = place1.Id.ToString(),
                DestinationPlaceId = place2.Id.ToString(),
                DeliverToId = person.Id.ToString()
            };

            var result = await WebRepoContainer.OrderRepo.Create(order);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.DestinationPlaceId,order.DestinationPlaceId);
            Assert.AreEqual(result.SourcePlaceId, order.SourcePlaceId);
            Assert.AreEqual(result.DeliverToId, order.DeliverToId);

        }


    }
}
