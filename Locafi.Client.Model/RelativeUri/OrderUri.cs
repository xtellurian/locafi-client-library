using System;
using Locafi.Client.Model.Dto.Orders;

namespace Locafi.Client.Model.RelativeUri
{
    // static methods for getting API calls from orders and actions
    public static class OrderUri
    {
        public static string ServiceName => "Orders";
        public static string Create => "CreateOrder";
        public static string GetOrders => "GetFilteredOrders";
        public static string AddSnapshotToOrder => "AddSnapshot";

        public static string GetOrder(Guid id)
        {
            return $"GetOrder/{id}";
        }
        public static string Allocate(Guid id)
        {
            return $"AllocateOrder/{id}";
        }

        public static string RevertToAllocating(Guid id)
        {
            return $"RevertOrderToAllocating/{id}";
        }

        public static string Dispatch(Guid id)
        {
            return $"DispatchOrder/{id}";
        }

        public static string Complete(Guid id)
        {
            return $"CompleteOrder/{id}";
        }

        public static string Receive(Guid id)
        {
            return $"ReceiveOrder/{id}";
        }

        public static string RevertToReceiving(Guid id)
        {
            return $"RevertOrderToReceiving/{id}";
        }

        public static string ClearSnapshots(OrderSummaryDto orderSummary, Guid placeId)
        {
            return $"{GetOrder(orderSummary.Id)}/ClearTags/{placeId}";
        }

        public static string Cancel(OrderSummaryDto orderSummary)
        {
            return $"{GetOrder(orderSummary.Id)}/Cancel";
        }

        public static string Delete(Guid id)
        {
            return $"DeleteOrder/{id}";
        }

        public static string GetPrintInfo(Guid id)
        {
            return $"{GetOrder(id)}/GetSkuPrintInfo";
        }

        public static string Cancel(Guid id)
        {
            return $"CancelOrder/{id}";
        }
    }
}
