using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Data;

namespace Locafi.Client.Contract.Services
{
    public interface IInventoryRepo
    {
        Task<IList<InventoryDto>> GetAllInventories();
        Task<InventoryDto> GetInventory(string id);
        Task<InventoryDto> CreateInventory(string name, Guid placeId);
        Task<InventoryDto> AddSnapshot(InventoryDto inventory, Guid snapshotId);
        Task<InventoryDto> Resolve(InventoryDto inventory);
    }
}