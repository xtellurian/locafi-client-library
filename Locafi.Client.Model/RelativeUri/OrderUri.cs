using System;
using Locafi.Client.Model.Dto.Orders;

namespace Locafi.Client.Model.RelativeUri
{
    // static methods for getting API calls from orders and actions
    public static class OrderUri
    {
        public static string ServiceName => "Orders";
        public static string Create => "CreateOrder";
        public static string GetOrders => "GetOrders";

        public static string GetOrder(Guid id)
        {
            return $"GetOrder/{id}";
        }
        public static string Allocate(OrderSummaryDto orderDetail, Guid snapshotId)
        {
            return $"{GetOrder(orderDetail.Id)}/Allocate/{snapshotId}";
        }

        public static string Dispatch(OrderSummaryDto orderSummary)
        {
            return $"/{GetOrder(orderSummary.Id)}/Dispatch";
        }

        public static string Complete(OrderSummaryDto orderSummary)
        {
            return $"/{GetOrder(orderSummary.Id)}/Complete";
        }

        public static string DisputeDispatch(OrderSummaryDto orderSummary)
        {
            return $"/{GetOrder(orderSummary.Id)}/DisputeDispatch";
        }

        public static string DisputeAllocate(OrderSummaryDto orderSummary, Guid snapshotId)
        {
            return $"{GetOrder(orderSummary.Id)}/DisputeAllocate/{snapshotId}";
        }


        public static string Receive(OrderSummaryDto orderSummary, Guid snapshotId)
        {
            return $"{GetOrder(orderSummary.Id)}/Receive/{snapshotId}";
        }

        public static string DisputeReceive(OrderSummaryDto orderSummary, Guid snapshotId)
        {
            return $"/{orderSummary.Id}/DisputeReceive/{snapshotId}";
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
            return $"{GetOrder(id)}/GetPrintInfo";
        }

    }
}
