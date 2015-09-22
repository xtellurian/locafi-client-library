using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto;
using Locafi.Client.Model.Dto.Inventory;
using Locafi.Client.Model.Dto.Items;
using Locafi.Client.Model.Dto.Orders;
using Locafi.Client.Model.Dto.Places;
using Locafi.Client.Model.Dto.Snapshots;
using Locafi.Client.Model.Query;
using Locafi.Client.Model.Query.PropertyComparison;
using Locafi.Script.Terminal;

namespace Locafi.Script
{
    public static class Cleaner
    {
        private static async Task DeleteEntities(IEnumerable<EntityDtoBase> entities, Func<EntityDtoBase, Task<bool>> asyncDeleteAction,
            string name)
        {
            var entityDtoBases = entities as IList<EntityDtoBase> ?? entities.ToList();
            Console.WriteLine($"Deleting {entityDtoBases.Count()} {name}(s)");
            var successCount = 0;
            var failedCount = 0;
            using (var progress = new ProgressBar())
            {
                var total = entityDtoBases.Count;
                var count = 1;
                foreach (var entity in entityDtoBases)
                {
                    try
                    {
                        if (await asyncDeleteAction(entity))
                        {
                            successCount++;
                        }
                        else
                        {
                            failedCount++;
                        }
                    }
                    catch
                    {
                        failedCount++;
                    }
                    count++;
                    progress.Report((double)count / total);
                }
            }
            Console.WriteLine($"Deleted {successCount} {name}(s)");
            Console.WriteLine($"Failed {failedCount} {name}(s)");
            Console.WriteLine("*");

        }

        public static async Task CleanItems(IRestQuery<ItemSummaryDto> query)
        {
            Console.WriteLine("--- Cleaning Items ---");
            var itemRepo = WebRepoContainer.ItemRepo;
            var items = await itemRepo.QueryItems(query);

            await DeleteEntities(items, i => itemRepo.DeleteItem(i.Id), "Item");

        }

        public static async Task CleanPlaces(IRestQuery<PlaceSummaryDto> query)
        {
            Console.WriteLine("--- Cleaning Places ---");
            var placeRepo = WebRepoContainer.PlaceRepo;
            var places = await placeRepo.QueryPlaces(query);

            await DeleteEntities(places, p => placeRepo.Delete(p.Id), "place");
        }

        public static async Task CleanInventories(IRestQuery<InventorySummaryDto> query)
        {
            Console.WriteLine("--- Cleaning Inventories ---");
            var inventoryRepo = WebRepoContainer.InventoryRepo;
            var inventories = await inventoryRepo.QueryInventories(query);

            await DeleteEntities(inventories, p => inventoryRepo.Delete(p.Id), "inventory");
        }

        public static async Task CleanSnapshots(IRestQuery<SnapshotSummaryDto> query)
        {
            Console.WriteLine("--- Cleaning Snapshots ---");
            var snapshotRepo = WebRepoContainer.SnapshotRepo;
            var snapshots = await snapshotRepo.QuerySnapshots(query);

            await DeleteEntities(snapshots, p => snapshotRepo.Delete(p.Id), "snapshot");
        }

        public static async Task CleanOrders(IRestQuery<OrderSummaryDto> query)
        {
            Console.WriteLine("--- Cleaning Orders ---");
            var orderRepo = WebRepoContainer.OrderRepo;
            var orders = await orderRepo.QueryOrders(query);

            await DeleteEntities(orders, o => orderRepo.DeleteOrder(o.Id), "order");
        }
    }
}
