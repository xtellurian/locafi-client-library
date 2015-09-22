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
            var quantity = ran.Next(10);
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
            var reservation = await _tagReservationRepo.ReserveTagsForSku(sku.Id, quantity);
            var addSnapshot = new AddSnapshotDto(place1.Id);
            foreach (var tag in reservation.TagNumbers)
            {
                addSnapshot.Tags.Add(new SnapshotTagDto(tag));
            }
            var snapshotDetail = await _snapshotRepo.CreateSnapshot(addSnapshot);
            // add snapshot to order
            var responseDto = await _orderRepo.Allocate(detail, snapshotDetail.Id);
            // Assertion that order is successfully allocated
            Assert.IsNotNull(responseDto);
            Assert.IsTrue(responseDto.Success);
            Assert.IsTrue(responseDto.OrderDetail.SourceSnapshotIds.Contains(snapshotDetail.Id));
            Assert.AreEqual(responseDto.OrderDetail.Status, OrderStatus.Allocated);
            // Dispatch
            detail = responseDto.OrderDetail;
            responseDto = await _orderRepo.Dispatch(detail);
            // Assert successful dispatch
            Assert.IsNotNull(responseDto);
            Assert.IsTrue(responseDto.Success);
            Assert.AreEqual(responseDto.OrderDetail.Status, OrderStatus.Dispatched);
        }

        [TestMethod]
        public async Task OrderDispatch_DisputeDispatchSuccess()
        {
            var ran = new Random();
            var quantity = ran.Next(2, 10);
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
            var reservation = await _tagReservationRepo.ReserveTagsForSku(sku.Id, quantity - 1); // underallocate
            var addSnapshot = new AddSnapshotDto(place1.Id);
            foreach (var tag in reservation.TagNumbers)
            {
                addSnapshot.Tags.Add(new SnapshotTagDto(tag));
            }
            var snapshotDetail = await _snapshotRepo.CreateSnapshot(addSnapshot);
            // add snapshot to order - should be partially allocated
            var order = await _orderRepo.Allocate(detail, snapshotDetail.Id);
            // Assertions
            Assert.IsNotNull(order);
            Assert.IsTrue(order.Success);
            Assert.IsTrue(order.OrderDetail.SourceSnapshotIds.Contains(snapshotDetail.Id));
            Assert.AreEqual(order.OrderDetail.Status, OrderStatus.PartiallyAllocated);
            // now lets dispute dispatch
            var reasons = await _reasonRepo.GetReasonsFor(ReasonFor.Order_Allocate); //TODO change to order_dispatch when available
            var disputeDto = new OrderDisputeDto();
            disputeDto.AddSkuItemDispute(sku.Id, reasons.FirstOrDefault().Id);
            var result = await _orderRepo.DisputeDispatch(detail, disputeDto);
            Assert.IsTrue(result.Success, "Order Action Result did not indicate success");
            Assert.AreEqual(result.OrderDetail.Status, OrderStatus.Dispatched);
        }
    }
}
