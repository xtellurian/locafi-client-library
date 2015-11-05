using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Model.Dto.Orders;
using Locafi.Client.Model.Dto.Snapshots;
using Locafi.Client.Model.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Locafi.Client.UnitTests.Tests.Rian.Orders
{
    [TestClass]
    public class OrderDispatchTests
    {
        private IPlaceRepo _placeRepo;
        private ISkuRepo _skuRepo;
        private IOrderRepo _orderRepo;
        private ISnapshotRepo _snapshotRepo;
        private ITagReservationRepo _tagReservationRepo;
        private IReasonRepo _reasonRepo;

        [TestInitialize]
        public void Initialise()
        {
            _placeRepo = WebRepoContainer.PlaceRepo;
            _skuRepo = WebRepoContainer.SkuRepo;
            _orderRepo = WebRepoContainer.OrderRepo;
            _snapshotRepo = WebRepoContainer.SnapshotRepo;
            _tagReservationRepo = WebRepoContainer.TagReservationRepo;
            _reasonRepo = WebRepoContainer.ReasonRepo;
        }
        
        [TestMethod]
        public async Task OrderDispatch_DispatchSuccess()
        { 
            var ran = new Random();
            var quantity = ran.Next(1, 10);
            // create new order
            var refNumber = Guid.NewGuid().ToString();
            string description = Guid.NewGuid().ToString();
            var allPlaces = await _placeRepo.GetAllPlaces();
            var place1 = allPlaces[ran.Next(allPlaces.Count - 1)];
            allPlaces.Remove(place1);
            var place2 = allPlaces[ran.Next(allPlaces.Count - 1)];
            var allSkus = await _skuRepo.GetAllSkus(); // sometimes doesn't work when i pick a sku that cannot be allocated
            var sku = allSkus[ran.Next(allSkus.Count - 1)];
            var addSkus = new List<AddOrderSkuLineItemDto> { new AddOrderSkuLineItemDto(sku.Id, quantity, 2) };
            // some random amoun with 2 packing size`
            var addOrder = new AddOrderDto(refNumber, description, place1.Id, place2.Id, addSkus);
            var detail = await _orderRepo.Create(addOrder);
            // create new snapshot forfilling order
            var result = await _orderRepo.Dispatch(detail);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.OrderDetail.State == OrderState.Receivable);

        }

       
    }
}
