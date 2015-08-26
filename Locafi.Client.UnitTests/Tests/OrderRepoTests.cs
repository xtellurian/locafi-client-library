using System;
using System.Threading.Tasks;
using Locafi.Client.Contract.Services;
using Locafi.Client.Model.Dto.Orders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Locafi.Client.UnitTests.Tests
{
    [TestClass]
    public class OrderRepoTests
    {
        private IPlaceRepo _placeRepo;
        private IPersonRepo _personRepo;
        private IOrderRepo _orderRepo;

        [TestInitialize]
        public void Initialize()
        {
            _placeRepo = WebRepoContainer.PlaceRepo;
            _personRepo = WebRepoContainer.PersonRepo;
            _orderRepo = WebRepoContainer.OrderRepo;
        }

        [TestMethod]
        public async Task Order_CreateSuccess()
        {
            var ran = new Random();
            var places = await _placeRepo.GetAllPlaces();
            var numPlaces = places.Count;
            var place1 = places[ran.Next(numPlaces - 1)]; // get random places
            var place2 = places[ran.Next(numPlaces - 1)];

            var persons = await _personRepo.GetAllPersons();
            var person = persons[0];

            var order = new OrderDto
            {
                SourcePlaceId = place1.Id.ToString(),
                DestinationPlaceId = place2.Id.ToString(),
                DeliverToId = person.Id.ToString()
            };

            var result = await _orderRepo.Create(order);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.DestinationPlaceId,order.DestinationPlaceId);
            Assert.AreEqual(result.SourcePlaceId, order.SourcePlaceId);
            Assert.AreEqual(result.DeliverToId, order.DeliverToId);

        }


    }
}
