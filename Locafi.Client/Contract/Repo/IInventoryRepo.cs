using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.Inventory;

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
    }
}