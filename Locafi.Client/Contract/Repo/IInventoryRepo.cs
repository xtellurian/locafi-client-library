// This is a contract file
// Breaking changes to this contract should be accompanied by a version change in the third version number
// ie ... 1.2.42.11 -> 1.2.43.0

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.Inventory;
using Locafi.Client.Model.Query;
using Locafi.Client.Model;
using Locafi.Client.Model.Dto.Snapshots;

namespace Locafi.Client.Contract.Repo
{
    public interface IInventoryRepo
    {
        Task<PageResult<InventorySummaryDto>> QueryInventories(string oDataQueryOptions = null);
        Task<PageResult<InventorySummaryDto>> QueryInventories(IRestQuery<InventorySummaryDto> query);
        Task<IQueryResult<InventorySummaryDto>> QueryInventoriesWithContinuation(IRestQuery<InventorySummaryDto> query);
        Task<InventoryDetailDto> GetInventory(Guid id);
        Task<InventoryDetailDto> CreateInventory(Guid placeId, string name = null, Guid? skuGroupId = null, List<Guid> SkuIds = null);
        Task<InventoryDetailDto> CreateInventory(AddInventoryDto addDto);
        Task<AddInventorySnapshotResultDto> AddSnapshot(Guid inventoryId, AddSnapshotDto snapshot);
        Task<InventoryDetailDto> Resolve(ResolveInventoryDto resolvedDto);
        Task<bool> Delete(Guid id);
        //Task<InventoryDetailDto> Complete(Guid inventoryId);
        //Task<InventoryDetailDto> AddItem(InventorySummaryDto inventory, Guid itemId);
    }
}