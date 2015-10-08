using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Exceptions;
using Locafi.Client.Model.Dto.Orders;
using Locafi.Client.Model.Query;
using Locafi.Client.Model.Query.PropertyComparison;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Locafi.Client.UnitTests.Tests.Rian
{
    [TestClass]
    public class OrderRepoCrudTests
    {
        private IPlaceRepo _placeRepo;
        private IPersonRepo _personRepo;
        private IOrderRepo _orderRepo;
        private IUserRepo _userRepo;
        private ISkuRepo _skuRepo;

        [TestInitialize]
        public void Initialize()
        {
            _placeRepo = WebRepoContainer.PlaceRepo;
            _personRepo = WebRepoContainer.PersonRepo;
            _orderRepo = WebRepoContainer.OrderRepo;
            _userRepo = WebRepoContainer.UserRepo;
            _skuRepo = WebRepoContainer.SkuRepo;
        }

        [TestMethod]
        public async Task OrderCrud_CreateSuccess()
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
            var skuLineItems = await GenerateSomeSkuLineItems();
            var itemLineItems = await GenerateSomeItemLineItems();
            var addOrder = new AddOrderDto(refNumber, description, sourcePlace.Id, 
                destinationPlace.Id,skuLineItems, itemLineItems, person.Id);

            var result = await _orderRepo.Create(addOrder);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.DestinationPlaceId, addOrder.DestinationPlaceId);
            Assert.AreEqual(result.SourcePlaceId, addOrder.SourcePlaceId);
            Assert.AreEqual(result.DeliverToId, addOrder.DeliverToId);

        }

        private async Task<IList<AddOrderSkuLineItemDto>> GenerateSomeSkuLineItems()
        {
            var ran = new Random();
            var skus = await _skuRepo.GetAllSkus();
            var sku = skus[ran.Next(skus.Count - 1)];
            skus.Remove(sku);
            var sku2 = skus[ran.Next(skus.Count - 1)];

            var result = new List<AddOrderSkuLineItemDto>();
            result.Add(new AddOrderSkuLineItemDto
            {
                PackingSize = 1,
                Quantity = 2,
                SkuId = sku.Id
            });
            result.Add(new AddOrderSkuLineItemDto
            {
                PackingSize = 1,
                Quantity = 2,
                SkuId = sku2.Id
            });
            return result;
        }

        private async Task<IList<AddOrderItemLineItemDto>> GenerateSomeItemLineItems()
        {
            return new List<AddOrderItemLineItemDto>(); //TODO: actually make some
        }

        [TestMethod]
        public async Task OrderCrud_GetAllOrders()
        {
            var orders = await _orderRepo.GetAllOrders();
            Assert.IsNotNull(orders, "OrderRepo.GetAllOrders returned Null");
            Assert.IsInstanceOfType(orders,typeof(IEnumerable<OrderSummaryDto>));
        }

        [TestMethod]
        public async Task OrderCrud_GetOrderById()
        {
            var orders = await _orderRepo.GetAllOrders();
            Assert.IsNotNull(orders, "OrderRepo.GetAllOrders returned Null");

            foreach (var order in orders)
            {
                var result = await _orderRepo.GetOrderById(order.Id);
                Assert.IsNotNull(result);
                Assert.AreEqual(order,result);
            }
        }
  //      [TestMethod]
        public async Task OrderCrud_DeleteOrder()
        {
            // from create success above
            var ran = new Random();
            var places = await _placeRepo.GetAllPlaces();
            var numPlaces = places.Count;
            var sourcePlace = places[ran.Next(numPlaces - 1)]; // get random places
            var destinationPlace = places[ran.Next(numPlaces - 1)];

            var persons = await _personRepo.GetAllPersons();
            var person = persons[0];

            var refNumber = Guid.NewGuid().ToString();
            var description = Guid.NewGuid().ToString();
            var skuLineItems = await GenerateSomeSkuLineItems();
            var itemLineItems = await GenerateSomeItemLineItems();
            var addOrder = new AddOrderDto(refNumber, description, sourcePlace.Id,
                destinationPlace.Id, skuLineItems, itemLineItems, person.Id);

            var order = await _orderRepo.Create(addOrder);
            // use above for deletion
            Assert.IsNotNull(order);
            var success = await _orderRepo.DeleteOrder(order.Id);
            Assert.IsTrue(success);
            var allOrders = await _orderRepo.GetAllOrders();
            Assert.IsFalse(allOrders.Contains(order));
        }


        [TestMethod]
        public async Task OrderCrud_QueryOrders()
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
            var skuLineItems = await GenerateSomeSkuLineItems();
            var itemLineItems = await GenerateSomeItemLineItems();
            var addOrder = new AddOrderDto(refNumber, description, sourcePlace.Id, destinationPlace.Id,
                skuLineItems, itemLineItems, person.Id);
            var order = await _orderRepo.Create(addOrder);
            Assert.IsNotNull(order);

            var query = OrderQuery.NewQuery(o => o.DestinationPlaceId, order.DestinationPlaceId,
                ComparisonOperator.Equals);

            var queryResult = await _orderRepo.QueryOrdersAsync(query);
            Assert.IsNotNull(queryResult, "queryResult != null");
            Assert.IsTrue(queryResult.Entities.Contains(order));
        }

    }
}
