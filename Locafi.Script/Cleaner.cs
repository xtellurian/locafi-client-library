using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto;
using Locafi.Client.Model.Query;
using Locafi.Client.Model.Query.PropertyComparison;

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
            }
            Console.WriteLine($"Deleted {successCount} {name}(s)");
            Console.WriteLine($"Failed {failedCount} {name}(s)");
        }

        public static async Task CleanItems(Guid userId)
        {
            Console.WriteLine("--- Cleaning Items ---");
            var itemRepo = WebRepoContainer.ItemRepo;
            var query = new ItemQuery();
            query.CreateQuery(i => i.CreatedByUserId, userId, ComparisonOperator.Equals);
            var items = await itemRepo.QueryItems(query);

            await DeleteEntities(items, i => itemRepo.DeleteItem(i.Id), "Item");

        }

        public static async Task CleanPlaces(Guid userId)
        {
            Console.WriteLine("--- Cleaning Places ---");
            var placeRepo = WebRepoContainer.PlaceRepo;
            var query = new PlaceQuery();
            query.CreateQuery(p=>p.CreatedByUserId, userId, ComparisonOperator.Equals);
            var places = await placeRepo.QueryPlaces(query);

            await DeleteEntities(places, p => placeRepo.Delete(p.Id), "place");
        }

        
    }
}
