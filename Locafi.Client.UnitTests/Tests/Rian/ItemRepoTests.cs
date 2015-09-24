﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Model.Dto.Items;
using Locafi.Client.Model.Dto.Places;
using Locafi.Client.Model.Dto.Users;
using Locafi.Client.Model.Query;
using Locafi.Client.Model.Query.PropertyComparison;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Locafi.Client.UnitTests.Tests.Rian
{
    [TestClass]
    public class ItemRepoTests
    {
        private IPlaceRepo _placeRepo;
        private IItemRepo _itemRepo;
        private IPersonRepo _personRepo;
        private ISkuRepo _skuRepo;
        private IUserRepo _userRepo;
        private IList<Guid> _toDelete;

        [TestInitialize]
        public void Setup()
        {
            _placeRepo = WebRepoContainer.PlaceRepo;
            _itemRepo = WebRepoContainer.ItemRepo;
            _personRepo = WebRepoContainer.PersonRepo;
            _skuRepo = WebRepoContainer.SkuRepo;
            _userRepo = WebRepoContainer.UserRepo;
            _toDelete = new List<Guid>();
        }

        [TestMethod]
        public async Task Item_Create()
        {
            var addItemDto = await CreateRandomAddItemDto();

            var result = await _itemRepo.CreateItem(addItemDto);

            Assert.IsNotNull(result);
            _toDelete.Add(result.Id);
            Assert.IsTrue(string.Equals(addItemDto.Name, result.Name));
            Assert.IsTrue(string.Equals(addItemDto.Description, result.Description));
            Assert.AreEqual(addItemDto.PlaceId, result.PlaceId);
            Assert.AreEqual(addItemDto.SkuId, result.SkuId);
            Assert.IsNotNull(result.CreatedByUserId);

            var check = await _itemRepo.GetItemDetail(result.Id);

            Assert.IsNotNull(check);
            Assert.AreEqual(result,check);
        }
        [TestMethod]
        public async Task Item_UseExtendedProperties()
        {
            var addItemDto = await CreateRandomAddItemDto();
            var result = await _itemRepo.CreateItem(addItemDto);
            Assert.IsNotNull(result);
            _toDelete.Add(result.Id);

            var skuDetail = await _skuRepo.GetSkuDetail(result.SkuId);
            foreach(var skuDetailExtendedProperty in skuDetail.SkuExtendedPropertyList)
            {
                var itemExtendedProperty = result.ItemExtendedPropertyList
                    .FirstOrDefault(e => e.SkuExtendedPropertyId == skuDetailExtendedProperty.Id);
                Assert.IsNotNull(itemExtendedProperty);
                Assert.AreEqual(skuDetailExtendedProperty.DefaultValue,itemExtendedProperty.Value);
            }
        }
        [TestMethod]
        public async Task Item_QueryItemsForSpecificItem()
        {
            var itemToAdd = await CreateRandomAddItemDto();
            var item = await _itemRepo.CreateItem(itemToAdd);
            Assert.IsNotNull(item);

            var q1 = new ItemQuery();
            q1.CreateQuery(i => i.CreatedByUserFullName, item.CreatedByUserFullName, ComparisonOperator.Equals);
            var r1 = await _itemRepo.QueryItems(q1);
            Assert.IsNotNull(r1);
            Assert.IsInstanceOfType(r1, typeof(IEnumerable<ItemSummaryDto>));
            Assert.IsTrue(r1.Contains(item));


        }
        [TestMethod]
        public async Task Item_QueryUsingTopTake()
        {
            var query = new ItemQuery();
            query.CreateQuery(i=>i.Name, "", ComparisonOperator.Contains, 10, 0);
            var items = await _itemRepo.QueryItems(query);
            Assert.IsTrue(items.Count == 10);
        }

        [TestMethod]
        public async Task Item_QuerySubstring()
        {
            var itemToAdd = await CreateRandomAddItemDto();
            var item = await _itemRepo.CreateItem(itemToAdd);
            Assert.IsNotNull(item);

            var q2 = new ItemQuery();
            q2.CreateQuery(i => i.Name, item.Name.Remove(item.Name.Length / 2), ComparisonOperator.Contains);
            var r2 = await _itemRepo.QueryItems(q2);
            Assert.IsNotNull(r2);
            Assert.IsTrue(r2.Contains(item));


        }

        [TestMethod]
        public async Task Item_Count()
        {
            var itemToAdd = await CreateRandomAddItemDto();
            var result = await _itemRepo.CreateItem(itemToAdd);
            Assert.IsNotNull(result, "result != null");
            _toDelete.Add(result.Id);
            var count = await _itemRepo.GetItemCount();
            Assert.IsTrue(count > 0);
        }

        

        [TestMethod]
        public async Task Item_MoveItem()
        {
            var itemToAdd = await CreateRandomAddItemDto();
            var item = await _itemRepo.CreateItem(itemToAdd);

            var place = await GetRandomOtherPlace(item.PlaceId);
            var user = await GetRandomUser();

            var moveItemDto = new UpdateItemPlaceDto
            {
                ItemId = item.Id,
                DateMoved = DateTime.UtcNow,
                MovedByUserId = user.Id,
                NewPlaceId = place.Id,
                OldPlaceId = item.PlaceId
            };

            var movedItem = await _itemRepo.UpdateItemPlace(moveItemDto);

            Assert.IsNotNull(movedItem);
            Assert.AreEqual(item, movedItem); // is returning incorrect item
            Assert.AreEqual(movedItem.PlaceId, moveItemDto.NewPlaceId);
        }

        public async Task Item_UpdateTag()
        {
            var itemToAdd = await CreateRandomAddItemDto();
            var item = await _itemRepo.CreateItem(itemToAdd);

            var user = await GetRandomUser();

            var dto = new UpdateItemTagDto
            {
                ChangedByUserId = user.Id,
                DateChanged = DateTime.UtcNow,
                ItemId = item.Id,

            };
        }

        [TestMethod]
        public async Task Item_Delete()
        {
            var itemToCreate = await CreateRandomAddItemDto();
            var item = await _itemRepo.CreateItem(itemToCreate);
            Assert.IsNotNull(item);

            var id = item.Id;
            await _itemRepo.DeleteItem(id);
            var sameItem = await _itemRepo.GetItemDetail(id);
            Assert.IsNull(sameItem);
        }

        #region PrivateMethods
        private async Task<AddItemDto> CreateRandomAddItemDto()
        {
            var ran = new Random();
            var places = await _placeRepo.GetAllPlaces();
            var place = places[ran.Next(places.Count - 1)]; // picks a random place for the item
            var persons = await _personRepo.GetAllPersons();
            var person = persons[ran.Next(persons.Count - 1)];
            var skus = await _skuRepo.GetAllSkus();
            var sku = skus[ran.Next(skus.Count - 1)];

            var name = Guid.NewGuid().ToString();
            var description = Guid.NewGuid().ToString();
            var tagNumber = Guid.NewGuid().ToString();

            var addItemDto = new AddItemDto(sku.Id, place.Id, name, description, tagNumber: tagNumber,
                personId: person.Id);
            return addItemDto;
        }

        

        private async Task<PlaceSummaryDto> GetRandomOtherPlace(Guid notThisPlaceId)
        {
            var places = await _placeRepo.GetAllPlaces();
            var ran = new Random();
            PlaceSummaryDto place = null;
            while (place?.Id.Equals(notThisPlaceId) ?? true)
            {
                place = places[ran.Next(places.Count - 1)];
            }
            return place;
        }

        private async Task<UserSummaryDto> GetRandomUser()
        {
            var ran = new Random();
            var users = await _userRepo.GetAllUsers();
            var user = users[ran.Next(users.Count - 1)];
            return user;
        }

        #endregion
    }
}
