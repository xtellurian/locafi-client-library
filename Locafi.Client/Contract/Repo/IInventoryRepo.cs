// This is a contract file
// Breaking changes to this contract should be accompanied by a version change in the third version number
// ie ... 1.2.42.11 -> 1.2.43.0

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.Inventory;
using Locafi.Client.Model.Query;

namespace Locafi.Client.Contract.Repo
{
    public interface IInventoryRepo
    {
        Task<IList<InventorySummaryDto>> GetAllInventories();
        Task<InventoryDetailDto> GetInventory(Guid id);
        Task<InventoryDetailDto> CreateInventory(Guid placeId, string name = null, Guid? skuGroupId = null);
        Task<InventoryDetailDto> AddSnapshot(InventorySummaryDto inventory, Guid snapshotId);
        Task<InventoryDetailDto> Resolve(Guid inventoryId, ResolveInventoryDto reasons);
        Task<bool> Delete(Guid id);
        [Obsolete]
        Task<IList<InventorySummaryDto>> QueryInventories(IRestQuery<InventorySummaryDto> query);
        Task<InventoryDetailDto> Complete(Guid inventoryId);
        Task<IQueryResult<InventorySummaryDto>> QueryInventoriesAsync(IRestQuery<InventorySummaryDto> query);
    }
}