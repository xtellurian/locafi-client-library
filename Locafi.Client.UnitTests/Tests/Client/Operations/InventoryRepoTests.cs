using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Model.Dto.Persons;
using Locafi.Client.Model.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Locafi.Client.Model.Dto.Tags;
using Locafi.Client.Exceptions;
using Locafi.Client.Model.Dto.CycleCountDtos;
using Locafi.Client.Model.Query.Builder;
using Locafi.Client.Model.Dto.Places;
using Locafi.Client.UnitTests.EntityGenerators;
using Locafi.Client.UnitTests.Validators;
using Locafi.Client.Model.Dto.SkuGroups;
using Locafi.Client.Model.Dto.Inventory;
using Locafi.Client.Model.Dto.Items;
using Locafi.Client.Model.Dto.Skus;
using Locafi.Client.Model.Query;
using Locafi.Client.UnitTests.Extensions;
using Locafi.Client.Model.Dto.Reasons;

namespace Locafi.Client.UnitTests.Tests
{
    [TestClass]
    public class InventoryRepoTests
    {
        private IPlaceRepo _placeRepo;
        private IPersonRepo _personRepo;
        private ITemplateRepo _templateRepo;
        private IInventoryRepo _inventoryRepo;
        private IItemRepo _itemRepo;
        private ISkuRepo _skuRepo;
        private IReasonRepo _reasonRepo;
        private List<Guid> _itemsToDelete;
        private List<Guid> _placesToDelete;
        private List<string> _tagNumbersToDelete;
        private List<Guid> _reasonsToDelete;
        private SkuGroupDetailDto _skuGroup;
        private ISkuGroupRepo _skuGroupRepo;

        [TestInitialize]
        public void Initialise()
        {
            _placeRepo = WebRepoContainer.PlaceRepo;
            _personRepo = WebRepoContainer.PersonRepo;
            _templateRepo = WebRepoContainer.TemplateRepo;
            _inventoryRepo = WebRepoContainer.InventoryRepo;
            _itemRepo = WebRepoContainer.ItemRepo;
            _skuRepo = WebRepoContainer.SkuRepo;
            _reasonRepo = WebRepoContainer.ReasonRepo;
            _skuGroupRepo = WebRepoContainer.SkuGroupRepo;
            _itemsToDelete = new List<Guid>();
            _placesToDelete = new List<Guid>();
            _tagNumbersToDelete = new List<string>();
            _reasonsToDelete = new List<Guid>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            // delete all items that were created
            foreach (var itemId in _itemsToDelete)
            {
                _itemRepo.DeleteItem(itemId).Wait();
            }

            // delete all places that were created
            foreach (var placeId in _placesToDelete)
            {
                _placeRepo.Delete(placeId).Wait();
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

            // delete all reasons that were created
            foreach (var reasonId in _reasonsToDelete)
            {
                _reasonRepo.Delete(reasonId).Wait();
            }

            // delete skugroup
            if (_skuGroup != null)
            {
                try
                {
                    _skuGroupRepo.DeleteSkuGroup(_skuGroup.Id).Wait();
                    _skuGroupRepo.DeleteSkuGroupName(_skuGroup.SkuGroupNameId);
                }
                catch { }
            }
        }

        [TestMethod]
        public async Task Inventory_CreateResolve_AssetOnly()
        {
            var categories = new Dictionary<Guid, int>()
            {
                { WebRepoContainer.AssetCategory1Id, 10 },
                { WebRepoContainer.AssetCategory2Id, 5 }
            };
            await TestCompleteInventoryProcess(null, categories);
        }
/*
        [TestMethod]
        public async Task Inventory_CreateResolve_SkuOnly()
        {
            var skus = new Dictionary<Guid, int>()
            {
                { WebRepoContainer.Sku1Id, 10 },
                { WebRepoContainer.Sku2Id, 5 }
            };
            await TestCompleteInventoryProcess(skus,null);
        }

        [TestMethod]
        public async Task Inventory_CreateResolve_AssetAndSku()
        {
            var skus = new Dictionary<Guid, int>()
            {
                { WebRepoContainer.Sku1Id, 10 },
                { WebRepoContainer.Sku2Id, 5 }
            };
            var categories = new Dictionary<Guid, int>()
            {
                { WebRepoContainer.AssetCategory1Id, 5 },
                { WebRepoContainer.AssetCategory2Id, 5 }
            };
            await TestCompleteInventoryProcess(skus, categories);
        }
*/
        [TestMethod]
        public async Task Inventory_CreateResolve_WithSkuGroup_AssetOnly()
        {
            var categories = new Dictionary<Guid, int>()
            {
                { WebRepoContainer.AssetCategory1Id, 5 },
                { WebRepoContainer.AssetCategory2Id, 5 }
            };
            if (_skuGroup == null)
            {
                var skuGroupName = await _skuGroupRepo.CreateSkuGroupName(new AddSkuGroupNameDto("Inventory Test Sku Group"));
                var skuGroup = await _skuGroupRepo.CreateSkuGroup(new AddSkuGroupDto()
                {
                    SkuGroupNameId = skuGroupName.Id,
                    PlaceIds = new List<Guid>()
                {
                    WebRepoContainer.Place1Id,
                    WebRepoContainer.Place2Id
                },
                    SkuIds = new List<Guid>()
                {
                    WebRepoContainer.AssetCategory1Id,
                    WebRepoContainer.Sku1Id
                }
                });
                _skuGroup = skuGroup;   // store for later cleanup
            }

            await TestCompleteInventoryProcess(null, categories, _skuGroup);
        }
/*
        [TestMethod]
        public async Task Inventory_CreateResolve_WithSkuGroup_SkuOnly()
        {
            var skus = new Dictionary<Guid, int>()
            {
                { WebRepoContainer.Sku1Id, 10 },
                { WebRepoContainer.Sku2Id, 5 }
            };
            if (_skuGroup == null)
            {
                var skuGroupName = await _skuGroupRepo.CreateSkuGroupName(new AddSkuGroupNameDto("Inventory Test Sku Group"));
                var skuGroup = await _skuGroupRepo.CreateSkuGroup(new AddSkuGroupDto()
                {
                    SkuGroupNameId = skuGroupName.Id,
                    PlaceIds = new List<Guid>()
                {
                    WebRepoContainer.Place1Id,
                    WebRepoContainer.Place2Id
                },
                    SkuIds = new List<Guid>()
                {
                    WebRepoContainer.AssetCategory1Id,
                    WebRepoContainer.Sku1Id
                }
                });
                _skuGroup = skuGroup;   // store for later cleanup
            }

            await TestCompleteInventoryProcess(skus, null, _skuGroup);
        }

        [TestMethod]
        public async Task Inventory_CreateResolve_WithSkuGroup_AssetAndSku()
        {
            var skus = new Dictionary<Guid, int>()
            {
                { WebRepoContainer.Sku1Id, 10 },
                { WebRepoContainer.Sku2Id, 5 }
            };
            var categories = new Dictionary<Guid, int>()
            {
                { WebRepoContainer.AssetCategory1Id, 5 },
                { WebRepoContainer.AssetCategory2Id, 5 }
            };
            if (_skuGroup == null)
            {
                var skuGroupName = await _skuGroupRepo.CreateSkuGroupName(new AddSkuGroupNameDto("Inventory Test Sku Group"));
                var skuGroup = await _skuGroupRepo.CreateSkuGroup(new AddSkuGroupDto()
                {
                    SkuGroupNameId = skuGroupName.Id,
                    PlaceIds = new List<Guid>()
                {
                    WebRepoContainer.Place1Id,
                    WebRepoContainer.Place2Id
                },
                    SkuIds = new List<Guid>()
                {
                    WebRepoContainer.AssetCategory1Id,
                    WebRepoContainer.Sku1Id
                }
                });
                _skuGroup = skuGroup;   // store for later cleanup
            }

            await TestCompleteInventoryProcess(skus, categories, _skuGroup);
        }
*/
        [TestMethod]
        public async Task Inventory_CreateResolve_WithSelectedSkus_AssetOnly()
        {
            var categories = new Dictionary<Guid, int>()
            {
                { WebRepoContainer.AssetCategory1Id, 10 }
            };
            var selectedSkus = new List<Guid>()
            {
                WebRepoContainer.AssetCategory1Id,
                WebRepoContainer.Sku1Id
            };

            await TestCompleteInventoryProcess(null, categories, null, selectedSkus);
        }
        /*
                [TestMethod]
                public async Task Inventory_CreateResolve_WithSelectedSkus_SkuOnly()
                {
                    var skus = new Dictionary<Guid, int>()
                    {
                        { WebRepoContainer.Sku1Id, 10 },
                        { WebRepoContainer.Sku2Id, 5 }
                    };
                    var selectedSkus = new List<Guid>()
                    {
                        WebRepoContainer.AssetCategory1Id,
                        WebRepoContainer.Sku1Id
                    };

                    await TestCompleteInventoryProcess(skus, null, null, selectedSkus);
                }

                [TestMethod]
                public async Task Inventory_CreateResolve_WithSelectedSkus_AssetAndSku()
                {
                    var skus = new Dictionary<Guid, int>()
                    {
                        { WebRepoContainer.Sku1Id, 10 },
                        { WebRepoContainer.Sku2Id, 5 }
                    };
                    var categories = new Dictionary<Guid, int>()
                    {
                        { WebRepoContainer.AssetCategory1Id, 10 }
                    };
                    var selectedSkus = new List<Guid>()
                    {
                        WebRepoContainer.AssetCategory1Id,
                        WebRepoContainer.Sku1Id
                    };

                    await TestCompleteInventoryProcess(skus, categories, null, selectedSkus);
                }
        */

        [TestMethod]
        public async Task Inventory_Query()
        {
            // create an inventory
            var rand = new Random(DateTime.UtcNow.Millisecond);
            var addInventoryDto = new AddInventoryDto()
            {
                Name = "Test Inventory - " + rand.Next().ToString(),
                PlaceId = WebRepoContainer.Place1Id
            };
            var inventory = await _inventoryRepo.CreateInventory(addInventoryDto);

            // check the response
            InventoryDtoValidator.InventoryDetailcheck(inventory);

            var inventories = await _inventoryRepo.QueryInventories();

            Validator.IsNotNull(inventories);
            Validator.IsTrue(inventories.Items.Count() > 0);
            Validator.IsTrue(inventories.Items.Contains(inventory));
            InventoryDtoValidator.InventorySummarycheck(inventories.Items.First(i => i.Id == inventory.Id));
        }

        [TestMethod]
        public async Task Inventory_Get()
        {
            // create an inventory
            var rand = new Random(DateTime.UtcNow.Millisecond);
            var addInventoryDto = new AddInventoryDto()
            {
                Name = "Test Inventory - " + rand.Next().ToString(),
                PlaceId = WebRepoContainer.Place1Id
            };
            var inventory = await _inventoryRepo.CreateInventory(addInventoryDto);

            // check the response
            InventoryDtoValidator.InventoryDetailcheck(inventory);

            var getInventory = await _inventoryRepo.GetInventory(inventory.Id);

            Validator.IsNotNull(getInventory);
            InventoryDtoValidator.InventoryDetailcheck(getInventory);
        }

        private async Task TestCompleteInventoryProcess(Dictionary<Guid,int> skusToUse, Dictionary<Guid,int> assetCategoriesToUse, SkuGroupDetailDto SkuGroup = null, List<Guid> selectedSkus = null)
        {
            Validator.IsFalse(SkuGroup != null && selectedSkus != null, "Cannot provide both sku group and list of sku's");

            var t1 = DateTime.UtcNow;
            var rand = new Random(DateTime.UtcNow.Millisecond);

            // create a place for the inventory
            var addPlace1Dto = await PlaceGenerator.GenerateRandomAddPlaceDto();
            addPlace1Dto.Name = "Inventory - " + addPlace1Dto.Name;
            var place1 = await _placeRepo.CreatePlace(addPlace1Dto);
            _placesToDelete.Add(place1.Id);

            if (SkuGroup != null)
            {
                // add place to skugroup
                var updateSkuGroupDto = new UpdateSkuGroupDto(SkuGroup);
                updateSkuGroupDto.AddPlace(place1.Id);
                SkuGroup = await _skuGroupRepo.UpdateSkuGroup(updateSkuGroupDto);
            }

            // create the inventory
            var addInventoryDto = new AddInventoryDto()
            {
                Name = "Test Inventory - " + rand.Next().ToString(),
                PlaceId = place1.Id
            };
            if (SkuGroup != null)    // add sku group if required
                addInventoryDto.SkuGroupId = SkuGroup.Id;
            if (selectedSkus != null)   // add selected skus if required
                addInventoryDto.SkuIds = selectedSkus;
            var inventory = await _inventoryRepo.CreateInventory(addInventoryDto);

            // check the response
            InventoryDtoValidator.InventoryDetailcheck(inventory);

            Validator.IsTrue(inventory.FoundItemsExpected.Count == 0);
            Validator.IsTrue(inventory.FoundItemsUnexpected.Count == 0);
            Validator.IsTrue(inventory.MissingItems.Count == 0);

            // add some existing items to the place
            List<ItemDetailDto> createdFoundItems = new List<ItemDetailDto>();

            // create items in the place to show as found items
            createdFoundItems.AddRange(await ItemGenerator.GenerateItems(assetCategoriesToUse, place1.Id));
            _itemsToDelete.AddRange(createdFoundItems.Select(i => i.Id));

            // build snapshot to resolve
            var addSnapshotDto = await SnapshotGenerator.GenerateSgtinSnapshot(skusToUse,createdFoundItems.Select(i => i.TagNumber).ToList());
            _tagNumbersToDelete.AddRange(addSnapshotDto.Tags.Where(t => !_tagNumbersToDelete.Contains(t.TagNumber)).Select(t => t.TagNumber));

            // resolve the snapshot
            inventory = await _inventoryRepo.AddSnapshot(inventory.Id,addSnapshotDto);

            // check the response we have created the items
            InventoryDtoValidator.InventoryDetailcheck(inventory);
            Validator.IsTrue(inventory.MissingItems.Count == 0);
            if (SkuGroup != null)
            {
                Validator.IsTrue(inventory.SkuGroupId == SkuGroup.Id);

                // get the skus that are valid in our tags used in the inventory
                var includedSkus = new List<Guid>();
                if(skusToUse != null)
                    includedSkus = SkuGroup.Skus.Select(s => s.Id).Intersect(skusToUse.Keys).ToList();
                includedSkus = includedSkus.Concat(SkuGroup.Skus.Select(s => s.Id).Intersect(createdFoundItems.Select(i => i.SkuId))).ToList();

                var expectedItemsCount = createdFoundItems.Where(i => includedSkus.Contains(i.SkuId)).Count();
                Validator.IsTrue(inventory.FoundItemsExpected.Count == expectedItemsCount);  // check the correct number of sku types have been found

                var unexpectedItemsCount = skusToUse != null ? skusToUse.Where(s => includedSkus.Contains(s.Key)).Select(s => s.Value).Sum() : 0;
                Validator.IsTrue(inventory.FoundItemsUnexpected.Count == unexpectedItemsCount);  // check the correct number of sku types have been found
            }
            else if (selectedSkus != null)
            {
                Validator.IsTrue(inventory.SelectedSkus.Count == selectedSkus.Count);

                // get the skus that are valid in our tags used in the inventory
                var includedSkus = new List<Guid>();
                if (skusToUse != null)
                    includedSkus = SkuGroup.Skus.Select(s => s.Id).Intersect(skusToUse.Keys).ToList();
                includedSkus = includedSkus.Concat(selectedSkus.Intersect(createdFoundItems.Select(i => i.SkuId))).ToList();

                var expectedItemsCount = createdFoundItems.Where(i => includedSkus.Contains(i.SkuId)).Count();
                Validator.IsTrue(inventory.FoundItemsExpected.Count == expectedItemsCount);  // check the correct number of sku types have been found

                var unexpectedItemsCount = skusToUse != null ? skusToUse.Where(s => includedSkus.Contains(s.Key)).Select(s => s.Value).Sum() : 0;
                Validator.IsTrue(inventory.FoundItemsUnexpected.Count == unexpectedItemsCount);  // check the correct number of sku types have been found
            }
            else
            {
                var expectedItemsCount = createdFoundItems.Count();
                Validator.IsTrue(inventory.FoundItemsExpected.Count == expectedItemsCount);  // check the correct number of sku types have been found

                var unexpectedItemsCount = skusToUse != null ? skusToUse.Select(s => s.Value).Sum() : 0;
                Validator.IsTrue(inventory.FoundItemsUnexpected.Count == unexpectedItemsCount);  // check the correct number of sku types have been found
            }
            // check the returned ItemSumamryReasonDto's
            foreach (var dto in inventory.FoundItemsExpected)
                ItemDtoValidator.ItemSummaryReasonCheck(dto);
            foreach (var dto in inventory.FoundItemsUnexpected)
                ItemDtoValidator.ItemSummaryReasonCheck(dto);
            foreach (var dto in inventory.MissingItems)
                ItemDtoValidator.ItemSummaryReasonCheck(dto);

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
            // check the response we have created the items
            InventoryDtoValidator.InventoryDetailcheck(resolvedInventory, true);
            // check the returned ItemSumamryReasonDto's
            foreach (var dto in resolvedInventory.FoundItemsExpected)
                ItemDtoValidator.ItemSummaryReasonCheck(dto);
            foreach (var dto in resolvedInventory.FoundItemsUnexpected)
                ItemDtoValidator.ItemSummaryReasonCheck(dto);
            foreach (var dto in resolvedInventory.MissingItems)
                ItemDtoValidator.ItemSummaryReasonCheck(dto);

            ///////////////////////////////////////////////////////////////////////
            // now run an inventory with unexpected items, and resolve all items //
            ///////////////////////////////////////////////////////////////////////

            // create the inventory
            var addInventoryDto2 = new AddInventoryDto()
            {
                Name = "Test Inventory - " + rand.Next().ToString(),
                PlaceId = place1.Id
            };
            if (SkuGroup != null)    // add sku group if required
                addInventoryDto2.SkuGroupId = SkuGroup.Id;
            if (selectedSkus != null)   // add selected skus if required
                addInventoryDto2.SkuIds = selectedSkus;
            var inventory2 = await _inventoryRepo.CreateInventory(addInventoryDto2);

            // check the response
            InventoryDtoValidator.InventoryDetailcheck(inventory2);

            Validator.IsTrue(inventory2.FoundItemsExpected.Count == 0);
            Validator.IsTrue(inventory2.FoundItemsUnexpected.Count == 0);
            Validator.IsTrue(inventory2.MissingItems.Count == (resolvedInventory.FoundItemsExpected.Count + resolvedInventory.FoundItemsUnexpected.Count)); // all items in the place should initially be missing

            // add some existing items to another place to be unexpected items
            List<ItemDetailDto> createdUnexpectedItems = new List<ItemDetailDto>();

            // create items in the place to show as found items
            createdUnexpectedItems.AddRange(await ItemGenerator.GenerateItems(assetCategoriesToUse, WebRepoContainer.Place1Id));
            _itemsToDelete.AddRange(createdUnexpectedItems.Select(i => i.Id));

            // build snapshot to resolve (new set of tags + the last set of tags which will be found items)
            var addSnapshotDto2 = await SnapshotGenerator.GenerateSgtinSnapshot(skusToUse, createdUnexpectedItems.Select(i => i.TagNumber).Concat(addSnapshotDto.Tags.Select(t => t.TagNumber)).ToList());
            _tagNumbersToDelete.AddRange(addSnapshotDto.Tags.Where(t => !_tagNumbersToDelete.Contains(t.TagNumber)).Select(t => t.TagNumber));

            // resolve the snapshot
            inventory2 = await _inventoryRepo.AddSnapshot(inventory2.Id, addSnapshotDto2);

            // check the response we have created the items
            InventoryDtoValidator.InventoryDetailcheck(inventory2);
            Validator.IsTrue(inventory2.MissingItems.Count == 0);   // no more missing items
            if (SkuGroup != null)
            {
                Validator.IsTrue(inventory2.SkuGroupId == SkuGroup.Id);

                // get the skus that are valid in our tags used in the inventory
                var includedSkus = new List<Guid>();
                if (skusToUse != null)
                    includedSkus = SkuGroup.Skus.Select(s => s.Id).Intersect(skusToUse.Keys).ToList();
                includedSkus = includedSkus.Concat(SkuGroup.Skus.Select(s => s.Id).Intersect(createdFoundItems.Select(i => i.SkuId))).ToList();

                var expectedItemsCount = createdFoundItems.Where(i => includedSkus.Contains(i.SkuId)).Count() + (skusToUse != null ? skusToUse.Where(s => includedSkus.Contains(s.Key)).Select(s => s.Value).Sum() : 0);
                Validator.IsTrue(inventory2.FoundItemsExpected.Count == expectedItemsCount);  // check the correct number of sku types have been found

                var unexpectedItemsCount = createdUnexpectedItems.Where(i => includedSkus.Contains(i.SkuId)).Count() + (skusToUse != null ? skusToUse.Where(s => includedSkus.Contains(s.Key)).Select(s => s.Value).Sum() : 0);
                Validator.IsTrue(inventory2.FoundItemsUnexpected.Count == unexpectedItemsCount);  // check the correct number of sku types have been found
            }
            else if (selectedSkus != null)
            {
                Validator.IsTrue(inventory2.SelectedSkus.Count == selectedSkus.Count);

                // get the skus that are valid in our tags used in the inventory
                var includedSkus = new List<Guid>();
                if (skusToUse != null)
                    includedSkus = SkuGroup.Skus.Select(s => s.Id).Intersect(skusToUse.Keys).ToList();
                includedSkus = includedSkus.Concat(selectedSkus.Intersect(createdFoundItems.Select(i => i.SkuId))).ToList();

                var expectedItemsCount = createdFoundItems.Where(i => includedSkus.Contains(i.SkuId)).Count() + (skusToUse != null ? skusToUse.Where(s => includedSkus.Contains(s.Key)).Select(s => s.Value).Sum() : 0);
                Validator.IsTrue(inventory2.FoundItemsExpected.Count == expectedItemsCount);  // check the correct number of sku types have been found

                var unexpectedItemsCount = createdUnexpectedItems.Where(i => includedSkus.Contains(i.SkuId)).Count() + (skusToUse != null ? skusToUse.Where(s => includedSkus.Contains(s.Key)).Select(s => s.Value).Sum() : 0);
                Validator.IsTrue(inventory2.FoundItemsUnexpected.Count == unexpectedItemsCount);  // check the correct number of sku types have been found
            }
            else
            {
                var expectedItemsCount = createdFoundItems.Count() + (skusToUse != null ? skusToUse.Select(s => s.Value).Sum() : 0);
                Validator.IsTrue(inventory2.FoundItemsExpected.Count == expectedItemsCount);  // check the correct number of sku types have been found

                var unexpectedItemsCount = createdUnexpectedItems.Count() + (skusToUse != null ? skusToUse.Select(s => s.Value).Sum() : 0);
                Validator.IsTrue(inventory2.FoundItemsUnexpected.Count == unexpectedItemsCount);  // check the correct number of sku types have been found
            }
            // check the returned ItemSumamryReasonDto's
            foreach (var dto in inventory2.FoundItemsExpected)
                ItemDtoValidator.ItemSummaryReasonCheck(dto);
            foreach (var dto in inventory2.FoundItemsUnexpected)
                ItemDtoValidator.ItemSummaryReasonCheck(dto);
            foreach (var dto in inventory2.MissingItems)
                ItemDtoValidator.ItemSummaryReasonCheck(dto);

            // create a reason
            var reason = await _reasonRepo.CreateReason(new AddReasonDto() { ReasonNumber = Guid.NewGuid().ToString(), Description = Guid.NewGuid().ToString() });
            _reasonsToDelete.AddUnique(reason.Id);

            var resolveDto2 = new ResolveInventoryDto() { Id = inventory2.Id };
            // set reason for all items
            foreach (var item in inventory2.FoundItemsExpected)
                resolveDto2.FoundItemsExpected.Add(new ResolveItemDto() { Id = item.Id, ReasonId = reason.Id });
            foreach (var item in inventory2.FoundItemsUnexpected)
                resolveDto2.FoundItemsUnexpected.Add(new ResolveItemDto() { Id = item.Id, ReasonId = reason.Id });
            foreach (var item in inventory2.MissingItems)
                resolveDto2.MissingItems.Add(new ResolveItemDto() { Id = item.Id, ReasonId = reason.Id });

            // resolve the inventory with reasons
            var resolvedInventory2 = await _inventoryRepo.Resolve(resolveDto2);
            // check the response we have set the reasons
            InventoryDtoValidator.InventoryDetailcheck(resolvedInventory2, true);
            // check the returned ItemSumamryReasonDto's
            foreach (var dto in resolvedInventory2.FoundItemsExpected)
                ItemDtoValidator.ItemSummaryReasonCheck(dto, reason.Id);
            foreach (var dto in resolvedInventory2.FoundItemsUnexpected)
                ItemDtoValidator.ItemSummaryReasonCheck(dto, reason.Id);
            foreach (var dto in resolvedInventory2.MissingItems)
                ItemDtoValidator.ItemSummaryReasonCheck(dto, reason.Id);

            //////////////////////////////////////////////////////////////////////////////////
            // Now do another inventory but with only the snapshot from the first inventory //
            // which should give no unexpected, but half missing/half found                 //
            //////////////////////////////////////////////////////////////////////////////////

            // create the inventory
            var addInventoryDto3 = new AddInventoryDto()
            {
                Name = "Test Inventory - " + rand.Next().ToString(),
                PlaceId = place1.Id
            };
            if (SkuGroup != null)    // add sku group if required
                addInventoryDto3.SkuGroupId = SkuGroup.Id;
            if (selectedSkus != null)   // add selected skus if required
                addInventoryDto3.SkuIds = selectedSkus;
            var inventory3 = await _inventoryRepo.CreateInventory(addInventoryDto3);

            // check the response
            InventoryDtoValidator.InventoryDetailcheck(inventory3);

            Validator.IsTrue(inventory3.FoundItemsExpected.Count == 0);
            Validator.IsTrue(inventory3.FoundItemsUnexpected.Count == 0);
            Validator.IsTrue(inventory3.MissingItems.Count == (resolvedInventory2.FoundItemsExpected.Count + resolvedInventory2.FoundItemsUnexpected.Count)); // all items in the place should initially be missing

            // resolve the snapshot
            inventory3 = await _inventoryRepo.AddSnapshot(inventory3.Id, addSnapshotDto);

            // check the response we have created the items
            InventoryDtoValidator.InventoryDetailcheck(inventory3);
            Validator.IsTrue(inventory3.FoundItemsUnexpected.Count == 0);
            if (SkuGroup != null)
            {
                Validator.IsTrue(inventory3.SkuGroupId == SkuGroup.Id);

                // get the skus that are valid in our tags used in the inventory
                var includedSkus = new List<Guid>();
                if (skusToUse != null)
                    includedSkus = SkuGroup.Skus.Select(s => s.Id).Intersect(skusToUse.Keys).ToList();
                includedSkus = includedSkus.Concat(SkuGroup.Skus.Select(s => s.Id).Intersect(createdFoundItems.Select(i => i.SkuId))).ToList();

                var expectedItemsCount = createdFoundItems.Where(i => includedSkus.Contains(i.SkuId)).Count() + (skusToUse != null ? skusToUse.Where(s => includedSkus.Contains(s.Key)).Select(s => s.Value).Sum() : 0);

                Validator.IsTrue(inventory3.MissingItems.Count == expectedItemsCount);  // check the correct number of sku types have been found
                Validator.IsTrue(inventory3.FoundItemsExpected.Count == expectedItemsCount);  // check the correct number of sku types have been found
            }
            else if (selectedSkus != null)
            {
                Validator.IsTrue(inventory3.SelectedSkus.Count == selectedSkus.Count);

                // get the skus that are valid in our tags used in the inventory
                var includedSkus = new List<Guid>();
                if (skusToUse != null)
                    includedSkus = SkuGroup.Skus.Select(s => s.Id).Intersect(skusToUse.Keys).ToList();
                includedSkus = includedSkus.Concat(selectedSkus.Intersect(createdFoundItems.Select(i => i.SkuId))).ToList();

                var expectedItemsCount = createdFoundItems.Where(i => includedSkus.Contains(i.SkuId)).Count() + (skusToUse != null ? skusToUse.Where(s => includedSkus.Contains(s.Key)).Select(s => s.Value).Sum() : 0);

                Validator.IsTrue(inventory3.FoundItemsExpected.Count == expectedItemsCount);  // check the correct number of sku types have been found
                Validator.IsTrue(inventory3.MissingItems.Count == expectedItemsCount);  // check the correct number of sku types have been found
            }
            else
            {
                var expectedItemsCount = createdFoundItems.Count() + (skusToUse != null ? skusToUse.Select(s => s.Value).Sum() : 0);

                Validator.IsTrue(inventory3.FoundItemsExpected.Count == expectedItemsCount);  // check the correct number of sku types have been found
                Validator.IsTrue(inventory3.MissingItems.Count == expectedItemsCount);  // check the correct number of sku types have been found
            }
            // check the returned ItemSumamryReasonDto's
            foreach (var dto in inventory3.FoundItemsExpected)
                ItemDtoValidator.ItemSummaryReasonCheck(dto);
            foreach (var dto in inventory3.FoundItemsUnexpected)
                ItemDtoValidator.ItemSummaryReasonCheck(dto);
            foreach (var dto in inventory3.MissingItems)
                ItemDtoValidator.ItemSummaryReasonCheck(dto);


            // set reason for all items
            var resolveDto3 = new ResolveInventoryDto() { Id = inventory3.Id };
            // set reason for all items
            foreach (var item in inventory3.FoundItemsExpected)
                resolveDto3.FoundItemsExpected.Add(new ResolveItemDto() { Id = item.Id, ReasonId = reason.Id });
            foreach (var item in inventory3.FoundItemsUnexpected)
                resolveDto3.FoundItemsUnexpected.Add(new ResolveItemDto() { Id = item.Id, ReasonId = reason.Id });
            foreach (var item in inventory3.MissingItems)
                resolveDto3.MissingItems.Add(new ResolveItemDto() { Id = item.Id, ReasonId = reason.Id });

            // resolve the inventory with reasons
            var resolvedInventory3 = await _inventoryRepo.Resolve(resolveDto3);

            // check the response we have set the reasons
            InventoryDtoValidator.InventoryDetailcheck(resolvedInventory3, true);
            // check the returned ItemSumamryReasonDto's
            foreach (var dto in resolvedInventory3.FoundItemsExpected)
                ItemDtoValidator.ItemSummaryReasonCheck(dto, reason.Id);
            foreach (var dto in resolvedInventory3.FoundItemsUnexpected)
                ItemDtoValidator.ItemSummaryReasonCheck(dto, reason.Id);
            foreach (var dto in resolvedInventory3.MissingItems)
                ItemDtoValidator.ItemSummaryReasonCheck(dto, reason.Id);

            var t2 = DateTime.UtcNow;
            var totalTime = t2 - t1;
        }
    }
}
