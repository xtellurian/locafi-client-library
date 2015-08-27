using System;
using Locafi.Client.Data;
using Locafi.Client.Model.Actions;

namespace Locafi.Client.Model.Extensions
{
    public static class InventoryUri
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inventoryBaseDto"> The details of the </param>
        /// <returns> The relative URI ie BASE_URL + SERVICE + THIS </returns>
        public static string CreateUri(this InventoryBaseDto inventoryBaseDto)
        {
            return "Create";
        }

        /// <summary>
        /// The relative URI for Adding a Snapshot to an Inventory
        /// </summary>
        /// <param name="inventoryDto"> The Inventory to which you are adding the Snapshot </param>
        /// <param name="snapshotId"> The Id of the Snapshot, which must be already created </param>
        /// <returns> The relative URI ie BASE_URL + SERVICE + THIS </returns>
        public static string AddSnapshotUri(this InventoryDto inventoryDto, Guid snapshotId)
        {
            return $"/{inventoryDto.Id}/AddSnapshot/{snapshotId}";
        }


        /// <summary>
        /// The relative URI for Resolving an Inventory by uploading a set of reasons
        /// </summary>
        /// <param name="inventoryDto">The Inventory to Resolve</param>
        /// <returns> The relative URI ie BASE_URL + SERVICE + THIS </returns>
        public static string ResolveUri(this InventoryDto inventoryDto)
        {
            return $"/{inventoryDto.Id}/Resolve";
        }

        /// <summary>
        /// The relative URI for Completing an Inventory
        /// </summary>
        /// <param name="inventoryDto"> The Inventory to Complete</param>
        /// <returns> The relative URI ie BASE_URL + SERVICE + THIS </returns>
        public static string CompleteUri(this InventoryDto inventoryDto)
        {
            return $"/{inventoryDto.Id}/Complete";
        }
    }
}
