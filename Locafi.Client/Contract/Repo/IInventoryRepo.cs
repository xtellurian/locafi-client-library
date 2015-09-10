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
        Task<InventoryDetailDto> CreateInventory(string name, Guid placeId);
        Task<InventoryDetailDto> AddSnapshot(InventorySummaryDto inventory, Guid snapshotId);
        Task<InventoryDetailDto> Resolve(Guid inventoryId, ResolveInventoryDto reasons);
        Task Delete(Guid id);
        Task<IList<InventorySummaryDto>> QueryInventories(IRestQuery<InventorySummaryDto> query);
        Task<InventoryDetailDto> Complete(Guid inventoryId);
    }
}