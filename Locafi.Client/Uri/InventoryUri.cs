using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Action;
using Locafi.Client.Data;

namespace Locafi.Client.Uri
{
    public static class InventoryUri
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inventoryBase"></param>
        /// <param name="action">Action you want Locafi to perform</param>
        /// <param name="snapshotId">Snapshot Id for this action</param>
        /// <param name="apiVersion">Api Version you are using</param>
        /// <returns></returns>
        public static string RelativeUri(this InventoryBaseDto inventoryBase, InventoryAction action, string snapshotId, ApiVersion apiVersion = ApiVersion.Develop)
        {
            switch (apiVersion)
            {
                case ApiVersion.Develop:
                    return Uri_Develop(inventoryBase, action, snapshotId);
                default:
                    throw new NotImplementedException($"Unknown Api Verison {apiVersion}");
            }
        }

        private static string Uri_Develop(InventoryBaseDto inventoryBase, InventoryAction action, string snapshotId)
        {
            var realInventory = inventoryBase as InventoryDto;
            switch (action)
            {
                case InventoryAction.Create:
                    return "/Create/";
                case InventoryAction.AddSnapshot:
                    if(string.IsNullOrEmpty(snapshotId)) throw new NullReferenceException("Snapshot cannot be null on Add Snapshot");
                    if(realInventory==null) throw new NotSupportedException("Cannot Add Snapshot to non-InventoryDto type");
                    return $"/{realInventory.Id}/AddSnapshot/{snapshotId}";
                case InventoryAction.Resolve:
                    if (realInventory == null) throw new NotSupportedException("Cannot Resolve to non-InventoryDto type");
                    return $"/{realInventory.Id}/Resolve";
                case InventoryAction.Complete:
                    if (realInventory == null) throw new NotSupportedException("Cannot Complete to non-InventoryDto type");
                    return $"/{realInventory.Id}/Complete";
                default:
                    throw new InvalidOperationException("Unknown Action");
            }
        }
    }
}
