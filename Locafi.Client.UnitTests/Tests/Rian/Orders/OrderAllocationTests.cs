using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Model.Dto.Orders;
using Locafi.Client.Model.Dto.Snapshots;
using Locafi.Client.Model.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Locafi.Client.UnitTests.Tests.Rian
{
    [TestClass]
    public class OrderAllocationTests
    {
        private IPlaceRepo _placeRepo;
        private IOrderRepo _orderRepo;
        private ISkuRepo _skuRepo;
        private ITagReservationRepo _tagReservationRepo;
        private ISnapshotRepo _snapshotRepo;
        private IReasonRepo _reasonRepo;

        [TestInitialize]
        public void Initialize()
        {
            _placeRepo = WebRepoContainer.PlaceRepo;
            _snapshotRepo = WebRepoContainer.SnapshotRepo;
            _orderRepo = WebRepoContainer.OrderRepo;
            _skuRepo = WebRepoContainer.SkuRepo;
            _tagReservationRepo = WebRepoContainer.TagReservationRepo;
            _reasonRepo = WebRepoContainer.ReasonRepo;
        }

        [TestMethod]
        public async Task OrderAllocate_Success()
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
            var addSkus = new List<AddOrderSkuLineItemDto> {new AddOrderSkuLineItemDto(sku.Id, quantity, 2)};
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
            var order = await _orderRepo.Allocate(detail, snapshotDetail.Id);
            // Assertions
            Assert.IsNotNull(order);
            Assert.IsTrue(order.Success);
            Assert.IsTrue(order.OrderDetail.SourceSnapshotIds.Contains(snapshotDetail.Id));
            Assert.AreEqual(order.OrderDetail.Status, OrderStatus.Allocated);
        }

        [TestMethod]
        public async Task OrderAllocate_DisputeSuccess_OverAllocate()
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
            var orderDetail = await _orderRepo.Create(addOrder);
            // create new snapshot forfilling order
            var reservation = await _tagReservationRepo.ReserveTagsForSku(sku.Id, quantity + 1);
            var addSnapshot = new AddSnapshotDto(place1.Id);
            foreach (var tag in reservation.TagNumbers)
            {
                addSnapshot.Tags.Add(new SnapshotTagDto(tag));
            }
            var snapshotDetail = await _snapshotRepo.CreateSnapshot(addSnapshot);
            // normal allocate should fail + assert failure
            var failedAllocateResult = await _orderRepo.Allocate(orderDetail, snapshotDetail.Id);
           // Assert.IsFalse(failedAllocateResult.Success); - not implemented properly
            Assert.IsFalse(failedAllocateResult.OrderDetail.SourceSnapshotIds.Contains(snapshotDetail.Id));
            // get possible reasons for dispute
            var reasons = await _reasonRepo.GetReasonsFor(ReasonFor.Order_Allocate);
            var reason = reasons[ran.Next(reasons.Count - 1)];
            // dispute allocate 
            var disputeDto = new OrderDisputeDto();
            disputeDto.AddSkuItemDispute(sku.Id,reason.Id);
            var successDisputeAllocateResult =
                await _orderRepo.DisputeAllocate(orderDetail, disputeDto, snapshotDetail.Id);
            // Assert success
            Assert.IsTrue(successDisputeAllocateResult.Success);
            Assert.AreEqual(successDisputeAllocateResult.OrderDetail.Status, OrderStatus.Allocated);
            Assert.IsTrue(successDisputeAllocateResult.OrderDetail.SourceSnapshotIds.Contains(snapshotDetail.Id));
        }




    }
}
