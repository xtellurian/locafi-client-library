using System;
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
using Locafi.Client.Model.Dto.FileUpload;
using Locafi.Client.Model.Query.Builder;
using Locafi.Client.Model.Dto.Skus;
using Locafi.Client.UnitTests.Validators;
using Locafi.Client.Model.Dto.Tags;
using Locafi.Client.Model.Dto.Persons;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Locafi.Client.UnitTests.Extensions;
using Locafi.Client.Model.Dto.Templates;

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
        private ITemplateRepo _teplateRepo;

        private List<Guid> _itemsToDelete;
        private List<Guid> _placesToDelete;

        [TestInitialize]
        public void Setup()
        {
            _placeRepo = WebRepoContainer.PlaceRepo;
            _itemRepo = WebRepoContainer.ItemRepo;
            _personRepo = WebRepoContainer.PersonRepo;
            _skuRepo = WebRepoContainer.SkuRepo;
            _userRepo = WebRepoContainer.UserRepo;
            _teplateRepo = WebRepoContainer.TemplateRepo;

            _itemsToDelete = new List<Guid>();
            _placesToDelete = new List<Guid>();
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
        }

        [TestMethod]
        public async Task Item_Basic_CRUD()
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

            // build update tag dto
            var updateTagDto = new UpdateItemTagDto()
            {
                Id = result.Id,
                ItemTagList = new List<WriteTagDto>()
                {
                    new WriteTagDto()
                    {
                        TagNumber = Guid.NewGuid().ToString(),
                        TagType = TagType.PassiveRfid
                    }
                }
            };
            // update the tag
            var tagUpdateResult = await _itemRepo.UpdateTag(updateTagDto);

            // check the result
            ItemDtoValidator.ItemDetailCheck(tagUpdateResult);
            Validator.IsTrue(string.Equals(addItemDto.Name, tagUpdateResult.Name));
            Validator.IsTrue(string.Equals(addItemDto.Description, tagUpdateResult.Description));
            Validator.AreEqual(addItemDto.PlaceId, tagUpdateResult.PlaceId);
            Validator.AreEqual(addItemDto.SkuId, tagUpdateResult.SkuId);
            Validator.AreNotEqual(tagUpdateResult.ItemTagList[0].TagNumber, result.ItemTagList[0].TagNumber);
            Validator.AreEqual(updateTagDto.ItemTagList[0].TagNumber, tagUpdateResult.ItemTagList[0].TagNumber);

            // build update item place dto
            var updatePlaceDto = new UpdateItemPlaceDto()
            {
                Id = result.Id,
                NewPlaceId = _placeRepo.QueryPlaces(QueryBuilder<PlaceSummaryDto>.NewQuery(p => p.Id,result.PlaceId, ComparisonOperator.NotEquals).Build()).Result.First().Id
            };
            var updatePlaceResult = await _itemRepo.UpdateItemPlace(updatePlaceDto);

            // check the result
            ItemDtoValidator.ItemDetailCheck(updatePlaceResult);
            Validator.IsTrue(string.Equals(addItemDto.Name, tagUpdateResult.Name));
            Validator.IsTrue(string.Equals(addItemDto.Description, tagUpdateResult.Description));
            Validator.AreNotEqual(addItemDto.PlaceId, updatePlaceResult.PlaceId);
            Validator.AreEqual(updatePlaceDto.NewPlaceId, updatePlaceResult.PlaceId);
            Validator.AreEqual(addItemDto.SkuId, tagUpdateResult.SkuId);

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

            // cleanup
            var delResult = await _itemRepo.DeleteItem(result.Id);
            Validator.IsTrue(delResult);
        }

        //[TestMethod]
        //public async Task Item_GetDetail()
        //{
        //    var addItemDto = await CreateRandomAssetAddItemDto();

        //    var result = await _itemRepo.CreateItem(addItemDto);

        //    ItemDtoValidator.ItemDetailCheck(result);

        //    Assert.IsTrue(string.Equals(addItemDto.Name, result.Name));
        //    Assert.IsTrue(string.Equals(addItemDto.Description, result.Description));
        //    Assert.AreEqual(addItemDto.PlaceId, result.PlaceId);
        //    Assert.AreEqual(addItemDto.SkuId, result.SkuId);

        //    var check = await _itemRepo.GetItemDetail(result.Id);

        //    ItemDtoValidator.ItemDetailCheck(check);
        //    Assert.AreEqual(result, check);

        //    // cleanup
        //    await _itemRepo.DeleteItem(result.Id);
        //}

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
                    case TemplateDataTypes.AutoId: newProp.Value = new Random().Next().ToString(); break;
                    case TemplateDataTypes.Bool: newProp.Value = true.ToString(); break;
                    case TemplateDataTypes.DateTime: newProp.Value = DateTime.UtcNow.ToString(); break;
                    case TemplateDataTypes.Decimal: newProp.Value = (((double)new Random().Next()) / 10.0).ToString(); break;
                    case TemplateDataTypes.Number: newProp.Value = new Random().Next().ToString(); break;
                    case TemplateDataTypes.String: newProp.Value = Guid.NewGuid().ToString(); break;
                }

                updateItemDto.ItemExtendedPropertyList.Add(prop);
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
        public async Task Item_Count()
        {
            // create a new place for this test
            var addPlaceDto = new AddPlaceDto() {
                Name = "Item_Count_Test_Place",
                TemplateId = (await _teplateRepo.QueryTemplates(QueryBuilder<TemplateSummaryDto>.NewQuery(t => t.TemplateType, TemplateFor.Place, ComparisonOperator.Equals).Build())).First().Id
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
            Validator.AreNotEqual(item, movedItem);
            Validator.AreEqual(movedItem.PlaceId, moveItemDto.NewPlaceId);
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

            // check response
            ItemDtoValidator.ItemDetailCheck(item);

            var id = item.Id;
            var deleteResult = await _itemRepo.DeleteItem(id);
            // check the result
            Validator.IsTrue(deleteResult);
            // verify by trying to get the item back (should error out if the items down't exist)
            try
            {
                var sameItem = await _itemRepo.GetItemDetail(id);
                Assert.IsTrue(false);    // fail, we could still get the item
            }
            catch (Exception)
            {
                Validator.IsTrue(true); //successfully deleted
            }            
        }

        [TestMethod]
        public async Task Item_Upload()
        {
            var itemToCreate = await CreateRandomAssetAddItemDto();
            var item = await _itemRepo.CreateItem(itemToCreate);
            Validator.IsNotNull(item);

            var id = item.Id;
            await _itemRepo.DeleteItem(id);
            try
            {
                var sameItem = await _itemRepo.GetItemDetail(id);
                Validator.IsTrue(false);
            }
            catch (Exception)
            {
                Validator.IsTrue(true); //successfully deleted
            }

            var itemToUpload = ConvertItemToUploadDto(item);
            itemToUpload.Operation = FileUploadOperation.Update;
            var updatedItem = await _itemRepo.UploadItems(itemToUpload);
            Validator.IsTrue(updatedItem.Count == 0);

            itemToUpload.Operation = FileUploadOperation.Create;
            updatedItem = await _itemRepo.UploadItems(itemToUpload);
            Validator.IsTrue(updatedItem.Count == 1);

            itemToUpload.Operation = FileUploadOperation.Update;
            updatedItem = await _itemRepo.UploadItems(itemToUpload);
            Validator.IsTrue(updatedItem.Count == 1);

            itemToUpload.Operation = FileUploadOperation.CreateIgnoreDuplicates;
            updatedItem = await _itemRepo.UploadItems(itemToUpload);
            Validator.IsTrue(updatedItem.Count == 0);

            itemToUpload.Operation = FileUploadOperation.CreateOrUpdate;
            itemToUpload.Entities.First()["tagnumber"] = "0101010101";
            updatedItem = await _itemRepo.UploadItems(itemToUpload);
            Validator.IsTrue(updatedItem.Count == 1);
            Validator.IsTrue(updatedItem.First().TagNumber == "0101010101");

            await _itemRepo.DeleteItem(id);
            try
            {
                var sameItem = await _itemRepo.GetItemDetail(id);
                Validator.IsTrue(false);
            }
            catch (Exception)
            {
                Validator.IsTrue(true); //successfully deleted
            }
        }

        private FileUploadDto ConvertItemToUploadDto(ItemDetailDto item)
        {
            var uploadDtoEntities = new List<Dictionary<string, string>>();
            var dic = new Dictionary<string, string>
            {
                ["name"] = item.Name,
                ["description"] = item.Description,
                ["person"] = item.CreatedByUserFullName,
                ["place"] = item.PlaceName,
                ["type"] = item.SkuName,
                ["tagnumber"] = item.TagNumber
            };
            foreach (var exProperty in item.ItemExtendedPropertyList)
            {
                dic[exProperty.Name] = exProperty.Value;
            }
            uploadDtoEntities.Add(dic);
            return new FileUploadDto { Entities = uploadDtoEntities, Operation = FileUploadOperation.CreateOrUpdate, UniqueProperty = "name" };
        }

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
            var ran = new Random();
            // choose a place
            var places = await _placeRepo.QueryPlaces();
            var place = places.Items.ElementAt(ran.Next(places.Items.Count() - 1)); // picks a random place for the item
            // choose a person
            var persons = await _personRepo.QueryPersons();
            var person = persons.Items.ElementAt(ran.Next(persons.Items.Count() - 1));
            // choose an asset sku
            var skus = await _skuRepo.QuerySkus(QueryBuilder<SkuSummaryDto>.NewQuery(s => s.CompanyPrefix,"",ComparisonOperator.Equals)
                .And(s => s.ItemReference, "", ComparisonOperator.Equals)
                .Build());
            var sku = skus.Items.ElementAt(ran.Next(skus.Items.Count() - 1));

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
            var ran = new Random();
            PlaceSummaryDto place = null;
            while (place?.Id.Equals(notThisPlaceId) ?? true)
            {
                place = places.Items.ElementAt(ran.Next(places.Items.Count() - 1));
            }
            return place;
        }

        private async Task<UserSummaryDto> GetRandomUser()
        {
            var ran = new Random();
            var users = await _userRepo.QueryUsers();
            var user = users.Items.ElementAt(ran.Next(users.Items.Count() - 1));
            return user;
        }

        #endregion
    }
}
