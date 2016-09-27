using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Model.Dto.Skus;
using Locafi.Client.Model.Enums;
using Locafi.Client.Model.Query;
using Locafi.Client.Model.Query.PropertyComparison;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Locafi.Client.UnitTests.Implementations;
using Locafi.Client.Processors.Encoding;
using Locafi.Client.UnitTests.Validators;
using Locafi.Client.Model;
using Locafi.Client.Model.Query.Builder;
using Locafi.Client.UnitTests.EntityGenerators;
using Locafi.Client.UnitTests.Extensions;
using Locafi.Client.Model.Dto.CycleCountDtos;
using Locafi.Client.Model.Dto.Items;
using Locafi.Client.Model.Dto.Inventory;

namespace Locafi.Client.UnitTests.Tests
{
    [TestClass]
    public class SkuRepoTests
    {
        private ISkuRepo _skuRepo;
        private ITemplateRepo _templateRepo;
        private IPlaceRepo _placeRepo;
        private ICycleCountRepo _cycleCountRepo;
        private IItemRepo _itemRepo;
        private IInventoryRepo _inventoryRepo;
        private IList<Guid> _skusToDelete;
        private IList<Guid> _placesToDelete;
        private IList<string> _tagNumbersToDelete;
        private List<Guid> _itemsToDelete;

        [TestInitialize]
        public void Initialize()
        {
            _skuRepo = WebRepoContainer.SkuRepo;
            _templateRepo = WebRepoContainer.TemplateRepo;
            _placeRepo = WebRepoContainer.PlaceRepo;
            _cycleCountRepo = WebRepoContainer.CycleCountRepo;
            _itemRepo = WebRepoContainer.ItemRepo;
            _inventoryRepo = WebRepoContainer.InventoryRepo;
            _skusToDelete = new List<Guid>();
            _placesToDelete = new List<Guid>();
            _tagNumbersToDelete = new List<string>();
            _itemsToDelete = new List<Guid>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            // delete all items that were created
            foreach (var itemId in _itemsToDelete)
            {
                _itemRepo.DeleteItem(itemId).Wait();
            }

            // delete items created from tags
            if (_tagNumbersToDelete.Count > 0)
            {
                var query = QueryBuilder<ItemSummaryDto>.NewQuery(i => i.TagNumber, string.Join(",", _tagNumbersToDelete), ComparisonOperator.ContainedIn).Build();
                var itemQuery = _itemRepo.QueryItems(query).Result;
                foreach (var item in itemQuery.Items)
                {
                    if (item != null)
                    {
                        _itemRepo.DeleteItem(item.Id).Wait();
                    }
                }
            }

            // delete all skus that were created
            foreach (var Id in _skusToDelete)
            {
                 _skuRepo.DeleteSku(Id).Wait();
            }

            // delete all places
            foreach(var Id in _placesToDelete)
            {
                _placeRepo.Delete(Id).Wait();
            }
        }

        [TestMethod]
        public async Task Sku_Create()
        {
            // create a sku
            var addDto = await SkuGenerator.GenerateSgtinSkuDto();
            var result = await _skuRepo.CreateSku(addDto);
            _skusToDelete.AddUnique(result.Id);   // store to deletae later

            // check result
            SkuDtoValidator.SkuDetailCheck(result, null, true);
            
            Validator.IsTrue(string.Equals(result.ItemReference, addDto.ItemReference));
            Validator.IsTrue(string.Equals(result.CompanyPrefix, addDto.CompanyPrefix));
        }

        [TestMethod]
        public async Task Sku_GetAll()
        {
            // create a sku so there is at least 1 to query
            var addDto = await SkuGenerator.GenerateSgtinSkuDto();
            var result = await _skuRepo.CreateSku(addDto);
            _skusToDelete.AddUnique(result.Id);   // store to deletae later

            // check result
            SkuDtoValidator.SkuDetailCheck(result, null, true);

            // get the skus
            var skus = await _skuRepo.QuerySkus();

            // check the result
            Validator.IsNotNull(skus);
            Validator.IsInstanceOfType(skus, typeof(PageResult<SkuSummaryDto>));
            Validator.IsTrue(skus.Count > 0);
        }

        [TestMethod]
        public async Task Sku_Query()
        {
            // create a sku
            var addDto = await SkuGenerator.GenerateSgtinSkuDto();
            var sku = await _skuRepo.CreateSku(addDto);
            _skusToDelete.AddUnique(sku.Id);   // store to deletae later

            // check result
            SkuDtoValidator.SkuDetailCheck(sku, null, true);

            // query for sku
            var query = new SkuQuery();
            query.CreateQuery(s => s.Name, sku.Name, ComparisonOperator.Equals);
            var result = await _skuRepo.QuerySkus(query);

            // check the result
            Validator.IsNotNull(result);
            Validator.IsTrue(result.Items.Contains(sku));

            // query for all skus of this template
            query.CreateQuery(s => s.TemplateId, sku.TemplateId, ComparisonOperator.Equals);
            result = await _skuRepo.QuerySkus(query);

            // check the result
            Validator.IsNotNull(result);
            Validator.IsTrue(result.Items.Contains(sku));

            // query for sgtin skus
            var query1 = QueryBuilder<SkuSummaryDto>.NewQuery(s => s.CompanyPrefix, "", ComparisonOperator.NotEquals)
                .And(s => s.ItemReference, "", ComparisonOperator.NotEquals)
                .Build();
            result = await _skuRepo.QuerySkus(query1);

            // check the result
            Validator.IsNotNull(result);
            Validator.IsTrue(result.Items.Contains(sku));

        }

        [TestMethod]
        public async Task Sku_GetDetails()
        {
            // create a sku
            var addDto = await SkuGenerator.GenerateSgtinSkuDto();
            var sku = await _skuRepo.CreateSku(addDto);
            _skusToDelete.AddUnique(sku.Id);   // store to delete later

            // check result
            SkuDtoValidator.SkuDetailCheck(sku, null, true);

            // now get the details back
            var skuDetails = await _skuRepo.GetSku(sku.Id);

            // check result
            SkuDtoValidator.SkuDetailCheck(skuDetails, null, true);
            Validator.AreEqual(sku, skuDetails);
        }

        [TestMethod]
        public async Task Sku_Update()
        {
            // create a sku
            var addDto = await SkuGenerator.GenerateSgtinSkuDto();
            var sku = await _skuRepo.CreateSku(addDto);
            _skusToDelete.AddUnique(sku.Id);   // store to delete later

            // check result
            SkuDtoValidator.SkuDetailCheck(sku, null, true);

            // now update the sku (don't change template, or ext props)
            var ran = new Random(DateTime.UtcNow.Millisecond);
            var skuNo = ran.Next().ToString();
            var companyPrefix = ran.Next(9999).ToString().PadLeft(4);
            var itemReference = ran.Next(9999).ToString().PadLeft(4);
            var name = "Sku - Update - " + ran.Next().ToString();
            var description = name + " - Description";
            var updateDto1 = new UpdateSkuDto(sku)
            {
                Name = name,
                Description = description,
                SkuNumber = skuNo,
                CompanyPrefix = companyPrefix,
                ItemReference = itemReference
            };
            var update1 = await _skuRepo.UpdateSku(updateDto1);

            // check result
            SkuDtoValidator.SkuDetailCheck(update1, null, true);
            Validator.AreNotEqual(sku.Name, update1.Name);
            Validator.AreNotEqual(sku.Description, update1.Description);
            Validator.AreNotEqual(sku.SkuNumber, update1.SkuNumber);
            Validator.AreEqual(update1.Id,updateDto1.Id);
            Validator.AreEqual(update1.CompanyPrefix, updateDto1.CompanyPrefix);
            Validator.AreEqual(update1.ItemReference, updateDto1.ItemReference);
            Validator.AreEqual(update1.Description, updateDto1.Description);
            Validator.AreEqual(update1.Name, updateDto1.Name);
            Validator.AreEqual(update1.SkuNumber, updateDto1.SkuNumber);
            Validator.AreEqual(update1.TemplateId, updateDto1.TemplateId);

            // now update the sku and change the template (must change ext props)
            var templates = await _templateRepo.GetTemplatesForType(TemplateFor.Item);
            var newTemplate = templates.Where(t => t.Id != sku.TemplateId).FirstOrDefault();
            var newTemplateDetail = await _templateRepo.GetById(newTemplate.Id);
            var updateDto2 = new UpdateSkuDto(update1);
            updateDto2.ChangeTemplate(newTemplateDetail);
            var update2 = await _skuRepo.UpdateSku(updateDto2);

            // check result
            SkuDtoValidator.SkuDetailCheck(update2, null, true);
            Validator.AreNotEqual(sku.Name, update1.Name);
            Validator.AreNotEqual(sku.Description, update1.Description);
            Validator.AreNotEqual(sku.SkuNumber, update1.SkuNumber);
            Validator.AreEqual(update2.Id, updateDto2.Id);
            Validator.AreEqual(update2.CompanyPrefix, updateDto2.CompanyPrefix);
            Validator.AreEqual(update2.ItemReference, updateDto2.ItemReference);
            Validator.AreEqual(update2.Description, updateDto2.Description);
            Validator.AreEqual(update2.Name, updateDto2.Name);
            Validator.AreEqual(update2.SkuNumber, updateDto2.SkuNumber);
            Validator.AreEqual(update2.TemplateId, updateDto2.TemplateId);
            // check we have the right extended properties
            foreach(var prop in newTemplateDetail.TemplateExtendedPropertyList)
            {
                Validator.IsTrue(update2.SkuExtendedPropertyList.Select(s => s.ExtendedPropertyId).Contains(prop.ExtendedPropertyId));
                Validator.AreEqual(update2.SkuExtendedPropertyList.Where(s => s.ExtendedPropertyId == prop.ExtendedPropertyId).Select(s => s.ExtendedPropertyName).FirstOrDefault(), prop.ExtendedPropertyName);
            }
        }

        [TestMethod]
        public async Task Sku_Delete()
        {
            // create a sku
            var addDto = await SkuGenerator.GenerateSgtinSkuDto();
            var sku = await _skuRepo.CreateSku(addDto);
            _skusToDelete.AddUnique(sku.Id);   // store to delete later

            // check result
            SkuDtoValidator.SkuDetailCheck(sku, null, true);

            // now delete the sku
            var deleteResult = await _skuRepo.DeleteSku(sku.Id);

            // check result
            Validator.IsTrue(deleteResult);
            // remove from delete list
            _skusToDelete.Remove(sku.Id);

            // verify
            var queryResult = await _skuRepo.QuerySkus(QueryBuilder<SkuSummaryDto>.NewQuery(s => s.Id, sku.Id, ComparisonOperator.Equals).Build());
            Validator.IsTrue(queryResult.Items.Count() == 0);

            // verify with get
            try
            {
                var sameItem = await _skuRepo.GetSku(sku.Id);

                Validator.IsTrue(false, "Deleted entity returned");
            }
            catch (Exception e)
            {
                // this is expected                
            }
        }

        [TestMethod]
        public async Task Sku_GetStockCount_SgtinOnly()
        {
            int numSgtinItemsPerSku = 10;

            // create place
            var addPlace = await PlaceGenerator.GenerateRandomAddPlaceDto();
            var place = await _placeRepo.CreatePlace(addPlace);
            _placesToDelete.AddUnique(place.Id);

            // create some sgtin skus
            var skus = new List<SkuDetailDto>();
            for (int i = 0; i < 2; i++)
            {
                var addSkuDto = await SkuGenerator.GenerateSgtinSkuDto();
                var sku = await _skuRepo.CreateSku(addSkuDto);

                skus.Add(sku);  // add to list

                _skusToDelete.AddUnique(sku.Id);   // store to delete later
            }

            // now create some items from these sku's by doing a cycle count on this location
            // create the cycle count
            var addDto = new AddCycleCountDto() { PlaceId = place.Id };
            var cycleDto = await _cycleCountRepo.CreateCycleCount(addDto);
            // build snapshot to resolve
            var addSnapshotDto = await SnapshotGenerator.GenerateSgtinSnapshot(skus.Select(s => s.Id).ToDictionary(k => k, k => numSgtinItemsPerSku), null);
            _tagNumbersToDelete.AddRangeUnique(addSnapshotDto.Tags.Select(t => t.TagNumber));

            // resolve the snapshot
            var resolveDto = new ResolveCycleCountDto(addSnapshotDto)
            {
                CycleCountId = cycleDto.Id
            };
            var responseDto = await _cycleCountRepo.ResolveCycleCount(resolveDto);
            cycleDto = responseDto.CycleCountDto;
            // check no notifications
            Validator.IsTrue(responseDto.Notifications.Count <= 0, "Notifications in response");

            // now query the stock count for this place
            var query = QueryBuilder<SkuStockCountDto>.NewQuery(s => s.PlaceId,place.Id,ComparisonOperator.Equals).Build();
            var stockCounts = await _skuRepo.GetSkuStockCount(query);

            // check the response
            Validator.IsInstanceOfType(stockCounts, typeof(PageResult<SkuStockCountDto>));
            Validator.IsTrue(stockCounts.Items.Count() == skus.Count);
            foreach (var sku in skus)
            {
                var stockCount = stockCounts.First(s => s.SkuId == sku.Id);
                SkuDtoValidator.SkuStockCountCheck(stockCount, true);
                Validator.IsTrue(stockCount.ItemCount == numSgtinItemsPerSku);
            }

            // now do another cycle count int he same location with a new set of tags + the existing
            // create the cycle count
            var addDto2 = new AddCycleCountDto() { PlaceId = place.Id };
            var cycleDto2 = await _cycleCountRepo.CreateCycleCount(addDto2);
            // build snapshot to resolve
            var addSnapshotDto2 = await SnapshotGenerator.GenerateSgtinSnapshot(skus.Select(s => s.Id).ToDictionary(k => k, k => numSgtinItemsPerSku), addSnapshotDto.Tags.Select(t => t.TagNumber).ToList());
            _tagNumbersToDelete.AddRangeUnique(addSnapshotDto2.Tags.Select(t => t.TagNumber));

            // resolve the snapshot
            var resolveDto2 = new ResolveCycleCountDto(addSnapshotDto2)
            {
                CycleCountId = cycleDto2.Id
            };
            responseDto = await _cycleCountRepo.ResolveCycleCount(resolveDto2);
            cycleDto2 = responseDto.CycleCountDto;
            // check no notifications
            Validator.IsTrue(responseDto.Notifications.Count <= 0, "Notifications in response");

            // now query the stock count for this place
            query = QueryBuilder<SkuStockCountDto>.NewQuery(s => s.PlaceId, place.Id, ComparisonOperator.Equals).Build();
            stockCounts = await _skuRepo.GetSkuStockCount(query);

            // check the response
            Validator.IsInstanceOfType(stockCounts, typeof(PageResult<SkuStockCountDto>));
            Validator.IsTrue(stockCounts.Items.Count() == skus.Count);    // all items should show as present
            foreach (var sku in skus)
            {
                foreach (var stockCount in stockCounts.Where(s => s.SkuId == sku.Id))
                {
                    SkuDtoValidator.SkuStockCountCheck(stockCount, true);
                    Validator.IsTrue(stockCount.ItemCount == (numSgtinItemsPerSku * 2));
                }

                var skuSum = stockCounts.Where(s => s.SkuId == sku.Id).Sum(s => s.ItemCount);
                Validator.IsTrue(skuSum == (numSgtinItemsPerSku * 2));
            }

            // now do another cycle count in the same location with a new set of tags + the first set so we will have 2*numSgtinItemsPerSku created, should not get missing
            // create the cycle count
            var addDto3 = new AddCycleCountDto() { PlaceId = place.Id };
            var cycleDto3 = await _cycleCountRepo.CreateCycleCount(addDto3);
            // build snapshot to resolve
            var addSnapshotDto3 = await SnapshotGenerator.GenerateSgtinSnapshot(skus.Select(s => s.Id).ToDictionary(k => k, k => numSgtinItemsPerSku), addSnapshotDto.Tags.Select(t => t.TagNumber).ToList());
            _tagNumbersToDelete.AddRangeUnique(addSnapshotDto3.Tags.Select(t => t.TagNumber));

            // resolve the snapshot
            var resolveDto3 = new ResolveCycleCountDto(addSnapshotDto3)
            {
                CycleCountId = cycleDto3.Id
            };
            responseDto = await _cycleCountRepo.ResolveCycleCount(resolveDto3);
            cycleDto3 = responseDto.CycleCountDto;
            // check no notifications
            Validator.IsTrue(responseDto.Notifications.Count <= 0, "Notifications in response");

            // now query the stock count for this place
            query = QueryBuilder<SkuStockCountDto>.NewQuery(s => s.PlaceId, place.Id, ComparisonOperator.Equals).Build();
            stockCounts = await _skuRepo.GetSkuStockCount(query);

            // check the response
            Validator.IsInstanceOfType(stockCounts, typeof(PageResult<SkuStockCountDto>));
            Validator.IsTrue(stockCounts.Items.Count() == skus.Count);    // all items should show as present
            foreach (var sku in skus)
            {
                foreach (var stockCount in stockCounts.Where(s => s.SkuId == sku.Id))
                {
                    SkuDtoValidator.SkuStockCountCheck(stockCount, true);
                    Validator.IsTrue(stockCount.ItemStatus != ItemStateType.Missing);
                    Validator.IsTrue(stockCount.ItemCount == (3 * numSgtinItemsPerSku));
                }

                var skuSum = stockCounts.Where(s => s.SkuId == sku.Id).Sum(s => s.ItemCount);
                Validator.IsTrue(skuSum == (numSgtinItemsPerSku * 3));
            }

            // now clear all the items from this place
            var cleared = await _itemRepo.ClearItems(new ClearItemStateDto() { PlaceId = place.Id });

            // now query the stock count for this place
            query = QueryBuilder<SkuStockCountDto>.NewQuery(s => s.PlaceId, place.Id, ComparisonOperator.Equals).Build();
            stockCounts = await _skuRepo.GetSkuStockCount(query);

            // check the response
            Validator.IsInstanceOfType(stockCounts, typeof(PageResult<SkuStockCountDto>));
            Validator.IsTrue(stockCounts.Items.Count() == skus.Count);  // all should be cleared
            foreach (var sku in skus)
            {
                foreach (var stockCount in stockCounts.Where(s => s.SkuId == sku.Id))
                {
                    SkuDtoValidator.SkuStockCountCheck(stockCount, true);
                    Validator.IsTrue(stockCount.ItemStatus == ItemStateType.Cleared);
                    Validator.IsTrue(stockCount.ItemCount == 3 * numSgtinItemsPerSku);
                }

                var skuSum = stockCounts.Where(s => s.SkuId == sku.Id).Sum(s => s.ItemCount);
                Validator.IsTrue(skuSum == (numSgtinItemsPerSku * 3));
            }

            // now do a cycle count but bring only the first lot back
            // create the cycle count
            var addDto4 = new AddCycleCountDto() { PlaceId = place.Id };
            var cycleDto4 = await _cycleCountRepo.CreateCycleCount(addDto4);

            // resolve the snapshot
            var resolveDto4 = new ResolveCycleCountDto(addSnapshotDto)
            {
                CycleCountId = cycleDto4.Id
            };
            responseDto = await _cycleCountRepo.ResolveCycleCount(resolveDto4);
            cycleDto = responseDto.CycleCountDto;
            // check no notifications
            Validator.IsTrue(responseDto.Notifications.Count <= 0, "Notifications in response");

            // now query the stock count for this place
            query = QueryBuilder<SkuStockCountDto>.NewQuery(s => s.PlaceId, place.Id, ComparisonOperator.Equals).Build();
            stockCounts = await _skuRepo.GetSkuStockCount(query);

            // check the response
            Validator.IsInstanceOfType(stockCounts, typeof(PageResult<SkuStockCountDto>));
            Validator.IsTrue(stockCounts.Items.Count() == (skus.Count * 2));  // should be 2 lots: present and cleared
            foreach (var sku in skus)
            {
                foreach (var stockCount in stockCounts.Where(s => s.SkuId == sku.Id))
                {
                    SkuDtoValidator.SkuStockCountCheck(stockCount, true);
                    Validator.IsTrue(stockCount.ItemStatus == ItemStateType.Cleared || stockCount.ItemStatus == ItemStateType.Present);
                    Validator.IsTrue(stockCount.ItemCount == (stockCount.ItemStatus == ItemStateType.Cleared ? 2 * numSgtinItemsPerSku : numSgtinItemsPerSku));
                }

                var skuSum = stockCounts.Where(s => s.SkuId == sku.Id).Sum(s => s.ItemCount);
                Validator.IsTrue(skuSum == (numSgtinItemsPerSku * 3));
            }

            // now query the stock count for this place, of items that aren't cleared
            query = QueryBuilder<SkuStockCountDto>.NewQuery(s => s.PlaceId, place.Id, ComparisonOperator.Equals)
                .And(s => s.ItemStatus, ItemStateType.Cleared,ComparisonOperator.NotEquals)
                .Build();
            stockCounts = await _skuRepo.GetSkuStockCount(query);

            // check the response
            Validator.IsInstanceOfType(stockCounts, typeof(PageResult<SkuStockCountDto>));
            Validator.IsTrue(stockCounts.Items.Count() == skus.Count);
            foreach (var sku in skus)
            {
                foreach (var stockCount in stockCounts.Where(s => s.SkuId == sku.Id))
                {
                    SkuDtoValidator.SkuStockCountCheck(stockCount, true);
                    Validator.IsTrue(stockCount.ItemStatus != ItemStateType.Cleared);
                    Validator.IsTrue(stockCount.ItemCount == numSgtinItemsPerSku);
                }

                var skuSum = stockCounts.Where(s => s.SkuId == sku.Id).Sum(s => s.ItemCount);
                Validator.IsTrue(skuSum == (numSgtinItemsPerSku));
            }

            // now query the stock count for this place, for a specific quantity of items
            query = QueryBuilder<SkuStockCountDto>.NewQuery(s => s.PlaceId, place.Id, ComparisonOperator.Equals)
                .And(s => s.ItemCount, numSgtinItemsPerSku, ComparisonOperator.Equals)
                .Build();
            stockCounts = await _skuRepo.GetSkuStockCount(query);
        }

        [TestMethod]
        public async Task Sku_GetStockCount_UniqueOnly()
        {
            int numUniqueItemsPerSku = 10;

            // create place
            var addPlace = await PlaceGenerator.GenerateRandomAddPlaceDto();
            var place = await _placeRepo.CreatePlace(addPlace);
            _placesToDelete.AddUnique(place.Id);

            // create some unique skus
            var skus = new List<SkuDetailDto>();
            for (int i = 0; i < 2; i++)
            {
                var addSkuDto = await SkuGenerator.GenerateTagPrefixSkuDto();
                var sku = await _skuRepo.CreateSku(addSkuDto);

                skus.Add(sku);  // add to list

                _skusToDelete.AddUnique(sku.Id);   // store to delete later
            }

            // create the items
            var createdItems = await ItemGenerator.GenerateItems(skus.Select(s => s.Id).ToDictionary(k => k, k => numUniqueItemsPerSku), place.Id);
            _itemsToDelete.AddRangeUnique(createdItems.Select(i => i.Id));

            // now query the stock count for this place
            var query = QueryBuilder<SkuStockCountDto>.NewQuery(s => s.PlaceId, place.Id, ComparisonOperator.Equals).Build();
            var stockCounts = await _skuRepo.GetSkuStockCount(query);

            // check the response
            Validator.IsInstanceOfType(stockCounts, typeof(PageResult<SkuStockCountDto>));
            Validator.IsTrue(stockCounts.Items.Count() == skus.Count);
            foreach (var sku in skus)
            {
                var stockCount = stockCounts.First(s => s.SkuId == sku.Id);
                SkuDtoValidator.SkuStockCountCheck(stockCount);
                Validator.IsTrue(stockCount.ItemStatus == ItemStateType.Present);
                Validator.IsTrue(stockCount.ItemCount == numUniqueItemsPerSku);
            }

            // now create more items in another place and move them here, so stock should ahve created and moved items
            var createdItems2 = await ItemGenerator.GenerateItems(skus.Select(s => s.Id).ToDictionary(k => k, k => numUniqueItemsPerSku), WebRepoContainer.Place1Id);
            _itemsToDelete.AddRangeUnique(createdItems2.Select(i => i.Id));
            foreach(var item in createdItems2)
            {
                await _itemRepo.UpdateItemPlace(new UpdateItemPlaceDto() { Id = item.Id, NewPlaceId = place.Id });
            }

            // now query the stock count for this place
            query = QueryBuilder<SkuStockCountDto>.NewQuery(s => s.PlaceId, place.Id, ComparisonOperator.Equals).Build();
            stockCounts = await _skuRepo.GetSkuStockCount(query);

            // check the response
            Validator.IsInstanceOfType(stockCounts, typeof(PageResult<SkuStockCountDto>));
            Validator.IsTrue(stockCounts.Items.Count() == (skus.Count * 2));
            foreach (var sku in skus)
            {
                foreach (var stockCount in stockCounts.Where(s => s.SkuId == sku.Id))
                {
                    SkuDtoValidator.SkuStockCountCheck(stockCount);
                    Validator.IsTrue(stockCount.ItemStatus == ItemStateType.Present || stockCount.ItemStatus == ItemStateType.Moved);
                    Validator.IsTrue(stockCount.ItemCount == numUniqueItemsPerSku);
                }
                var skuSum = stockCounts.Where(s => s.SkuId == sku.Id).Sum(s => s.ItemCount);
                Validator.IsTrue(skuSum == (numUniqueItemsPerSku * 2));
            }

            // now do an inventory on the location with only the first set of tags, to get present and missing stock
            // create the inventory
            var addInventoryDto = new AddInventoryDto()
            {
                Name = "Test Inventory",
                PlaceId = place.Id
            };
            var inventory = await _inventoryRepo.CreateInventory(addInventoryDto);

            // build snapshot to resolve
            var addSnapshotDto = await SnapshotGenerator.GenerateSgtinSnapshot(null, createdItems.Select(i => i.TagNumber).ToList());
            _tagNumbersToDelete.AddRangeUnique(addSnapshotDto.Tags.Select(t => t.TagNumber));

            // resolve the snapshot
            var responseDto = await _inventoryRepo.AddSnapshot(inventory.Id, addSnapshotDto);
            inventory = responseDto.InventoryDto;
            // check no notifications
            Validator.IsTrue(responseDto.Notifications.Count <= 0, "Notifications in response");

            // resolve the inventory with no reasons
            var resolveDto = new ResolveInventoryDto() { Id = inventory.Id };
            // set reason for all items
            foreach (var item in inventory.FoundItemsExpected)
                resolveDto.FoundItemsExpected.Add(new ResolveItemDto() { Id = item.Id });
            foreach (var item in inventory.FoundItemsUnexpected)
                resolveDto.FoundItemsUnexpected.Add(new ResolveItemDto() { Id = item.Id });
            foreach (var item in inventory.MissingItems)
                resolveDto.MissingItems.Add(new ResolveItemDto() { Id = item.Id });

            var resolvedInventory = await _inventoryRepo.Resolve(resolveDto);

            // now query the stock count for this place
            query = QueryBuilder<SkuStockCountDto>.NewQuery(s => s.PlaceId, place.Id, ComparisonOperator.Equals).Build();
            stockCounts = await _skuRepo.GetSkuStockCount(query);

            // check the response
            Validator.IsInstanceOfType(stockCounts, typeof(PageResult<SkuStockCountDto>));
            Validator.IsTrue(stockCounts.Items.Count() == (skus.Count * 2));
            foreach (var sku in skus)
            {
                foreach (var stockCount in stockCounts.Where(s => s.SkuId == sku.Id))
                {
                    SkuDtoValidator.SkuStockCountCheck(stockCount);
                    Validator.IsTrue(stockCount.ItemStatus == ItemStateType.Present || stockCount.ItemStatus == ItemStateType.Missing);
                    Validator.IsTrue(stockCount.ItemCount == numUniqueItemsPerSku);
                }
                var skuSum = stockCounts.Where(s => s.SkuId == sku.Id).Sum(s => s.ItemCount);
                Validator.IsTrue(skuSum == (numUniqueItemsPerSku * 2));
            }
        }

        [TestMethod]
        public async Task SgtinDecodeTest()
        {
            var tag = new TestTag("52414D5000A0001000000027");

            Assert.IsFalse(tag.HasSgtin());

            tag.TagNumber = "30340003EB5BAF8000000243";

            Validator.IsTrue(tag.HasSgtin());
        }

        [TestMethod]
        public async Task GTIN13_Encode_Decode_Test()
        {
            string prefix = "9300601";
            string itemRef = "002178";
            string barcodeNo = "9300601021789";

            Validator.IsTrue((prefix + itemRef).Length == 13);

            var epc = SgtinGenerator.GenerateSgtin96PosTag(prefix, itemRef, 1);
            
            var tag = new TestTag(epc);

            Validator.IsTrue(tag.HasSgtin());

            Validator.IsTrue(tag.GetCompanyPrefix() == prefix);
            Validator.IsTrue(tag.GetItemreference() == itemRef);
            Validator.IsTrue(tag.GetGtin() == barcodeNo);
        }
    }
}
