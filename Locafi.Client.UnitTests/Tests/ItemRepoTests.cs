using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Locafi.Client.Contract.Services;
using Locafi.Client.Data;
using Locafi.Client.Model.Dto.Items;
using Locafi.Client.Model.Dto.Places;
using Locafi.Client.Model.Query;
using Locafi.Client.Model.Query.PropertyComparison;
using Locafi.Client.Model.Query.Simple;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

            var addItemDto = new AddItemDto
            {
                Description = description,
                Name = name,
                PlaceId = place.Id,
                SkuId = sku.Id,
                TagNumber = tagNumber,
                TagType = 0,
                ItemExtendedPropertyList = new List<WriteItemExtendedPropertyDto>(),
                PersonId = person.Id
            };
            return addItemDto;
        }

        [TestMethod]
        public async Task Item_SimpleQueryItems()
        {
            var itemToAdd = await CreateRandomAddItemDto();
            var item = await _itemRepo.CreateItem(itemToAdd);
            Assert.IsNotNull(item, "Failed to create Item");

            var query1 = new SimpleItemQuery(item.Name, SimpleItemQuery.StringProperties.Name);
            var result1 = await _itemRepo.QueryItems(query1);
            Assert.IsNotNull(result1, "query1 returned null on repo query");
            Assert.IsInstanceOfType(result1,typeof(IEnumerable<ItemSummaryDto>));
            Assert.IsTrue(result1.Contains(item));

            var query2 = new SimpleItemQuery(item.Description, SimpleItemQuery.StringProperties.Description);
            var result2 = await _itemRepo.QueryItems(query2);
            Assert.IsNotNull(result2);
            Assert.IsInstanceOfType(result2, typeof(IEnumerable<ItemSummaryDto>));
            Assert.IsTrue(result2.Contains(item));

            var query3 = new SimpleItemQuery(item.SkuId, SimpleItemQuery.IdProperties.SkuId);
            var result3 = await _itemRepo.QueryItems(query3);
            Assert.IsNotNull(result3);
            Assert.IsInstanceOfType(result3, typeof(IEnumerable<ItemSummaryDto>));
            Assert.IsTrue(result3.Contains(item));
        }

        [TestMethod]
        public async Task Item_QueryItems()
        {
            var itemToAdd = await CreateRandomAddItemDto();
            var item = await _itemRepo.CreateItem(itemToAdd);
            Assert.IsNotNull(item);

            var q1 = new ItemQuery();
            q1.CreateQuery(i=> i.CreatedByUserFullName, item.CreatedByUserFullName, ComparisonOperator.Equals);
            var r1 = await _itemRepo.QueryItems(q1);
            Assert.IsNotNull(r1);
            Assert.IsInstanceOfType(r1,typeof(IEnumerable<ItemSummaryDto>));
            Assert.IsTrue(r1.Contains(item));
        }

        [TestMethod]
        public async Task Item_Count()
        {
            var itemToAdd = await CreateRandomAddItemDto();
            var result = await _itemRepo.CreateItem(itemToAdd);
            Assert.IsNotNull(result);
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
            Assert.AreEqual(item,movedItem);
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

        [TestCleanup]
        public async void Cleanup()
        {
            foreach (var id in _toDelete)
            {
                await _itemRepo.DeleteItem(id);
            }
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

        private async Task<UserDto> GetRandomUser()
        {
            var ran = new Random();
            var users = await _userRepo.GetAllUsers();
            var user = users[ran.Next(users.Count - 1)];
            return user;
        }


    }
}
