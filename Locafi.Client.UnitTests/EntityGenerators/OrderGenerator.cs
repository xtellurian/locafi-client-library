using Locafi.Client.Model.Dto.Orders;
using Locafi.Client.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.UnitTests.EntityGenerators
{
    public static class OrderGenerator
    {
        public static AddOrderDto CreateAddOrderDto(OrderType orderType, Dictionary<Guid, int> skusToUse, List<Guid> itemsToUse, Guid? fromPlaceId = null, Guid? toPlaceId = null, Guid? personId = null, Guid? customerId = null)
        {
            var ran = new Random(DateTime.UtcNow.Millisecond);

            switch (orderType)
            {
                case OrderType.Inbound:
                    if (toPlaceId == null)
                        toPlaceId = WebRepoContainer.Place2Id;
                    break;
                case OrderType.Outbound:
                    if (fromPlaceId == null)
                        fromPlaceId = WebRepoContainer.Place2Id;
                    break;
                case OrderType.Transfer:
                    if (toPlaceId == null || fromPlaceId == null)
                    {
                        toPlaceId = WebRepoContainer.Place2Id;
                        fromPlaceId = WebRepoContainer.Place1Id;
                    }
                    break;
                case OrderType.Return:
                    throw new NotImplementedException("Return orders not supported yet");
                    break;
                case OrderType.Loan:
                    throw new NotImplementedException("Loans not supported yet");
                    break;
            }

            var refNumber = Guid.NewGuid().ToString();
            var description = Guid.NewGuid().ToString();
            var skuLineItems = GenerateSkuLineItems(skusToUse);
            var itemLineItems = GenerateItemLineItems(itemsToUse);
            var addOrderDto = new AddOrderDto(orderType, refNumber, description, fromPlaceId,
                toPlaceId, skuLineItems, itemLineItems, personId, customerId);

            return addOrderDto;
        }

        private static IList<AddOrderSkuDto> GenerateSkuLineItems(Dictionary<Guid, int> skusToUse)
        {
            var result = new List<AddOrderSkuDto>();

            if (skusToUse != null)
            {
                foreach (var skuLine in skusToUse)
                {
                    result.Add(new AddOrderSkuDto
                    {
                        RequiredCount = skuLine.Value,
                        SkuId = skuLine.Key
                    });
                }
            }

            return result;
        }

        private static IList<AddOrderUniqueItemDto> GenerateItemLineItems(List<Guid> itemsToUse)
        {
            var result = new List<AddOrderUniqueItemDto>();

            if (itemsToUse != null)
            {
                foreach (var itemLine in itemsToUse)
                {
                    result.Add(new AddOrderUniqueItemDto
                    {
                        ItemId = itemLine
                    });
                }
            }

            return result;
        }

        public static async Task<OrderDetailDto> ToOrderDetailDto(AddOrderDto dto)
        {
            var detail = new OrderDetailDto()
            {
                CustomerOrderNumber = dto.CustomerOrderNumber,
                Comments = dto.Comments,
                ToPlaceId = dto.ToPlaceId,
                FromPlaceId = dto.FromPlaceId,
                CustomerId = dto.CustomerId,
                OrderType = dto.OrderType,
                OrderState = OrderStateType.Created,
                DeliverToPersonId = dto.DeliverToPersonId
            };

            foreach (var sku in dto.OrderSkus)
            {
                detail.OrderSkuList.Add(new ReadOrderSkuDto(await WebRepoContainer.SkuRepo.GetSku(sku.SkuId)) { RequiredCount = sku.RequiredCount });
            }

            foreach (var item in dto.OrderUniqueItems)
            {
                detail.OrderItemList.Add(new ReadOrderItemDto(await WebRepoContainer.ItemRepo.GetItemDetail(item.ItemId)));
            }

            return detail;
        }
    }
}
