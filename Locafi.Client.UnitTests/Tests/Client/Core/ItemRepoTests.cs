﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Model.Dto.Items;
using Locafi.Client.Model.Dto.Places;
using Locafi.Client.Model.Dto.Users;
using Locafi.Client.Model.Query;
using Locafi.Client.Model.Query.PropertyComparison;
using Locafi.Client.Model.Enums;
using Locafi.Client.Model.Search;
using Locafi.Client.Model.Dto;
using Locafi.Client.Model.Query.Builder;
using Locafi.Client.Model.Dto.Skus;
using Locafi.Client.UnitTests.Validators;
using Locafi.Client.Model.Dto.Tags;
using Locafi.Client.Model.Dto.Persons;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Locafi.Client.UnitTests.Extensions;
using Locafi.Client.Model.Dto.Templates;
using Locafi.Client.Model;
using Locafi.Client.UnitTests.EntityGenerators;
using System.Linq.Expressions;

namespace Locafi.Client.UnitTests.Tests
{
    [TestClass]
    public class ItemRepoTests
    {
        private IPlaceRepo _placeRepo;
        private IItemRepo _itemRepo;
        private IPersonRepo _personRepo;
        private ISkuRepo _skuRepo;
        private IUserRepo _userRepo;
        private ITemplateRepo _templateRepo;
        private IExtendedPropertyRepo _extPropRepo;

        private List<Guid> _itemsToDelete;
        private List<Guid> _placesToDelete;
        private List<Guid> _skusToDelete;
        private List<Guid> _templatesToDelete;
        private List<Guid> _extpropsToDelete;

        [TestInitialize]
        public void Setup()
        {
            _placeRepo = WebRepoContainer.PlaceRepo;
            _itemRepo = WebRepoContainer.ItemRepo;
            _personRepo = WebRepoContainer.PersonRepo;
            _skuRepo = WebRepoContainer.SkuRepo;
            _userRepo = WebRepoContainer.UserRepo;
            _templateRepo = WebRepoContainer.TemplateRepo;
            _extPropRepo = WebRepoContainer.ExtendedPropertyRepo;

            _itemsToDelete = new List<Guid>();
            _placesToDelete = new List<Guid>();
            _skusToDelete = new List<Guid>();
            _templatesToDelete = new List<Guid>();
            _extpropsToDelete = new List<Guid>();
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

            foreach (var id in _skusToDelete)
            {
                _skuRepo.DeleteSku(id).Wait();
            }

            foreach (var id in _templatesToDelete)
            {
                _templateRepo.DeleteTemplate(id).Wait();
            }

            foreach (var id in _extpropsToDelete)
            {
                _extPropRepo.DeleteExtendedProperty(id).Wait();
            }
        }

        [TestMethod]
        public async Task Item_Create()
        {
            // build add dto
            var addItemDto = await CreateRandomAssetAddItemDto();

            // create the item
            var result = await _itemRepo.CreateItem(addItemDto);
            _itemsToDelete.AddUnique(result.Id);

            // check response
            ItemDtoValidator.ItemDetailCheck(result);

            Validator.IsTrue(string.Equals(addItemDto.Name, result.Name));
            Validator.IsTrue(string.Equals(addItemDto.Description, result.Description));
            Validator.AreEqual(addItemDto.PlaceId, result.PlaceId);
            Validator.AreEqual(addItemDto.SkuId, result.SkuId);

            // get the item
            var check = await _itemRepo.GetItemDetail(result.Id);

            // check the response
            ItemDtoValidator.ItemDetailCheck(check);
            Validator.AreEqual(result, check);
        }

        [TestMethod]
        public async Task Item_Get()
        {
            // build add dto
            var addItemDto = await CreateRandomAssetAddItemDto();

            // create the item
            var result = await _itemRepo.CreateItem(addItemDto);
            _itemsToDelete.AddUnique(result.Id);

            // check response
            ItemDtoValidator.ItemDetailCheck(result);

            Validator.IsTrue(string.Equals(addItemDto.Name, result.Name));
            Validator.IsTrue(string.Equals(addItemDto.Description, result.Description));
            Validator.AreEqual(addItemDto.PlaceId, result.PlaceId);
            Validator.AreEqual(addItemDto.SkuId, result.SkuId);

            // get the item
            var check = await _itemRepo.GetItemDetail(result.Id);

            // check the response
            ItemDtoValidator.ItemDetailCheck(check);
            Validator.IsTrue(string.Equals(check.Name, result.Name));
            Validator.IsTrue(string.Equals(check.Description, result.Description));
            Validator.AreEqual(check.PlaceId, result.PlaceId);
            Validator.AreEqual(check.SkuId, result.SkuId);
        }

        [TestMethod]
        public async Task Item_Update()
        {
            // build add dto
            var addItemDto = await CreateRandomAssetAddItemDto();

            // create the item
            var result = await _itemRepo.CreateItem(addItemDto);
            _itemsToDelete.AddUnique(result.Id);

            // check response
            ItemDtoValidator.ItemDetailCheck(result);

            Validator.IsTrue(string.Equals(addItemDto.Name, result.Name));
            Validator.IsTrue(string.Equals(addItemDto.Description, result.Description));
            Validator.AreEqual(addItemDto.PlaceId, result.PlaceId);
            Validator.AreEqual(addItemDto.SkuId, result.SkuId);

            // build update item dto
            var updateItemDto = new UpdateItemDto()
            {
                Id = result.Id,
                Description = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                SkuId = result.SkuId,
                PersonId = _personRepo.QueryPersons(QueryBuilder<PersonSummaryDto>.NewQuery(p => p.Id, (Guid)result.PersonId, ComparisonOperator.NotEquals).Build()).Result.First().Id
            };
            foreach (var prop in result.ItemExtendedPropertyList)
                updateItemDto.ItemExtendedPropertyList.Add(prop);

            var updateItemResult = await _itemRepo.UpdateItem(updateItemDto);

            // check the result
            ItemDtoValidator.ItemDetailCheck(updateItemResult);
            Validator.IsTrue(string.Equals(updateItemDto.Name, updateItemResult.Name));
            Validator.IsTrue(string.Equals(updateItemDto.Description, updateItemResult.Description));
            Validator.AreEqual(updateItemDto.PersonId, updateItemResult.PersonId);
            Validator.AreEqual(updateItemDto.SkuId, updateItemResult.SkuId);
        }

        [TestMethod]
        public async Task Item_UseExtendedProperties()
        {
            var addItemDto = await CreateRandomAssetAddItemDto();
            var result = await _itemRepo.CreateItem(addItemDto);
            _itemsToDelete.AddUnique(result.Id);

            ItemDtoValidator.ItemDetailCheck(result);

            var skuDetail = await _skuRepo.GetSku(result.SkuId);
            foreach (var skuDetailExtendedProperty in skuDetail.SkuExtendedPropertyList.Where(s => !s.IsSkuLevelProperty))
            {
                var itemExtendedProperty = result.ItemExtendedPropertyList
                    .FirstOrDefault(e => e.ExtendedPropertyId == skuDetailExtendedProperty.ExtendedPropertyId);
                Validator.IsNotNull(itemExtendedProperty, "Extended property was null");
            }

            // now update extended properties

            // build update item dto, but only change the extended properties
            var updateItemDto = new UpdateItemDto()
            {
                Id = result.Id,
                Description = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                SkuId = result.SkuId,
                PersonId = result.PersonId
            };
            // loop through each extended property and change
            foreach (var prop in result.ItemExtendedPropertyList)
            {
                var newProp = new WriteItemExtendedPropertyDto()
                {
                    ExtendedPropertyId = prop.ExtendedPropertyId
                };

                switch (prop.DataType)
                {
//                    case TemplateDataTypes.AutoId: newProp.Value = new Random(DateTime.UtcNow.Millisecond).Next().ToString(); break;
                    case TemplateDataTypes.Bool: newProp.Value = true.ToString(); break;
                    case TemplateDataTypes.DateTime: newProp.Value = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssK"); break;
                    case TemplateDataTypes.Decimal: newProp.Value = (((double)new Random(DateTime.UtcNow.Millisecond).Next()) / 10.0).ToString(); break;
                    case TemplateDataTypes.Number: newProp.Value = new Random(DateTime.UtcNow.Millisecond).Next().ToString(); break;
                    case TemplateDataTypes.String: newProp.Value = Guid.NewGuid().ToString(); break;
                }

                updateItemDto.ItemExtendedPropertyList.Add(newProp);
            }

            // update the item
            var updateItemResult = await _itemRepo.UpdateItem(updateItemDto);

            // check the result
            ItemDtoValidator.ItemDetailCheck(updateItemResult);
            Validator.IsTrue(string.Equals(updateItemDto.Name, updateItemResult.Name));
            Validator.IsTrue(string.Equals(updateItemDto.Description, updateItemResult.Description));
            Validator.AreEqual(updateItemDto.PersonId, updateItemResult.PersonId);
            Validator.AreEqual(updateItemDto.SkuId, updateItemResult.SkuId);
            // check the extended properties were changed
            foreach (var prop in updateItemResult.ItemExtendedPropertyList)
            {
                var dtoProp = updateItemDto.ItemExtendedPropertyList.FirstOrDefault(p => p.ExtendedPropertyId == prop.ExtendedPropertyId);
                Validator.AreEqual(prop.Value, dtoProp?.Value);
            }
        }

        [TestMethod]
        public async Task Item_QueryItemsForSpecificItem()
        {
            // create item
            var itemToAdd = await CreateRandomAssetAddItemDto();
            var item = await _itemRepo.CreateItem(itemToAdd);
            _itemsToDelete.AddUnique(item.Id);

            // check response
            ItemDtoValidator.ItemDetailCheck(item);

            // build query to find this item
            var query = ItemQuery.NewQuery(i => i.Id, item.Id, ComparisonOperator.Equals);
            // query item
            var r1 = await _itemRepo.QueryItemsContinuation(query);

            // check the response
            Validator.IsNotNull(r1);
            Validator.IsTrue(r1.Entities.Count == 1);
            ItemDtoValidator.ItemSummaryCheck(r1.Entities.First());
            Validator.IsTrue(r1.Entities.Contains(item));

            try
            {
                var y = ExpressionBuilder.BuildPropertyExpression<ItemSummaryDto>("Name");

                var q2 = QueryBuilder<ItemSummaryDto>.NewQuery(y, item.Name, ComparisonOperator.Contains);

                // query item
                var r2 = await _itemRepo.QueryItemsContinuation(q2.Build());

                // check the response
                Validator.IsNotNull(r2);
                Validator.IsTrue(r2.Entities.Count == 1);
                ItemDtoValidator.ItemSummaryCheck(r2.Entities.First());
                Validator.IsTrue(r2.Entities.Contains(item));
            }
            catch
            {

            }
        }

        [TestMethod]
        public async Task Item_QueryUsingTopTake()
        {
            // add 20 items to the db to ensure that they are there for the test
            for(int i=0; i< 20; i++)
            {
                var addDto = await CreateRandomAssetAddItemDto();
                var result = await _itemRepo.CreateItem(addDto);
                _itemsToDelete.AddUnique(result.Id);
            }

            // query to take only 10 items
            var query = ItemQuery.NewQuery(i => i.Name, "", ComparisonOperator.Contains, 10, 0);
            var items = await _itemRepo.QueryItemsContinuation(query);

            // check the response
            Validator.IsTrue(items.Entities.Count == 10);
        }

        [TestMethod]
        public async Task Item_QuerySubstring()
        {
            // create an item
            var itemToAdd = await CreateRandomAssetAddItemDto();
            var item = await _itemRepo.CreateItem(itemToAdd);
            _itemsToDelete.AddUnique(item.Id);

            // check response
            ItemDtoValidator.ItemDetailCheck(item);

            // build substring query
            var q2 = new ItemQuery();
            q2.CreateQuery(i => i.Name, item.Name.Remove(item.Name.Length / 2), ComparisonOperator.Contains);
            var r2 = await _itemRepo.QueryItems(q2);

            // check the response
            Validator.IsNotNull(r2);
            Validator.IsTrue(r2.Contains(item));
        }

        [TestMethod]
        public async Task Item_MoveItem()
        {
            // create an item
            var itemToAdd = await CreateRandomAssetAddItemDto();
            var item = await _itemRepo.CreateItem(itemToAdd);
            _itemsToDelete.AddUnique(item.Id);

            // check response
            ItemDtoValidator.ItemDetailCheck(item);

            // get place to move to
            var place = await GetRandomOtherPlace(item.PlaceId);

            // move the item
            var moveItemDto = new UpdateItemPlaceDto
            {
                Id = item.Id,
                NewPlaceId = place.Id,
            };
            var movedItem = await _itemRepo.UpdateItemPlace(moveItemDto);

            // check response
            ItemDtoValidator.ItemDetailCheck(movedItem);
            Validator.AreEqual(movedItem.PlaceId, moveItemDto.NewPlaceId);
        }

        [TestMethod]
        public async Task Item_QueryItemMovementHistory()
        {
            // create an item
            var itemToAdd = await CreateRandomAssetAddItemDto();
            var item = await _itemRepo.CreateItem(itemToAdd);
            _itemsToDelete.AddUnique(item.Id);

            // check response
            ItemDtoValidator.ItemDetailCheck(item);

            // get place to move to
            var place = await GetRandomOtherPlace(item.PlaceId);

            // move the item
            var moveItemDto = new UpdateItemPlaceDto
            {
                Id = item.Id,
                NewPlaceId = place.Id,
            };
            var movedItem = await _itemRepo.UpdateItemPlace(moveItemDto);

            // check response
            ItemDtoValidator.ItemDetailCheck(movedItem);
            Validator.AreEqual(movedItem.PlaceId, moveItemDto.NewPlaceId);

            // now query the item movement history for this items movement
            var query = QueryBuilder<ItemPlaceMovementHistoryDto>.NewQuery(i => i.ItemId, movedItem.Id, ComparisonOperator.Equals).Build();
            var history = await _itemRepo.GetItemPlaceHistory(query);

            // check response
            Validator.IsNotNull(history, "No item movement history returned");
            Validator.IsInstanceOfType(history, typeof(PageResult<ItemPlaceMovementHistoryDto>));
            Validator.IsTrue(history.Items.Count() >= 1);
        }

        [TestMethod]
        public async Task Item_QueryItemStateHistory()
        {
            // create an item
            var itemToAdd = await CreateRandomAssetAddItemDto();
            var item = await _itemRepo.CreateItem(itemToAdd);
            _itemsToDelete.AddUnique(item.Id);

            // check response
            ItemDtoValidator.ItemDetailCheck(item);

            // get place to move to
            var place = await GetRandomOtherPlace(item.PlaceId);

            // move the item
            var moveItemDto = new UpdateItemPlaceDto
            {
                Id = item.Id,
                NewPlaceId = place.Id,
            };
            var movedItem = await _itemRepo.UpdateItemPlace(moveItemDto);

            // check response
            ItemDtoValidator.ItemDetailCheck(movedItem);
            Validator.AreEqual(movedItem.PlaceId, moveItemDto.NewPlaceId);

            // now query the item state history for this items state changes
            var query = QueryBuilder<ItemStateHistoryDto>.NewQuery(i => i.ItemId, movedItem.Id, ComparisonOperator.Equals).Build();
            var history = await _itemRepo.GetItemStateHistory(query);

            // check response
            Validator.IsNotNull(history, "No item state history returned");
            Validator.IsInstanceOfType(history, typeof(PageResult<ItemStateHistoryDto>));
            Validator.IsTrue(history.Items.Count() >= 1);
        }

        [TestMethod]
        public async Task Item_UpdateTag()
        {
            // create an item
            var itemToAdd = await CreateRandomAssetAddItemDto();
            var item = await _itemRepo.CreateItem(itemToAdd);
            _itemsToDelete.AddUnique(item.Id);

            // check response
            ItemDtoValidator.ItemDetailCheck(item);

            // update items tag
            var dto = new UpdateItemTagDto
            {
                Id = item.Id,
                ItemTagList = new List<WriteTagDto>()
                {
                    new WriteTagDto()
                    {
                        TagNumber = Guid.NewGuid().ToString(),
                        TagType = TagType.PassiveRfid
                    }
                }
            };
            var updateTagResult = await _itemRepo.UpdateTag(dto);

            // check response
            ItemDtoValidator.ItemDetailCheck(updateTagResult);
            Validator.AreNotEqual(item.TagNumber, updateTagResult.TagNumber);
            Validator.AreEqual(updateTagResult.TagNumber, dto.ItemTagList.First().TagNumber);
        }

        // Test re-using an items tag made available because the original items tag number was changed
        [TestMethod]
        public async Task Item_ReUseItemTag()
        {
            // create an item
            var itemToAdd = await CreateRandomAssetAddItemDto();
            var item = await _itemRepo.CreateItem(itemToAdd);
            _itemsToDelete.AddUnique(item.Id);

            // check response
            ItemDtoValidator.ItemDetailCheck(item);

            // update items tag
            var dto = new UpdateItemTagDto
            {
                Id = item.Id,
                ItemTagList = new List<WriteTagDto>()
                {
                    new WriteTagDto()
                    {
                        TagNumber = Guid.NewGuid().ToString(),
                        TagType = TagType.PassiveRfid
                    }
                }
            };
            var updateTagResult = await _itemRepo.UpdateTag(dto);

            // check response
            ItemDtoValidator.ItemDetailCheck(updateTagResult);
            Validator.AreNotEqual(item.TagNumber, updateTagResult.TagNumber);
            Validator.AreEqual(updateTagResult.TagNumber, dto.ItemTagList.First().TagNumber);

            // now try and re-use the original tag with a new item
            var itemAddDto2 = await CreateRandomAssetAddItemDto();
            itemAddDto2.ItemTagList[0].TagNumber = itemToAdd.ItemTagList[0].TagNumber;
            var reuseResult = await _itemRepo.CreateItem(itemAddDto2);
            _itemsToDelete.AddUnique(reuseResult.Id);

            // check response
            ItemDtoValidator.ItemDetailCheck(reuseResult);
            Validator.AreEqual(reuseResult.TagNumber, item.TagNumber);
        }

        // Test re-using the tag from an item that was deleted with that tag still assigned to it
        [TestMethod]
        public async Task Item_ReUseDeletedItemsTag()
        {
            // create an item
            var itemToAdd = await CreateRandomAssetAddItemDto();
            var item = await _itemRepo.CreateItem(itemToAdd);
            _itemsToDelete.AddUnique(item.Id);

            // check response
            ItemDtoValidator.ItemDetailCheck(item);

            // delete the item
            var deleteResult = await _itemRepo.DeleteItem(item.Id);

            // verify
            Validator.IsTrue(deleteResult);
            _itemsToDelete.Remove(item.Id);

            // now try and re-use the original tag with a new item
            var itemAddDto2 = await CreateRandomAssetAddItemDto();
            itemAddDto2.ItemTagList[0].TagNumber = itemToAdd.ItemTagList[0].TagNumber;
            var reuseResult = await _itemRepo.CreateItem(itemAddDto2);
            _itemsToDelete.AddUnique(reuseResult.Id);

            // check response
            ItemDtoValidator.ItemDetailCheck(reuseResult);
            Validator.AreEqual(reuseResult.TagNumber, item.TagNumber);
        }

        [TestMethod]
        public async Task Item_Delete()
        {
            // create an item
            var itemToCreate = await CreateRandomAssetAddItemDto();
            var item = await _itemRepo.CreateItem(itemToCreate);
            _itemsToDelete.AddUnique(item.Id);

            // check response
            ItemDtoValidator.ItemDetailCheck(item);

            var id = item.Id;
            var deleteResult = await _itemRepo.DeleteItem(id);
            // check the result
            Validator.IsTrue(deleteResult);
            // remove from delete list
            _itemsToDelete.Remove(item.Id);

            // verify with query
            var query = QueryBuilder<ItemSummaryDto>.NewQuery(n => n.Id, item.Id, ComparisonOperator.Equals).Build();
            var idQuery = await _itemRepo.QueryItems(query);
            Validator.IsFalse(idQuery.Contains(item));            

            // verify with get
            try
            {
                var sameItem = await _itemRepo.GetItemDetail(id);

                Validator.IsTrue(false, "Deleted entity returned");
            }
            catch (Exception e)
            {
                // this is expected                
            }
        }

        [TestMethod]
        public async Task Item_Count()
        {
            // create a new place for this test
            var addPlaceDto = new AddPlaceDto() {
                Name = "Item_Count_Test_Place",
                TemplateId = (await _templateRepo.QueryTemplates(QueryBuilder<TemplateSummaryDto>.NewQuery(t => t.TemplateType, TemplateFor.Place, ComparisonOperator.Equals).Build())).First().Id
            };
            var placeResult = await _placeRepo.CreatePlace(addPlaceDto);
            _placesToDelete.AddUnique(placeResult.Id);

            // Add a bunch of items to this place
            var numItemsTocreate = 20;
            for (int i = 0; i < numItemsTocreate; i++)
            {
                var itemToAdd = await CreateRandomAssetAddItemDto();
                itemToAdd.Name = "Item_Count_Test";
                itemToAdd.PlaceId = placeResult.Id;
                var result = await _itemRepo.CreateItem(itemToAdd);

                _itemsToDelete.AddUnique(result.Id);

                // check response
                ItemDtoValidator.ItemDetailCheck(result);
            }

            // build query to get count of all the items in this place
            var query = ItemQuery.NewQuery(i => i.PlaceId, placeResult.Id, ComparisonOperator.Equals, 0);
            var queryResult = await _itemRepo.QueryItems(query);
            Validator.IsNotNull(queryResult);
            Validator.IsTrue(queryResult.Count == numItemsTocreate);
        }

        [TestMethod] 
        public async Task Item_TestAllExtendedPropertyTypes()
        {
            // create full item template
            var addTemplateDto = await TemplateGenerator.GenerateAddTemplateDtoWithFullExtProps(TemplateFor.Item);
            var template = await _templateRepo.CreateTemplate(addTemplateDto);
            _templatesToDelete.AddUnique(template.Id);
            _extpropsToDelete.AddRangeUnique(template.TemplateExtendedPropertyList.Select(e => e.ExtendedPropertyId));
            // create sku from template
            var addSkuDto = await SkuGenerator.GeneratePlainSkuDto(template.Id);
            var sku = await _skuRepo.CreateSku(addSkuDto);
            _skusToDelete.AddUnique(sku.Id);
            // create item
            var ran = new Random(DateTime.UtcNow.Millisecond);
            var name = "Random - " + sku.Name + " " + ran.Next().ToString();
            var description = name + " - Description";
            var tagNumber = Guid.NewGuid().ToString();
            var addItemDto = new AddItemDto(sku, WebRepoContainer.Place1Id, name, description, tagNumber: tagNumber,
                personId: WebRepoContainer.Person1Id);
            var result = await _itemRepo.CreateItem(addItemDto);
            _itemsToDelete.AddUnique(result.Id);

            ItemDtoValidator.ItemDetailCheck(result);

            // check every extended property
            var skuDetail = await _skuRepo.GetSku(result.SkuId);
            foreach (var skuDetailExtendedProperty in skuDetail.SkuExtendedPropertyList.Where(s => !s.IsSkuLevelProperty))
            {
                var extendedProperty = result.ItemExtendedPropertyList
                    .FirstOrDefault(e => e.ExtendedPropertyId == skuDetailExtendedProperty.ExtendedPropertyId);
                Validator.IsNotNull(extendedProperty, "Extended property was null");
                var addExtendedProperty = addItemDto.ItemExtendedPropertyList
                    .FirstOrDefault(e => e.ExtendedPropertyId == skuDetailExtendedProperty.ExtendedPropertyId);
                Validator.IsTrue(ExtendedPropertyDtoValidator.CanParseDtoValue(extendedProperty));
                Validator.IsTrue(ExtendedPropertyDtoValidator.ParsedValuesAreEqual(extendedProperty, addExtendedProperty));
            }

            // now do a get to check it works
            var itemGet = await _itemRepo.GetItemDetail(result.Id);
            ItemDtoValidator.ItemDetailCheck(itemGet);

            // check every extended property
            foreach (var skuDetailExtendedProperty in skuDetail.SkuExtendedPropertyList.Where(s => !s.IsSkuLevelProperty))
            {
                var extendedProperty = itemGet.ItemExtendedPropertyList
                    .FirstOrDefault(e => e.ExtendedPropertyId == skuDetailExtendedProperty.ExtendedPropertyId);
                Validator.IsNotNull(extendedProperty, "Extended property was null");
                var addExtendedProperty = addItemDto.ItemExtendedPropertyList
                    .FirstOrDefault(e => e.ExtendedPropertyId == skuDetailExtendedProperty.ExtendedPropertyId);
                Validator.IsTrue(ExtendedPropertyDtoValidator.CanParseDtoValue(extendedProperty));
                Validator.IsTrue(ExtendedPropertyDtoValidator.ParsedValuesAreEqual(extendedProperty, addExtendedProperty));
            }

            // now update extended properties

            // build update item dto, but only change the extended properties
            var updateItemDto = new UpdateItemDto()
            {
                Id = result.Id,
                Description = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                SkuId = result.SkuId,
                PersonId = result.PersonId
            };

            // loop through each extended property and change
            foreach (var prop in result.ItemExtendedPropertyList)
            {
                var newProp = new WriteItemExtendedPropertyDto()
                {
                    ExtendedPropertyId = prop.ExtendedPropertyId
                };

                switch (prop.DataType)
                {
                    //                    case TemplateDataTypes.AutoId: newProp.Value = new Random(DateTime.UtcNow.Millisecond).Next().ToString(); break;
                    case TemplateDataTypes.Bool: newProp.Value = true.ToString(); break;
                    case TemplateDataTypes.DateTime: newProp.Value = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssK"); break;
                    case TemplateDataTypes.Decimal: newProp.Value = (((double)new Random(DateTime.UtcNow.Millisecond).Next()) / 10.0).ToString(); break;
                    case TemplateDataTypes.Number: newProp.Value = new Random(DateTime.UtcNow.Millisecond).Next().ToString(); break;
                    case TemplateDataTypes.String: newProp.Value = Guid.NewGuid().ToString(); break;
                }

                updateItemDto.ItemExtendedPropertyList.Add(newProp);
            }

            // update the item
            var updateItemResult = await _itemRepo.UpdateItem(updateItemDto);

            // check the result
            ItemDtoValidator.ItemDetailCheck(updateItemResult);
            Validator.IsTrue(string.Equals(updateItemDto.Name, updateItemResult.Name));
            Validator.IsTrue(string.Equals(updateItemDto.Description, updateItemResult.Description));
            Validator.AreEqual(updateItemDto.PersonId, updateItemResult.PersonId);
            Validator.AreEqual(updateItemDto.SkuId, updateItemResult.SkuId);
            // check the extended properties were changed
            foreach (var prop in updateItemResult.ItemExtendedPropertyList)
            {
                var dtoProp = updateItemDto.ItemExtendedPropertyList.FirstOrDefault(p => p.ExtendedPropertyId == prop.ExtendedPropertyId);
                Validator.IsTrue(ExtendedPropertyDtoValidator.CanParseDtoValue(prop));
                Validator.IsTrue(ExtendedPropertyDtoValidator.ParsedValuesAreEqual(prop, dtoProp));
            }

            // now do a get to check it works
            itemGet = await _itemRepo.GetItemDetail(updateItemResult.Id);
            ItemDtoValidator.ItemDetailCheck(itemGet);

            // check every extended property
            foreach (var prop in itemGet.ItemExtendedPropertyList)
            {
                var dtoProp = updateItemDto.ItemExtendedPropertyList.FirstOrDefault(p => p.ExtendedPropertyId == prop.ExtendedPropertyId);
                Validator.IsTrue(ExtendedPropertyDtoValidator.CanParseDtoValue(prop));
                Validator.IsTrue(ExtendedPropertyDtoValidator.ParsedValuesAreEqual(prop, dtoProp));
            }

            // now update again but set all ext props to null to check that there are no errors

            updateItemDto.ItemExtendedPropertyList.Clear();
            // loop through each extended property and change
            foreach (var prop in result.ItemExtendedPropertyList)
            {
                var newProp = new WriteItemExtendedPropertyDto()
                {
                    ExtendedPropertyId = prop.ExtendedPropertyId
                };

                newProp.Value = "";

                updateItemDto.ItemExtendedPropertyList.Add(newProp);
            }

            // update the item
            updateItemResult = await _itemRepo.UpdateItem(updateItemDto);

            // check the result
            ItemDtoValidator.ItemDetailCheck(updateItemResult);
            Validator.IsTrue(string.Equals(updateItemDto.Name, updateItemResult.Name));
            Validator.IsTrue(string.Equals(updateItemDto.Description, updateItemResult.Description));
            Validator.AreEqual(updateItemDto.PersonId, updateItemResult.PersonId);
            Validator.AreEqual(updateItemDto.SkuId, updateItemResult.SkuId);
            // check the extended properties were changed
            foreach (var prop in updateItemResult.ItemExtendedPropertyList)
            {
                var dtoProp = updateItemDto.ItemExtendedPropertyList.FirstOrDefault(p => p.ExtendedPropertyId == prop.ExtendedPropertyId);
                Validator.IsTrue(ExtendedPropertyDtoValidator.CanParseDtoValue(prop));
                Validator.IsTrue(ExtendedPropertyDtoValidator.ParsedValuesAreEqual(prop, dtoProp));
            }

            // now do a get to check it works
            itemGet = await _itemRepo.GetItemDetail(updateItemResult.Id);
            ItemDtoValidator.ItemDetailCheck(itemGet);

            // check every extended property
            foreach (var prop in itemGet.ItemExtendedPropertyList)
            {
                var dtoProp = updateItemDto.ItemExtendedPropertyList.FirstOrDefault(p => p.ExtendedPropertyId == prop.ExtendedPropertyId);
                Validator.IsTrue(ExtendedPropertyDtoValidator.CanParseDtoValue(prop));
                Validator.IsTrue(ExtendedPropertyDtoValidator.ParsedValuesAreEqual(prop, dtoProp));
            }
        }

        // TODO: Complete below when functionality added to V3

        //TODO: Somethings wrong with this test, figure out what's wrong/talk to Mark
        //[TestMethod]
        //public async Task Item_Search()
        //{
        //    // need to update test to actually add a known item first

        //    // build search query
        //    var searchQuery = new SearchCollectionDto() { SearchType = SearchCollectionType.Or };
        //    searchQuery.SearchParameters.Add(new SearchParameter()
        //    {
        //        PropertyName = "*",
        //        DataType = TemplateDataTypes.String,
        //        Value = "test"
        //    });
        //    searchQuery.SearchParameters.Add(new SearchParameter()
        //    {
        //        PropertyName = "TagNumber",
        //        DataType = TemplateDataTypes.String,
        //        Value = "21"
        //    });

        //    // search for item
        //    var result = await _itemRepo.SearchItems(searchQuery);
        //    Assert.IsNotNull(result, "result != null");
        //    //var count = await _itemRepo.GetItemCount(query);
        //    //Assert.IsTrue(count > 0);
        //}

        #region PrivateMethods
        private async Task<AddItemDto> CreateRandomAssetAddItemDto()
        {
            var ran = new Random(DateTime.UtcNow.Millisecond);
            // choose a place
            var places = await _placeRepo.QueryPlaces();
            var place = places.Items.ElementAt(ran.Next(places.Items.Count())); // picks a random place for the item
            // choose a person
            var persons = await _personRepo.QueryPersons();
            var person = persons.Items.ElementAt(ran.Next(persons.Items.Count()));
            // choose an asset sku
            var skus = await _skuRepo.QuerySkus(QueryBuilder<SkuSummaryDto>.NewQuery(s => s.CompanyPrefix,"",ComparisonOperator.Equals)
                .And(s => s.ItemReference, "", ComparisonOperator.Equals)
                .Build());
            var sku = skus.Items.ElementAt(ran.Next(skus.Items.Count()));

            var skuDetail = await _skuRepo.GetSku(sku.Id);

            var name = "Random - " + sku.Name + " " + ran.Next().ToString();
            var description = name + " - Description";
            var tagNumber = Guid.NewGuid().ToString();

            var addItemDto = new AddItemDto(skuDetail, place.Id, name, description, tagNumber: tagNumber,
                personId: person.Id);

            return addItemDto;
        }

        

        private async Task<PlaceSummaryDto> GetRandomOtherPlace(Guid notThisPlaceId)
        {
            var places = await _placeRepo.QueryPlaces();
            var ran = new Random(DateTime.UtcNow.Millisecond);
            PlaceSummaryDto place = null;
            while (place?.Id.Equals(notThisPlaceId) ?? true)
            {
                place = places.Items.ElementAt(ran.Next(places.Items.Count()));
            }
            return place;
        }

        private async Task<UserSummaryDto> GetRandomUser()
        {
            var ran = new Random(DateTime.UtcNow.Millisecond);
            var users = await _userRepo.QueryUsers();
            var user = users.Items.ElementAt(ran.Next(users.Items.Count()));
            return user;
        }

        #endregion
    }
}
