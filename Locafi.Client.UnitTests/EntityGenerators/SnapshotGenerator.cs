using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.Snapshots;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Model.Dto.Items;
using Locafi.Client.Model.Query.PropertyComparison;
using Locafi.Client.Model.Query;
using Locafi.Client.Model.Dto.Skus;
using Locafi.Client.Model.Dto.Orders;
using Locafi.Client.Model.Query.Builder;

namespace Locafi.Client.UnitTests.EntityGenerators
{
    public static class SnapshotGenerator
    {
        public static AddSnapshotDto CreateRandomSnapshotForUpload(Guid placeId, int number = -1)
        {
            var ran = new Random(DateTime.UtcNow.Millisecond);
            var count = number <= 0 ? ran.Next(50) : number;
            var name = Guid.NewGuid().ToString();
            var tags = GenerateRandomTags(count);
            var snap = new AddSnapshotDto()
            {
                StartTime = DateTime.UtcNow.Subtract(new TimeSpan(0, 1, 0)),
                EndTime = DateTime.UtcNow,
                Tags = tags
            };
            return snap;
        }

        public static async Task<AddSnapshotDto> CreateNewGtinSnapshotForUpload(Guid placeId, int totalTagNumber = -1, int randomTagNumber = -1, Guid? skuId = null)
        {
            ISkuRepo _skuRepo = WebRepoContainer.SkuRepo;
            SkuSummaryDto sku;
            if (skuId == null)
            {
                var skus = await _skuRepo.QuerySkus();
                // find a sku with a gtin
                sku = skus.Where(s => !string.IsNullOrEmpty(s.CompanyPrefix) && !string.IsNullOrEmpty(s.ItemReference)).FirstOrDefault();
                if (sku == null)
                    return null;
            }
            else
            {
                sku = await _skuRepo.GetSku((Guid)skuId);
            }

            var ran = new Random(DateTime.UtcNow.Millisecond);
            var totalCount = totalTagNumber <= 0 ? ran.Next(50) : totalTagNumber;
            var randomCount = randomTagNumber <= 0 ? 0 : randomTagNumber;
            var name = Guid.NewGuid().ToString();
            var tags = await GenerateGtinTags(sku.Id,totalCount - randomCount);
            tags = tags.Concat(GenerateRandomTags(randomCount)).ToList();
            var snap = new AddSnapshotDto()
            {
                StartTime = DateTime.UtcNow.Subtract(new TimeSpan(0, 1, 0)),
                EndTime = DateTime.UtcNow,
                Tags = tags
            };
            return snap;
        }

        public static async Task<AddSnapshotDto> CreateExistingGtinSnapshotForUpload(Guid placeId, int totalTagNumber = -1, int randomTagNumber = -1, Guid? skuId = null)
        {
            ISkuRepo _skuRepo = WebRepoContainer.SkuRepo;
            SkuSummaryDto sku;
            if (skuId == null)
            {
                var skus = await _skuRepo.QuerySkus();
                // find a sku with a gtin
                sku = skus.Where(s => !string.IsNullOrEmpty(s.CompanyPrefix) && !string.IsNullOrEmpty(s.ItemReference)).FirstOrDefault();
                if (sku == null)
                    return null;
            }else
            {
                sku = await _skuRepo.GetSku((Guid)skuId);
            }

            IItemRepo _itemRepo = WebRepoContainer.ItemRepo;
            
            var ran = new Random(DateTime.UtcNow.Millisecond);
            var totalCount = totalTagNumber <= 0 ? ran.Next(50) : totalTagNumber;
            var randomCount = randomTagNumber <= 0 ? 0 : randomTagNumber;
            var name = Guid.NewGuid().ToString();

            var q2 = QueryBuilder<ItemSummaryDto>.NewQuery(i => i.SkuId, sku.Id, ComparisonOperator.Equals).Take(0).Build();
            int availableItems = (int)(await _itemRepo.QueryItems(q2)).Count;

            var q1 = new ItemQuery();
            q1.CreateQuery(i => i.SkuId, sku.Id, ComparisonOperator.Equals, availableItems > totalCount ? totalCount - randomCount : availableItems, availableItems > totalCount ? ran.Next(availableItems - totalCount - randomCount) : 0);
            var q3 = QueryBuilder<ItemSummaryDto>.NewQuery(i => i.SkuId, sku.Id, ComparisonOperator.Equals)
                .And(i => i.TagNumber, null, ComparisonOperator.NotEquals)
                .And(i => i.TagNumber, "", ComparisonOperator.NotEquals)
                .Take(availableItems > totalCount ? totalCount - randomCount : availableItems)
                .Skip(availableItems > totalCount ? ran.Next(availableItems - totalCount - randomCount) : 0)
                .Build()
                ;
            var items = await _itemRepo.QueryItemsContinuation(q3);

            IList<SnapshotTagDto> tags = new List<SnapshotTagDto>();
            foreach(var item in items.Entities)
            {
                tags.Add(new SnapshotTagDto
                {
                    TagNumber = item.TagNumber
                });
            }

            // generate some new tags if there aren't enough already exisiting tags for this sku
            tags = tags.Concat(await GenerateGtinTags(sku.Id, totalCount - randomCount - items.Entities.Count)).ToList();
            tags = tags.Concat(GenerateRandomTags(randomCount)).ToList();
            var snap = new AddSnapshotDto()
            {
                StartTime = DateTime.UtcNow.Subtract(new TimeSpan(0, 1, 0)),
                EndTime = DateTime.UtcNow,
                Tags = tags
            };
            return snap;
        }

        public static async Task<AddSnapshotDto> CreateExistingItemSnapshotForUpload(Guid placeId, List<Guid> itemIds)
        {
            IItemRepo _itemRepo = WebRepoContainer.ItemRepo;
            var name = Guid.NewGuid().ToString();

            ItemDetailDto item;
            IList<SnapshotTagDto> tags = new List<SnapshotTagDto>();
            foreach (var id in itemIds)
            {
                item = await _itemRepo.GetItemDetail(id);
                tags.Add(new SnapshotTagDto
                {
                    TagNumber = item.TagNumber
                });
            }

            var snap = new AddSnapshotDto()
            {
                StartTime = DateTime.UtcNow.Subtract(new TimeSpan(0, 1, 0)),
                EndTime = DateTime.UtcNow,
                Tags = tags
            };
            return snap;
        }

        public static AddSnapshotDto CreateSnapshotForUpload(Guid placeId, List<string> tasgNumbers)
        {
            var name = Guid.NewGuid().ToString();

            IList<SnapshotTagDto> tags = new List<SnapshotTagDto>();
            foreach (var tagNumber in tasgNumbers)
            {
                tags.Add(new SnapshotTagDto
                {
                    TagNumber = tagNumber
                });
            }

            var snap = new AddSnapshotDto()
            {
                StartTime = DateTime.UtcNow.Subtract(new TimeSpan(0, 1, 0)),
                EndTime = DateTime.UtcNow,
                Tags = tags
            };
            return snap;
        }

        private static IList<SnapshotTagDto> GenerateRandomTags(int number)
        {

            var tags = new List<SnapshotTagDto>();
            for (var n = 0; n < number; n++)
            {
                tags.Add(GenerateRandomTag());
            }
            return tags;
        }

        private static async Task<IList<SnapshotTagDto>> GenerateGtinTags(Guid id, int number)
        {
            ITagReservationRepo _tagReservationRepo = WebRepoContainer.TagReservationRepo;
            var tags = new List<SnapshotTagDto>();

            var rand = new Random(DateTime.UtcNow.Millisecond);

            var chunkSize = 1000;
            for (var pos = 0; pos < number; pos += chunkSize)
            {
                var chunk = (pos + chunkSize) < number ? chunkSize : number - pos;
                var tagReservation = await _tagReservationRepo.ReserveTagsForSku(id, chunk);

                foreach (var tagNo in tagReservation.TagNumbers)
                {
                    tags.Add(new SnapshotTagDto(tagNo, rand.Next(1, 100), rand.Next(-700, -300) / 10.0));
                }
            }
            return tags;
        }

        private static SnapshotTagDto GenerateRandomTag()
        {
            var rand = new Random(DateTime.Now.Millisecond);
            return new SnapshotTagDto(Guid.NewGuid().ToString(),rand.Next(1,100),rand.Next(-70,-30));
        }

        public static SnapshotTagDto GenerateTag(string tagNumber)
        {
            var rand = new Random(DateTime.UtcNow.Millisecond);
            return new SnapshotTagDto(tagNumber, rand.Next(1, 100), rand.Next(-700, -300) / 10.0);
        }

        public static async Task<AddSnapshotDto> GenerateSgtinSnapshot(IDictionary<Guid, int> skusToCreate, List<string> existingTagsToInclude)
        {
            // create the dto
            var addDto = new AddSnapshotDto()
            {
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow
            };

            // create the new sgtin tags
            if (skusToCreate != null)
            {
                foreach (var kv in skusToCreate)
                {
                    var newTags = await GenerateGtinTags(kv.Key, kv.Value);
                    foreach (var tag in newTags)
                        addDto.Tags.Add(tag);
                }
            }

            // add the existing tags
            if (existingTagsToInclude != null)
            {
                foreach (var tagNum in existingTagsToInclude)
                {
                    addDto.Tags.Add(GenerateTag(tagNum));
                }
            }

            return addDto;
        }
    }
}
