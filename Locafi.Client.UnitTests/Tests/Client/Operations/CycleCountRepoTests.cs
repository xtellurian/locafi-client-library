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
using Locafi.Client.Model.Dto.Items;
using Locafi.Client.Model.Query;

namespace Locafi.Client.UnitTests.Tests
{
    [TestClass]
    public class CycleCountRepoTests
    {
        private ICycleCountRepo _cycleCountRepo;
        private IPlaceRepo _placeRepo;
        private IPersonRepo _personRepo;
        private ITemplateRepo _templateRepo;
        private ISkuGroupRepo _skuGroupRepo;
        private IList<Guid> _toCleanup;
        private SkuGroupDetailDto _skuGroup;
        private List<Guid> _itemsToDelete;
        private IItemRepo _itemRepo;
        private List<string> _tagNumbersToDelete;

        [TestInitialize]
        public void Initialise()
        {
            _cycleCountRepo = WebRepoContainer.CycleCountRepo;
            _placeRepo = WebRepoContainer.PlaceRepo;
            _personRepo = WebRepoContainer.PersonRepo;
            _templateRepo = WebRepoContainer.TemplateRepo;
            _skuGroupRepo = WebRepoContainer.SkuGroupRepo;
            _itemRepo = WebRepoContainer.ItemRepo;
            _toCleanup = new List<Guid>();
            _itemsToDelete = new List<Guid>();
            _tagNumbersToDelete = new List<string>();
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
            var query = QueryBuilder<ItemSummaryDto>.NewQuery(i => i.TagNumber, string.Join(",", _tagNumbersToDelete), ComparisonOperator.ContainedIn).Build();
            var itemQuery = _itemRepo.QueryItems(query).Result;
            foreach (var item in itemQuery.Items)
            {
                if (item != null)
                {
                    _itemRepo.DeleteItem(item.Id).Wait();
                }
            }

            // delete skugroup
            if (_skuGroup != null)
            {
                _skuGroupRepo.DeleteSkuGroup(_skuGroup.Id).Wait();
                _skuGroupRepo.DeleteSkuGroupName(_skuGroup.SkuGroupNameId);
            }
        }

        [TestMethod]
        public async Task CycleCount_CreateResolve_SkuOnly()
        {
            var skus = new Dictionary<Guid, int>()
            {
                { WebRepoContainer.Sku1Id, 10 },
                { WebRepoContainer.Sku2Id, 5 }
            };
            await TestCompleteCycleCountProcess(skus,null);
        }

        [TestMethod]
        public async Task CycleCount_CreateResolve_AssetOnly()
        {
            var categories = new Dictionary<Guid, int>()
            {
                { WebRepoContainer.AssetCategory1Id, 5 },
                { WebRepoContainer.AssetCategory2Id, 5 }
            };
            await TestCompleteCycleCountProcess(null, categories);
        }

        [TestMethod]
        public async Task CycleCount_CreateResolve_SkuAndAsset()
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
            await TestCompleteCycleCountProcess(skus, categories);
        }

        [TestMethod]
        public async Task CycleCount_CreateResolve_WithSkuGroup_SkuOnly()
        {
            var skus = new Dictionary<Guid, int>()
            {
                { WebRepoContainer.Sku1Id, 10 },
                { WebRepoContainer.Sku2Id, 5 }
            };

            var skuGroupName = await _skuGroupRepo.CreateSkuGroupName(new AddSkuGroupNameDto("CycleCount Test Sku Group"));
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
                    WebRepoContainer.Sku1Id,
                    WebRepoContainer.AssetCategory1Id
                }
            });
            _skuGroup = skuGroup;   // store for later cleanup

            await TestCompleteCycleCountProcess(skus, null, skuGroup);
        }

        [TestMethod]
        public async Task CycleCount_CreateResolve_WithSkuGroup_AssetOnly()
        {
            var categories = new Dictionary<Guid, int>()
            {
                { WebRepoContainer.AssetCategory1Id, 5 },
                { WebRepoContainer.AssetCategory2Id, 5 }
            };

            var skuGroupName = await _skuGroupRepo.CreateSkuGroupName(new AddSkuGroupNameDto("CycleCount Test Sku Group"));
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
                    WebRepoContainer.Sku1Id,
                    WebRepoContainer.AssetCategory1Id
                }
            });
            _skuGroup = skuGroup;   // store for later cleanup

            await TestCompleteCycleCountProcess(null, categories, skuGroup);
        }

        [TestMethod]
        public async Task CycleCount_CreateResolve_WithSkuGroup_SkuAndAsset()
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

            var skuGroupName = await _skuGroupRepo.CreateSkuGroupName(new AddSkuGroupNameDto("CycleCount Test Sku Group"));
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
                    WebRepoContainer.Sku1Id,
                    WebRepoContainer.AssetCategory1Id
                }
            });
            _skuGroup = skuGroup;   // store for later cleanup

            await TestCompleteCycleCountProcess(skus, categories, skuGroup);
        }

        [TestMethod]
        public async Task CycleCount_CreateResolve_WithSelectedSkus_SkuOnly()
        {
            var skus = new Dictionary<Guid, int>()
            {
                { WebRepoContainer.Sku1Id, 10 },
                { WebRepoContainer.Sku2Id, 5 }
            };
            var selectedSkus = new List<Guid>() { WebRepoContainer.Sku1Id, WebRepoContainer.AssetCategory1Id };

            await TestCompleteCycleCountProcess(skus, null, null, selectedSkus);
        }

        [TestMethod]
        public async Task CycleCount_CreateResolve_WithSelectedSkus_AssetOnly()
        {
            var categories = new Dictionary<Guid, int>()
            {
                { WebRepoContainer.AssetCategory1Id, 5 },
                { WebRepoContainer.AssetCategory2Id, 5 }
            };
            var selectedSkus = new List<Guid>() { WebRepoContainer.Sku1Id, WebRepoContainer.AssetCategory1Id };

            await TestCompleteCycleCountProcess(null, categories, null, selectedSkus);
        }

        [TestMethod]
        public async Task CycleCount_CreateResolve_WithSelectedSkus_SkuAndAsset()
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
            var selectedSkus = new List<Guid>() { WebRepoContainer.Sku1Id, WebRepoContainer.AssetCategory1Id };

            await TestCompleteCycleCountProcess(skus, categories, null, selectedSkus);
        }

        [TestMethod]
        public async Task CycleCount_CreateResolve_NoSelectedSkus_Large()
        {
            var sku1Count = 1500;
            var sku2Count = 1500;

            // create the cycle count
            var addDto = new AddCycleCountDto() { PlaceId = WebRepoContainer.Place1Id };
            var cycleDto = await _cycleCountRepo.CreateCycleCount(addDto);

            // check the response
            CycleCountDtoValidator.CycleCountDetailcheck(cycleDto);

            Validator.IsTrue(cycleDto.PresentItems.Count == 0);
            Validator.IsTrue(cycleDto.MovedItems.Count == 0);
            Validator.IsTrue(cycleDto.CreatedItems.Count == 0);

            // build snapshot to resolve
            var addSnapshotDto = await SnapshotGenerator.GenerateSgtinSnapshot(new Dictionary<Guid, int>() {
                { WebRepoContainer.Sku1Id, sku1Count },
                { WebRepoContainer.Sku2Id, sku2Count }
            },
            null
            );

            var resolveDto = new ResolveCycleCountDto(addSnapshotDto)
            {
                CycleCountId = cycleDto.Id
            };

            var t1 = DateTime.UtcNow;
            // resolve the snapshot
            cycleDto = await _cycleCountRepo.ResolveCycleCount(resolveDto);
            var t2 = DateTime.UtcNow;
            var totalTime = t2 - t1;

            // check the response we have create the items
            CycleCountDtoValidator.CycleCountDetailcheck(cycleDto, true);

            Validator.IsTrue(cycleDto.PresentItems.Count == 0);
            Validator.IsTrue(cycleDto.MovedItems.Count == 0);
            Validator.IsTrue(cycleDto.CreatedItems.Count == 2);
            Validator.IsTrue(cycleDto.CreatedItems.First(i => i.Id == WebRepoContainer.Sku1Id).ItemCount == sku1Count);
            Validator.IsTrue(cycleDto.CreatedItems.First(i => i.Id == WebRepoContainer.Sku2Id).ItemCount == sku2Count);

            SkuDtoValidator.SkuSummaryCheck(cycleDto.CreatedItems.First(i => i.Id == WebRepoContainer.Sku1Id), true);
            SkuDtoValidator.SkuSummaryCheck(cycleDto.CreatedItems.First(i => i.Id == WebRepoContainer.Sku2Id), true);
        }

        public async Task TestCompleteCycleCountProcess(Dictionary<Guid,int> skusToUse, Dictionary<Guid, int> assetCategoriesToUse, SkuGroupDetailDto SkuGroup = null, List<Guid> selectedSkus = null)
        {
            Validator.IsFalse(SkuGroup != null && selectedSkus != null, "Cannot provide both sku group and list of sku's");

            // have these empty rather than null
            if (skusToUse == null)
                skusToUse = new Dictionary<Guid, int>();

            if (assetCategoriesToUse == null)
                assetCategoriesToUse = new Dictionary<Guid, int>();

            var t1 = DateTime.UtcNow;

            // create the cycle count
            var addDto = new AddCycleCountDto() { PlaceId = WebRepoContainer.Place1Id };
            if (SkuGroup != null)    // add sku group if required
                addDto.SkuGroupId = SkuGroup.Id;
            if (selectedSkus != null)   // add selected skus if required
                addDto.SkuIds = selectedSkus;
            var cycleDto = await _cycleCountRepo.CreateCycleCount(addDto);

            // check the response
            CycleCountDtoValidator.CycleCountDetailcheck(cycleDto);

            Validator.IsTrue(cycleDto.PresentItems.Count == 0);
            Validator.IsTrue(cycleDto.MovedItems.Count == 0);
            Validator.IsTrue(cycleDto.CreatedItems.Count == 0);

            // add some existing items to the place
            List<ItemDetailDto> createdFoundItems = new List<ItemDetailDto>();

            // create items in the place to show as found items
            createdFoundItems.AddRange(await ItemGenerator.GenerateItems(assetCategoriesToUse, WebRepoContainer.Place1Id));
            _itemsToDelete.AddRange(createdFoundItems.Select(i => i.Id));

            // build snapshot to resolve
            var addSnapshotDto = await SnapshotGenerator.GenerateSgtinSnapshot(skusToUse, createdFoundItems.Select(i => i.TagNumber).ToList());
            _tagNumbersToDelete.AddRange(addSnapshotDto.Tags.Where(t => !_tagNumbersToDelete.Contains(t.TagNumber)).Select(t => t.TagNumber));

            var resolveDto = new ResolveCycleCountDto(addSnapshotDto)
            {
                CycleCountId = cycleDto.Id
            };

            // resolve the snapshot
            cycleDto = await _cycleCountRepo.ResolveCycleCount(resolveDto);

            // check the response we have created the items
            CycleCountDtoValidator.CycleCountDetailcheck(cycleDto, true);
            Validator.IsTrue(cycleDto.MovedItems.Count == 0);
            if (SkuGroup != null)
            {
                Validator.IsTrue(cycleDto.SkuGroupId == SkuGroup.Id);

                var includedSkus = SkuGroup.Skus.Select(s => s.Id).Intersect(skusToUse.Keys).ToList();

                Validator.IsTrue(cycleDto.CreatedItems.Count == includedSkus.Count);  // check the correct number of sku types have been created
                // check the sku count dto and that the right count of each sku were created
                foreach (var skuId in includedSkus)
                {
                    var kv = skusToUse.First(k => k.Key == skuId);
                    var skuCountDto = cycleDto.CreatedItems.First(i => i.Id == kv.Key);
                    SkuDtoValidator.SkuSummaryCheck(skuCountDto, true);
                    Validator.IsTrue(skuCountDto.ItemCount == kv.Value);
                }

                var expectedSkus = SkuGroup.Skus.Select(s => s.Id).Intersect(assetCategoriesToUse.Keys).ToList();
                Validator.IsTrue(cycleDto.PresentItems.Count == expectedSkus.Count);  // check the correct number of sku types have been found
                // check the sku count dto and that the right count of each sku were created
                foreach (var skuId in expectedSkus)
                {
                    var kv = assetCategoriesToUse.First(k => k.Key == skuId);
                    var skuCountDto = cycleDto.PresentItems.First(i => i.Id == kv.Key);
                    SkuDtoValidator.SkuSummaryCheck(skuCountDto);
                    Validator.IsTrue(skuCountDto.ItemCount == kv.Value);
                }
            }
            else if (selectedSkus != null)
            {
                Validator.IsTrue(cycleDto.SelectedSkus.Count == selectedSkus.Count);

                var includedSkus = selectedSkus.Intersect(skusToUse.Keys).ToList();

                Validator.IsTrue(cycleDto.CreatedItems.Count == includedSkus.Count);  // check the correct number of sku types have been created
                // check the sku count dto and that the right count of each sku were created
                foreach (var skuId in includedSkus)
                {
                    var kv = skusToUse.First(k => k.Key == skuId);
                    var skuCountDto = cycleDto.CreatedItems.First(i => i.Id == kv.Key);
                    SkuDtoValidator.SkuSummaryCheck(skuCountDto, true);
                    Validator.IsTrue(skuCountDto.ItemCount == kv.Value);
                }


                var expectedSkus = selectedSkus.Intersect(assetCategoriesToUse.Keys).ToList();
                Validator.IsTrue(cycleDto.PresentItems.Count == expectedSkus.Count);  // check the correct number of sku types have been found
                // check the sku count dto and that the right count of each sku were created
                foreach (var skuId in expectedSkus)
                {
                    var kv = assetCategoriesToUse.First(k => k.Key == skuId);
                    var skuCountDto = cycleDto.PresentItems.First(i => i.Id == kv.Key);
                    SkuDtoValidator.SkuSummaryCheck(skuCountDto);
                    Validator.IsTrue(skuCountDto.ItemCount == kv.Value);
                }
            }
            else
            {
                Validator.IsTrue(cycleDto.CreatedItems.Count == skusToUse.Count);  // check the correct number of sku types have been created
                // check the sku count dto and that the right count of each sku were created
                foreach (var kv in skusToUse)
                {
                    var skuCountDto = cycleDto.CreatedItems.First(i => i.Id == kv.Key);
                    SkuDtoValidator.SkuSummaryCheck(skuCountDto, true);
                    Validator.IsTrue(skuCountDto.ItemCount == kv.Value);
                }

                Validator.IsTrue(cycleDto.PresentItems.Count == assetCategoriesToUse.Count);  // check the correct number of sku types have been found
                // check the sku count dto and that the right count of each sku were created
                foreach (var kv in assetCategoriesToUse)
                {
                    var skuCountDto = cycleDto.PresentItems.First(i => i.Id == kv.Key);
                    SkuDtoValidator.SkuSummaryCheck(skuCountDto);
                    Validator.IsTrue(skuCountDto.ItemCount == kv.Value);
                }
            }

            ///////////////////////////////
            // now repeat for location 2 //
            ///////////////////////////////

            // create the cycle count
            var addDto2 = new AddCycleCountDto() { PlaceId = WebRepoContainer.Place2Id };
            if (SkuGroup != null)    // add sku group if required
                addDto2.SkuGroupId = SkuGroup.Id;
            if (selectedSkus != null)   // add selected skus if required
                addDto2.SkuIds = selectedSkus;
            var cycleDto2 = await _cycleCountRepo.CreateCycleCount(addDto2);

            // check the response
            CycleCountDtoValidator.CycleCountDetailcheck(cycleDto2);

            Validator.IsTrue(cycleDto2.PresentItems.Count == 0);
            Validator.IsTrue(cycleDto2.MovedItems.Count == 0);
            Validator.IsTrue(cycleDto2.CreatedItems.Count == 0);

            // build snapshot to resolve
            var addSnapshotDto2 = await SnapshotGenerator.GenerateSgtinSnapshot(skusToUse, null);

            var resolveDto2 = new ResolveCycleCountDto(addSnapshotDto2)
            {
                CycleCountId = cycleDto2.Id
            };

            // resolve the snapshot
            cycleDto2 = await _cycleCountRepo.ResolveCycleCount(resolveDto2);

            // check the response we have create the items
            CycleCountDtoValidator.CycleCountDetailcheck(cycleDto2, true);
            Validator.IsTrue(cycleDto2.PresentItems.Count == 0);
            Validator.IsTrue(cycleDto2.MovedItems.Count == 0);
            if (SkuGroup != null)
            {
                var includedSkus = SkuGroup.Skus.Select(s => s.Id).Intersect(skusToUse.Keys).ToList();
                Validator.IsTrue(cycleDto2.CreatedItems.Count == includedSkus.Count);  // check the correct number of sku types have been created
                // check the sku count dto and that the right count of each sku were created
                foreach (var skuId in includedSkus)
                {
                    var kv = skusToUse.First(k => k.Key == skuId);
                    var skuCountDto = cycleDto2.CreatedItems.First(i => i.Id == kv.Key);
                    SkuDtoValidator.SkuSummaryCheck(skuCountDto, true);
                    Validator.IsTrue(skuCountDto.ItemCount == kv.Value);
                }
                /*
                var expectedSkus = SkuGroup.Skus.Select(s => s.Id).Intersect(assetCategoriesToUse.Keys).ToList();
                Validator.IsTrue(cycleDto.PresentItems.Count == expectedSkus.Count);  // check the correct number of sku types have been found
                // check the sku count dto and that the right count of each sku were created
                foreach (var skuId in expectedSkus)
                {
                    var kv = assetCategoriesToUse.First(k => k.Key == skuId);
                    var skuCountDto = cycleDto2.PresentItems.First(i => i.Id == kv.Key);
                    SkuDtoValidator.SkuSummaryCheck(skuCountDto, true);
                    Validator.IsTrue(skuCountDto.ItemCount == kv.Value);
                }
                */
            }
            else if (selectedSkus != null)
            {
                var includedSkus = selectedSkus.Intersect(skusToUse.Keys).ToList();
                Validator.IsTrue(cycleDto2.CreatedItems.Count == includedSkus.Count);  // check the correct number of sku types have been created
                // check the sku count dto and that the right count of each sku were created
                foreach (var skuId in includedSkus)
                {
                    var kv = skusToUse.First(k => k.Key == skuId);
                    var skuCountDto = cycleDto2.CreatedItems.First(i => i.Id == kv.Key);
                    SkuDtoValidator.SkuSummaryCheck(skuCountDto, true);
                    Validator.IsTrue(skuCountDto.ItemCount == kv.Value);
                }
                /*
                var expectedSkus = selectedSkus.Intersect(assetCategoriesToUse.Keys).ToList();
                Validator.IsTrue(cycleDto.PresentItems.Count == expectedSkus.Count);  // check the correct number of sku types have been found
                // check the sku count dto and that the right count of each sku were created
                foreach (var skuId in expectedSkus)
                {
                    var kv = assetCategoriesToUse.First(k => k.Key == skuId);
                    var skuCountDto = cycleDto2.PresentItems.First(i => i.Id == kv.Key);
                    SkuDtoValidator.SkuSummaryCheck(skuCountDto, true);
                    Validator.IsTrue(skuCountDto.ItemCount == kv.Value);
                }
                */
            }
            else
            {
                Validator.IsTrue(cycleDto2.CreatedItems.Count == skusToUse.Count);  // check the correct number of sku types have been created
                // check the sku count dto and that the right count of each sku were created
                foreach (var kv in skusToUse)
                {
                    var skuCountDto = cycleDto2.CreatedItems.First(i => i.Id == kv.Key);
                    SkuDtoValidator.SkuSummaryCheck(skuCountDto, true);
                    Validator.IsTrue(skuCountDto.ItemCount == kv.Value);
                }
                /*
                Validator.IsTrue(cycleDto.PresentItems.Count == assetCategoriesToUse.Count);  // check the correct number of sku types have been found
                // check the sku count dto and that the right count of each sku were created
                foreach (var kv in assetCategoriesToUse)
                {
                    var skuCountDto = cycleDto2.PresentItems.First(i => i.Id == kv.Key);
                    SkuDtoValidator.SkuSummaryCheck(skuCountDto, true);
                    Validator.IsTrue(skuCountDto.ItemCount == kv.Value);
                }
                */
            }

            ////////////////////////////////////////////////////////////////////////////////
            // Now do cycle count in place 2 again with new tags, present items from last //
            // snapshot in place 2 and moved items from last snapshot in place 1          //
            ////////////////////////////////////////////////////////////////////////////////

            // create the cycle count
            var addDto3 = new AddCycleCountDto() { PlaceId = WebRepoContainer.Place2Id };
            if (SkuGroup != null)    // add sku group if required
                addDto3.SkuGroupId = SkuGroup.Id;
            if (selectedSkus != null)   // add selected skus if required
                addDto3.SkuIds = selectedSkus;
            var cycleDto3 = await _cycleCountRepo.CreateCycleCount(addDto3);

            // check the response
            CycleCountDtoValidator.CycleCountDetailcheck(cycleDto3);

            Validator.IsTrue(cycleDto3.PresentItems.Count == 0);
            Validator.IsTrue(cycleDto3.MovedItems.Count == 0);
            Validator.IsTrue(cycleDto3.CreatedItems.Count == 0);

            // build snapshot to resolve
            var existingTags = new List<string>();
            foreach (var tag in addSnapshotDto.Tags)
            {
                existingTags.Add(tag.TagNumber);
            }
            foreach (var tag in addSnapshotDto2.Tags)
            {
                existingTags.Add(tag.TagNumber);
            }
            var addSnapshotDto3 = await SnapshotGenerator.GenerateSgtinSnapshot(skusToUse, existingTags);

            var resolveDto3 = new ResolveCycleCountDto(addSnapshotDto3)
            {
                CycleCountId = cycleDto3.Id
            };

            // resolve the snapshot
            cycleDto3 = await _cycleCountRepo.ResolveCycleCount(resolveDto3);

            // check the response we have create the items
            CycleCountDtoValidator.CycleCountDetailcheck(cycleDto3, true);

            // check the response we have create the items
            CycleCountDtoValidator.CycleCountDetailcheck(cycleDto3, true);
            if (SkuGroup != null)
            {
                var includedSkus = SkuGroup.Skus.Select(s => s.Id).Intersect(skusToUse.Keys).ToList();

                Validator.IsTrue(cycleDto3.PresentItems.Count == includedSkus.Count);  // check the correct number of sku types have been created
                // check the sku count dto and that the right count of each sku were created
                foreach (var skuId in includedSkus)
                {
                    var kv = skusToUse.First(k => k.Key == skuId);
                    var skuCountDto = cycleDto3.PresentItems.First(i => i.Id == kv.Key);
                    SkuDtoValidator.SkuSummaryCheck(skuCountDto, true);
                    Validator.IsTrue(skuCountDto.ItemCount == kv.Value);
                }

                var expectedSkus = SkuGroup.Skus.Select(s => s.Id).Intersect(assetCategoriesToUse.Keys).ToList();
                Validator.IsTrue(cycleDto3.MovedItems.Count == includedSkus.Count + expectedSkus.Count);  // check the correct number of sku types have been created
                // check the sku count dto and that the right count of each sku were created
                foreach (var skuId in includedSkus)
                {
                    var kv = skusToUse.First(k => k.Key == skuId);
                    var skuCountDto = cycleDto3.MovedItems.First(i => i.Id == kv.Key);
                    SkuDtoValidator.SkuSummaryCheck(skuCountDto);
                    Validator.IsTrue(skuCountDto.ItemCount == kv.Value);
                }
                // check the sku count dto and that the right count of each sku were created
                foreach (var skuId in expectedSkus)
                {
                    var kv = assetCategoriesToUse.First(k => k.Key == skuId);
                    var skuCountDto = cycleDto3.MovedItems.First(i => i.Id == kv.Key);
                    SkuDtoValidator.SkuSummaryCheck(skuCountDto);
                    Validator.IsTrue(skuCountDto.ItemCount == kv.Value);
                }

                Validator.IsTrue(cycleDto3.CreatedItems.Count == includedSkus.Count);  // check the correct number of sku types have been created
                // check the sku count dto and that the right count of each sku were created
                foreach (var skuId in includedSkus)
                {
                    var kv = skusToUse.First(k => k.Key == skuId);
                    var skuCountDto = cycleDto3.CreatedItems.First(i => i.Id == kv.Key);
                    SkuDtoValidator.SkuSummaryCheck(skuCountDto, true);
                    Validator.IsTrue(skuCountDto.ItemCount == kv.Value);
                }
            }
            else if (selectedSkus != null)
            {
                var includedSkus = selectedSkus.Intersect(skusToUse.Keys).ToList();

                Validator.IsTrue(cycleDto3.PresentItems.Count == includedSkus.Count);  // check the correct number of sku types have been created
                // check the sku count dto and that the right count of each sku were created
                foreach (var skuId in includedSkus)
                {
                    var kv = skusToUse.First(k => k.Key == skuId);
                    var skuCountDto = cycleDto3.PresentItems.First(i => i.Id == kv.Key);
                    SkuDtoValidator.SkuSummaryCheck(skuCountDto, true);
                    Validator.IsTrue(skuCountDto.ItemCount == kv.Value);
                }

                var expectedSkus = selectedSkus.Intersect(assetCategoriesToUse.Keys).ToList();
                Validator.IsTrue(cycleDto3.MovedItems.Count == includedSkus.Count + expectedSkus.Count);  // check the correct number of sku types have been created
                // check the sku count dto and that the right count of each sku were created
                foreach (var skuId in includedSkus)
                {
                    var kv = skusToUse.First(k => k.Key == skuId);
                    var skuCountDto = cycleDto3.MovedItems.First(i => i.Id == kv.Key);
                    SkuDtoValidator.SkuSummaryCheck(skuCountDto,true);
                    Validator.IsTrue(skuCountDto.ItemCount == kv.Value);
                }
                // check the sku count dto and that the right count of each sku were created
                foreach (var skuId in expectedSkus)
                {
                    var kv = assetCategoriesToUse.First(k => k.Key == skuId);
                    var skuCountDto = cycleDto3.MovedItems.First(i => i.Id == kv.Key);
                    SkuDtoValidator.SkuSummaryCheck(skuCountDto);
                    Validator.IsTrue(skuCountDto.ItemCount == kv.Value);
                }


                Validator.IsTrue(cycleDto3.CreatedItems.Count == includedSkus.Count);  // check the correct number of sku types have been created
                // check the sku count dto and that the right count of each sku were created
                foreach (var skuId in includedSkus)
                {
                    var kv = skusToUse.First(k => k.Key == skuId);
                    var skuCountDto = cycleDto3.CreatedItems.First(i => i.Id == kv.Key);
                    SkuDtoValidator.SkuSummaryCheck(skuCountDto, true);
                    Validator.IsTrue(skuCountDto.ItemCount == kv.Value);
                }
            }
            else
            {
                Validator.IsTrue(cycleDto3.PresentItems.Count == skusToUse.Count);  // check the correct number of sku types have been created
                // check the sku count dto and that the right count of each sku were created
                foreach (var kv in skusToUse)
                {
                    var skuCountDto = cycleDto3.PresentItems.First(i => i.Id == kv.Key);
                    SkuDtoValidator.SkuSummaryCheck(skuCountDto, true);
                    Validator.IsTrue(skuCountDto.ItemCount == kv.Value);
                }
                
                Validator.IsTrue(cycleDto3.MovedItems.Count == skusToUse.Count + assetCategoriesToUse.Count);  // check the correct number of sku types have been created
                // check the sku count dto and that the right count of each sku were created
                foreach (var kv in skusToUse)
                {
                    var skuCountDto = cycleDto3.MovedItems.First(i => i.Id == kv.Key);
                    SkuDtoValidator.SkuSummaryCheck(skuCountDto, true);
                    Validator.IsTrue(skuCountDto.ItemCount == kv.Value);
                }
                // check the sku count dto and that the right count of each sku were created
                foreach (var kv in assetCategoriesToUse)
                {
                    var skuCountDto = cycleDto3.MovedItems.First(i => i.Id == kv.Key);
                    SkuDtoValidator.SkuSummaryCheck(skuCountDto);
                    Validator.IsTrue(skuCountDto.ItemCount == kv.Value);
                }

                Validator.IsTrue(cycleDto3.CreatedItems.Count == skusToUse.Count);  // check the correct number of sku types have been created
                // check the sku count dto and that the right count of each sku were created
                foreach (var kv in skusToUse)
                {
                    var skuCountDto = cycleDto3.CreatedItems.First(i => i.Id == kv.Key);
                    SkuDtoValidator.SkuSummaryCheck(skuCountDto, true);
                    Validator.IsTrue(skuCountDto.ItemCount == kv.Value);
                }
            }

            var t2 = DateTime.UtcNow;
            var totalTime = t2 - t1;
        }
    }
}
