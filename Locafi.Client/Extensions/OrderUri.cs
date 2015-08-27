using System;
using Locafi.Client.Model.Dto.Orders;

namespace Locafi.Client.Model.Extensions
{
    // static methods for getting API calls from orders and actions
    public static class OrderUri
    {
        /// <summary>
        /// The relative URI for creating a new Order
        /// </summary>
        /// <param name="orderDto">The Order to be created</param>
        /// <returns>The relative URI ie BASE_URL + SERVICE + THIS </returns>
        public static string CreateUri(this OrderDto orderDto)
        {
            return "Create";
        }

        /// <summary>
        /// The relative URI for allocating a snapshot to an Order
        /// </summary>
        /// <param name="order">The order to allocate to</param>
        /// <param name="snapshotId">The ID of a snapshot *already created*</param>
        /// <returns> The relative URI ie BASE_URL + SERVICE + THIS </returns>
        public static string AllocateUri(this OrderDto order, Guid snapshotId)
        {
            return $"/{order.Id}/Allocate/{snapshotId}";
        }

        /// <summary>
        /// The relative URI for dispatching an Order
        /// </summary>
        /// <param name="order">The order to dispatch</param>
        /// <returns> The relative URI ie BASE_URL + SERVICE + THIS </returns>
        public static string DispatchUri(this OrderDto order)
        {
            return $"/{order.Id}/Dispatch";
        }

        /// <summary>
        /// The relative URI for receiving an Order
        /// </summary>
        /// <param name="order"> The order being received</param>
        /// <param name="snapshotId"> The snapshot to add to the list of received snapshots </param>
        /// <returns> The relative URI ie BASE_URL + SERVICE + THIS </returns>
        public static string ReceiveUri(this OrderDto order, Guid snapshotId)
        {
            return $"/{order.Id}/Receive/{snapshotId}";
        }

    }
}
