using System;
using Locafi.Client.Data;
using Locafi.Client.Model.Actions;

namespace Locafi.Client.Model.Extensions
{
    public static class InventoryUri
    {

        public static string AddSnapshotUri(this InventoryDto inventoryDto, Guid snapshotId)
        {
            return $"/{inventoryDto.Id}/AddSnapshot/{snapshotId}";
        }

        public static string CreateUri(this InventoryBaseDto inventoryBaseDto)
        {
            return "Create";
        }

        public static string ResolveUri(this InventoryDto inventoryDto)
        {
            return $"/{inventoryDto.Id}/Resolve";
        }

        public static string CompleteUri(this InventoryDto inventoryDto)
        {
            return $"/{inventoryDto.Id}/Complete";
        }
    }
}
