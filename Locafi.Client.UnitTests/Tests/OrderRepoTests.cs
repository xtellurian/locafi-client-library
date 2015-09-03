using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Contract.Repo;
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
        private IList<Guid> _toCleanup;

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
            var sourcePlace = places[ran.Next(numPlaces - 1)]; // get random places
            var destinationPlace = places[ran.Next(numPlaces - 1)];

            var persons = await _personRepo.GetAllPersons();
            var person = persons[0];

            var refNumber = Guid.NewGuid().ToString();
            var description = Guid.NewGuid().ToString();

            var order = new OrderDto
            {
                DateCreated = DateTime.UtcNow,
                DateLastModified = DateTime.UtcNow,
                SourcePlaceId = sourcePlace.Id.ToString(),
                DestinationPlaceId = destinationPlace.Id.ToString(),
                DeliverToId = person.Id.ToString(),
                ReferenceNumber = refNumber,
                Description = description,
                Status = "Created",

            };

            var result = await _orderRepo.Create(order);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.DestinationPlaceId,order.DestinationPlaceId);
            Assert.AreEqual(result.SourcePlaceId, order.SourcePlaceId);
            Assert.AreEqual(result.DeliverToId, order.DeliverToId);

        }

        [TestMethod]
        public async Task Order_GetAllOrders()
        {
            var orders = await _orderRepo.GetAllOrders();
            Assert.IsNotNull(orders, "OrderRepo.GetAllOrders returned Null");
            Assert.IsInstanceOfType(orders,typeof(IEnumerable<OrderDto>));
        }

        [TestMethod]
        public async Task Order_GetOrderById()
        {
            var orders = await _orderRepo.GetAllOrders();
            Assert.IsNotNull(orders, "OrderRepo.GetAllOrders returned Null");
            Assert.IsInstanceOfType(orders, typeof(IEnumerable<OrderDto>));

            foreach (var order in orders)
            {
                var result = await _orderRepo.GetOrderById(order.Id);
                Assert.IsNotNull(result);
                Assert.AreEqual(order,result);
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            
        }


    }
}
