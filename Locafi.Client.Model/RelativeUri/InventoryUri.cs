using System;
using Locafi.Client.Model.Dto.Inventory;

namespace Locafi.Client.Model.RelativeUri
{
    public static class InventoryUri
    {
        public static string ServiceName => "Inventories";
        public static string GetInventories => "GetFilteredInventories";
        public static string CreateInventory => "CreateInventory";


        public static string GetInventory(Guid id)
        {
            return $"GetInventory/{id}";
        }

        /// <summary>
        /// The relative URI for Adding a Snapshot to an Inventory
        /// </summary>
        /// <param name="inventoryDto"> The Inventory to which you are adding the Snapshot </param>
        /// <param name="snapshotId"> The Id of the Snapshot, which must be already created </param>
        /// <returns> The relative URI ie BASE_URL + SERVICE + THIS </returns>
        public static string AddSnapshot => "AddInventorySnapshot";

        //public static string AddItem(InventorySummaryDto inventoryDto, Guid itemId)
        //{
        //    return $"/{inventoryDto.Id}/AddItem/{itemId}";
        //}

        /// <summary>
        /// The relative URI for Resolving an Inventory by uploading a set of reasons
        /// </summary>
        /// <param name="inventoryId">The ID of the Inventory you wish to resolve</param>
        /// <returns> The relative URI ie BASE_URL + SERVICE + THIS </returns>
        public static string Resolve => "ResolveInventory";

        //public static string Complete(Guid inventoryId)
        //{
        //    return $"/{inventoryId}/Complete";
        //}

        //public static string Delete(Guid id)
        //{
        //    return $"DeleteInventory/{id}";
        //}
    }
}
