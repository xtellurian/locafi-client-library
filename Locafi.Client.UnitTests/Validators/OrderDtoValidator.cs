using Locafi.Client.Model.Dto.CycleCountDtos;
using Locafi.Client.Model.Dto.Inventory;
using Locafi.Client.Model.Dto.Orders;
using Locafi.Client.Model.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Locafi.Client.UnitTests.Validators
{
    public static class OrderDtoValidator
    {
        public static void OrderSummarycheck(OrderSummaryDto dto, OrderStateType? expectedState = null)
        {
            BaseDtoValidator.DtoBaseCheck(dto);

            try {
                switch(dto.OrderType)
                {
                    case OrderType.Inbound:
                        Assert.IsNotNull(dto.ToPlaceId);
                        BaseDtoValidator.CheckNullableGuid(dto.ToPlaceId, "OrderSummarycheck: ToPlaceId == null/Empty");
                        Assert.IsTrue(dto.FromPlaceId == null || dto.FromPlaceId == Guid.Empty, "OrderSummarycheck: FromPlaceId != null/Empty");
                        Assert.IsTrue(dto.CustomerId == null || dto.CustomerId == Guid.Empty, "OrderSummarycheck: CustomerId != null/Empty");
                        break;
                    case OrderType.Outbound:
                        Assert.IsNotNull(dto.FromPlaceId);
                        BaseDtoValidator.CheckNullableGuid(dto.FromPlaceId, "OrderSummarycheck: FromPlaceId == null/Empty");
                        Assert.IsTrue(dto.ToPlaceId == null || dto.ToPlaceId == Guid.Empty, "OrderSummarycheck: ToPlaceId != null/Empty");
                        break;
                    case OrderType.Transfer:
                        Assert.IsNotNull(dto.ToPlaceId);
                        Assert.IsNotNull(dto.FromPlaceId);
                        BaseDtoValidator.CheckNullableGuid(dto.ToPlaceId, "OrderSummarycheck: ToPlaceId == null/Empty");
                        BaseDtoValidator.CheckNullableGuid(dto.FromPlaceId, "OrderSummarycheck: FromPlaceId == null/Empty");
                        Assert.IsTrue(dto.CustomerId == null || dto.CustomerId == Guid.Empty, "OrderSummarycheck: CustomerId != null/Empty");
                        Assert.IsTrue(dto.ToPlaceId != dto.FromPlaceId);
                        break;
                    case OrderType.Return:
                        throw new NotImplementedException("Return orders not supported yet");
                        break;
                    case OrderType.Loan:
                        throw new NotImplementedException("Loans not supported yet");
                        break;
                }
                
                BaseDtoValidator.CheckString(dto.CustomerOrderNumber, "OrderSummarycheck: CustomerOrderNumber == null/Empty");

                if(expectedState != null)
                    Assert.AreEqual(dto.OrderState, expectedState, "OrderSummarycheck: incorrect OrderState state (Expected:" + expectedState.ToString() + ", Actual: " + dto.OrderState.ToString() + ")");
            }
            catch (Exception e)
            {
                var msg = "Error Message: " + e.Message + "\r\n\r\nStack Trace: " + e.StackTrace;
                MessageBox.Show(msg, "Validation Error");
                throw e;
            }
        }

        public static void OrderDetailcheck(OrderDetailDto dto, OrderStateType? expectedState = null)
        {
            OrderSummarycheck(dto, expectedState);

            try {
                Assert.IsNotNull(dto.OrderSkuList, "OrderDetailcheck: OrderSkuList == null");
                Assert.IsNotNull(dto.OrderItemList, "OrderDetailcheck: OrderItemList == null");
            }
            catch (Exception e)
            {
                var msg = "Error Message: " + e.Message + "\r\n\r\nStack Trace: " + e.StackTrace;
                MessageBox.Show(msg, "Validation Error");
                throw e;
            }
        }

        public static void OrderCreateComparison(OrderDetailDto dto, AddOrderDto addDto)
        {
            try { 
                Assert.AreEqual(dto.ToPlaceId, addDto.ToPlaceId);
                Assert.AreEqual(dto.FromPlaceId, addDto.FromPlaceId);
                Assert.AreEqual(dto.DeliverToPersonId, addDto.DeliverToPersonId);
                Assert.AreEqual(dto.CustomerId, addDto.CustomerId);
                Assert.AreEqual(dto.CustomerOrderNumber, addDto.CustomerOrderNumber);
                Assert.AreEqual(dto.Comments, addDto.Comments);

                Assert.AreEqual(dto.OrderSkuList.Count, addDto.OrderSkus.Count);
                Assert.AreEqual(dto.OrderItemList.Count, addDto.OrderUniqueItems.Count);

                // check each sku line
                foreach (var skuLine in addDto.OrderSkus)
                {
                    var testVal = dto.OrderSkuList.Where(s => s.Id == skuLine.SkuId).FirstOrDefault();
                    Assert.IsNotNull(testVal);
                    Assert.AreEqual(skuLine.RequiredCount, testVal.RequiredCount);
                }

                // check each item line
                foreach (var itemLine in addDto.OrderUniqueItems)
                {
                    var testVal = dto.OrderItemList.Where(i => i.Id == itemLine.ItemId).FirstOrDefault();
                    Assert.IsNotNull(testVal);
                }
            }
            catch (Exception e)
            {
                var msg = "Error Message: " + e.Message + "\r\n\r\nStack Trace: " + e.StackTrace;
                MessageBox.Show(msg, "Validation Error");
                throw e;
            }
        }

        public static void OrderDetailComparison(OrderDetailDto dto, OrderDetailDto dto2)
        {
            try
            {
                Assert.AreEqual(dto.ToPlaceId, dto2.ToPlaceId);
                Assert.AreEqual(dto.FromPlaceId, dto2.FromPlaceId);
                Assert.AreEqual(dto.DeliverToPersonId, dto2.DeliverToPersonId);
                Assert.AreEqual(dto.CustomerId, dto2.CustomerId);
                Assert.AreEqual(dto.CustomerOrderNumber, dto2.CustomerOrderNumber);
                Assert.AreEqual(dto.Comments, dto2.Comments);

                Assert.AreEqual(dto.OrderSkuList.Count, dto2.OrderSkuList.Count);
                Assert.AreEqual(dto.OrderItemList.Count, dto2.OrderItemList.Count);

                // check each sku line
                foreach (var skuLine in dto2.OrderSkuList)
                {
                    var testVal = dto.OrderSkuList.Where(s => s.Id == skuLine.Id).FirstOrDefault();
                    Assert.IsNotNull(testVal);
                    Assert.AreEqual(skuLine.RequiredCount, testVal.RequiredCount);
                }

                // check each item line
                foreach (var itemLine in dto2.OrderItemList)
                {
                    var testVal = dto.OrderItemList.Where(i => i.Id == itemLine.Id).FirstOrDefault();
                    Assert.IsNotNull(testVal);
                }
            }
            catch (Exception e)
            {
                var msg = "Error Message: " + e.Message + "\r\n\r\nStack Trace: " + e.StackTrace;
                MessageBox.Show(msg, "Validation Error");
                throw e;
            }
        }
    }
}
