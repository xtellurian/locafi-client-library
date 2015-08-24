using System;
using Locafi.Client.Model.Actions;
using Locafi.Client.Model.Dto.Orders;

namespace Locafi.Client.Model.Uri
{
    // static methods for getting API calls from orders and actions
    public static class OrderUri
    {
        /// <summary>
        /// Returns the relative API path for an action on an Order
        /// </summary>
        /// <param name="order"></param>
        /// <param name="action">The action you want Locafi to perfom</param>
        /// <param name="snapshotId">Snapshot Id for this action</param>
        /// <param name="apiVersion">Api ApiVersion - defaults to latest</param>
        /// <returns>The corresping URI</returns>
        public static string RelativeUri(this OrderDto order, OrderAction action, string snapshotId, ApiVersion apiVersion = ApiVersion.Develop)
        {
            switch (apiVersion)
            {
                case ApiVersion.Develop:
                    return Uri_Develop(order, action, snapshotId);
                default:
                    throw new NotImplementedException($"This operation is not supported in Api Version {apiVersion}");
            }
            
        }

        private static string Uri_Develop(OrderDto order, OrderAction action, string snapshotId)
        {
            if (action == OrderAction.Allocate || action == OrderAction.Receive ||
                action == OrderAction.DisputeAllocate || action == OrderAction.DisputeReceive ||
                action == OrderAction.AdHocReceive || action == OrderAction.AdHocDisputeReceive)
            {
                if (string.IsNullOrEmpty(snapshotId)) throw new NullReferenceException("SnapshotId cannot be null or empty when Allocating or Receiving");
            }

            switch (action)
            {
                case OrderAction.Create:
                    return "/Create";
                case OrderAction.Allocate:
                    return $"/{order.Id}/Allocate/{snapshotId}";
                case OrderAction.DisputeAllocate:
                    throw new NotImplementedException(); // TODO:
                case OrderAction.Dispatch:
                    return $"/{order.Id}/Dispatch";
                case OrderAction.Receive:
                    return $"/{order.Id}/Receive/{snapshotId}";
                case OrderAction.DisputeReceive:
                    throw new NotImplementedException();
                case OrderAction.AdHocReceive:
                    throw new NotImplementedException();
                case OrderAction.AdHocDisputeReceive:
                    throw new NotImplementedException();
                case OrderAction.Cancel:
                    throw new NotImplementedException();
                default:
                    throw new InvalidOperationException("Unknown Action");
            }
        }
    }
}
