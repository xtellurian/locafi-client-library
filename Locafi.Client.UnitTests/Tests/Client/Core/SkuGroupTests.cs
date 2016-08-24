using System;
using System.Linq;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.SkuGroups;
using Locafi.Client.Model.Query;
using Locafi.Client.Model.Query.PropertyComparison;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Locafi.Client.Model.Query.Builder;
using Locafi.Client.Contract.Repo;
using System.Collections.Generic;
using Locafi.Client.Exceptions;
using System.Net;
using Locafi.Client.UnitTests.Validators;
using Locafi.Client.UnitTests.EntityGenerators;
using Locafi.Client.Model;
using Locafi.Client.UnitTests.Extensions;

namespace Locafi.Client.UnitTests.Tests
{
    [TestClass]
    public class SkuGroupTests
    {
        private const string TestGroupName = "First Test Group";
        private const string SecondTestGroupName = "Second Test Group";

        private IPlaceRepo _placeRepo;
        private ISkuRepo _skuRepo;
        private ISkuGroupRepo _skuGroupRepo;

        private List<Guid> _placesToDelete;
        private List<Guid> _skusToDelete;
        private List<Guid> _skuGroupNamesToDelete;
        private List<Guid> _skuGroupsToDelete;

        [TestInitialize]
        public void Initialise()
        {
            _placeRepo = WebRepoContainer.PlaceRepo;
            _skuRepo = WebRepoContainer.SkuRepo;
            _skuGroupRepo = WebRepoContainer.SkuGroupRepo;

            _placesToDelete = new List<Guid>();
            _skusToDelete = new List<Guid>();
            _skuGroupNamesToDelete = new List<Guid>();
            _skuGroupsToDelete = new List<Guid>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            // delete all skuGroups that were created
            foreach (var Id in _skuGroupsToDelete)
            {
                try
                {
                    _skuGroupRepo.DeleteSkuGroup(Id).Wait();
                }
                catch { }
            }

            // delete all places that were created
            foreach (var placeId in _placesToDelete)
            {
                _placeRepo.Delete(placeId).Wait();
            }

            // delete all skuGroupNames that were created
            foreach (var Id in _skuGroupNamesToDelete)
            {
                try
                {
                    _skuGroupRepo.DeleteSkuGroupName(Id).Wait();
                }
                catch { }
            }

            // delete all skus that were created
            foreach (var Id in _skusToDelete)
            {
                _skuRepo.DeleteSku(Id).Wait();
            }
        }

        [TestMethod]
        public async Task SkuGroupName_Create()
        {
            // create name
            var name = Guid.NewGuid().ToString();
            var createDto = new AddSkuGroupNameDto(name);
            var groupName = await _skuGroupRepo.CreateSkuGroupName(createDto);
            _skuGroupNamesToDelete.AddUnique(groupName.Id);

            // check response
            SkuGroupDtoValidator.SkuGroupNameDetailCheck(groupName);
            Validator.AreEqual(name, groupName.Name, "Name strings are not equal");
        }

        [TestMethod]
        public async Task SkuGroupName_Get()
        {
            // create name
            var name = Guid.NewGuid().ToString();
            var createDto = new AddSkuGroupNameDto(name);
            var groupName = await _skuGroupRepo.CreateSkuGroupName(createDto);
            _skuGroupNamesToDelete.AddUnique(groupName.Id);

            // check response
            SkuGroupDtoValidator.SkuGroupNameDetailCheck(groupName);
            Validator.AreEqual(name, groupName.Name, "Name strings are not equal");

            // Get the one we just made by Id
            var id = groupName.Id;
            var groupNameObject = await _skuGroupRepo.GetNameById(groupName.Id);

            // check response
            SkuGroupDtoValidator.SkuGroupNameDetailCheck(groupNameObject);
            Validator.AreEqual(groupName.Name, groupNameObject.Name, "Group Names Objects are Equal");
        }

        [TestMethod]
        public async Task SkuGroupName_Update()
        {
            // create name
            var name = Guid.NewGuid().ToString();
            var createDto = new AddSkuGroupNameDto(name);
            var groupName = await _skuGroupRepo.CreateSkuGroupName(createDto);
            _skuGroupNamesToDelete.AddUnique(groupName.Id);

            // check response
            SkuGroupDtoValidator.SkuGroupNameDetailCheck(groupName);
            Validator.AreEqual(name, groupName.Name, "Name strings are not equal");

            //update existing
            var name2 = Guid.NewGuid().ToString();
            var updateDto = new UpdateSkuGroupNameDto(groupName.Id, name2);
            var groupNameObject2 = await _skuGroupRepo.UpdateSkuGroupName(updateDto);

            // check response
            SkuGroupDtoValidator.SkuGroupNameDetailCheck(groupNameObject2);
            Validator.AreNotEqual(groupName.Name, groupNameObject2.Name);
            Validator.AreEqual(groupNameObject2.Name, name2);
        }

        [TestMethod]
        public async Task SkuGroupName_Delete()
        {
            // create name
            var name = Guid.NewGuid().ToString();
            var createDto = new AddSkuGroupNameDto(name);
            var groupName = await _skuGroupRepo.CreateSkuGroupName(createDto);
            _skuGroupNamesToDelete.AddUnique(groupName.Id);

            // check response
            SkuGroupDtoValidator.SkuGroupNameDetailCheck(groupName);
            Validator.AreEqual(name, groupName.Name, "Name strings are not equal");

            // delete the group name
            var deleteResult = await _skuGroupRepo.DeleteSkuGroupName(groupName.Id);

            // check result
            Validator.IsTrue(deleteResult, "Reason not deleted");
            _skuGroupNamesToDelete.Remove(groupName.Id);

            // verify with query
            var query = QueryBuilder<SkuGroupNameDetailDto>.NewQuery(n => n.Id, groupName.Id, ComparisonOperator.Equals).Build();
            var nameQuery = await _skuGroupRepo.QuerySkuGroupNames(query);
            Validator.IsFalse(nameQuery.Contains(groupName));

            // verify with get
            try
            {
                var nameDetail = await _skuGroupRepo.GetNameById(groupName.Id);

                Validator.IsTrue(false, "Deleted entity returned");
            }catch(Exception e)
            {
                // this is expected
            }
        }

        [TestMethod]
        public async Task SkuGroup_Create()
        {
            // create name
            var name = Guid.NewGuid().ToString();
            var createDto = new AddSkuGroupNameDto(name);
            var groupName = await _skuGroupRepo.CreateSkuGroupName(createDto);
            _skuGroupNamesToDelete.AddUnique(groupName.Id);

            // check response
            SkuGroupDtoValidator.SkuGroupNameDetailCheck(groupName);
            Validator.AreEqual(name, groupName.Name, "Name strings are not equal");

            // create a sku
            var addDto = await SkuGenerator.GenerateSgtinSkuDto();
            var sku1 = await _skuRepo.CreateSku(addDto);
            _skusToDelete.AddUnique(sku1.Id);   // store to deletae later

            // create place
            var addPlace = await PlaceGenerator.GenerateRandomAddPlaceDto();
            var place1 = await _placeRepo.CreatePlace(addPlace);
            _placesToDelete.AddUnique(place1.Id);

            // now create a new group with 1 place and 1 sku
            var addGroupDto = new AddSkuGroupDto(groupName.Id);
            addGroupDto.AddSku(sku1.Id);
            addGroupDto.AddPlace(place1.Id);
            var group = await _skuGroupRepo.CreateSkuGroup(addGroupDto);
            _skuGroupsToDelete.AddUnique(group.Id);

            // check the response
            SkuGroupDtoValidator.SkuGroupDetailCheck(group);
            Validator.AreEqual(group.SkuGroupName, groupName.Name, "Name of group equals group name");
            Validator.IsTrue(group.Skus.Contains(sku1), "Group contains Sku");
            Validator.IsTrue(group.Places.Contains(place1), "Group contains place");
        }

        [TestMethod]
        public async Task SkuGroup_Get()
        {
            // create name
            var name = Guid.NewGuid().ToString();
            var createDto = new AddSkuGroupNameDto(name);
            var groupName = await _skuGroupRepo.CreateSkuGroupName(createDto);
            _skuGroupNamesToDelete.AddUnique(groupName.Id);

            // check response
            SkuGroupDtoValidator.SkuGroupNameDetailCheck(groupName);
            Validator.AreEqual(name, groupName.Name, "Name strings are not equal");

            // create a sku
            var addDto = await SkuGenerator.GenerateSgtinSkuDto();
            var sku1 = await _skuRepo.CreateSku(addDto);
            _skusToDelete.AddUnique(sku1.Id);   // store to deletae later

            // create place
            var addPlace = await PlaceGenerator.GenerateRandomAddPlaceDto();
            var place1 = await _placeRepo.CreatePlace(addPlace);
            _placesToDelete.AddUnique(place1.Id);

            // now create a new group with 1 place and 1 sku
            var addGroupDto = new AddSkuGroupDto(groupName.Id);
            addGroupDto.AddSku(sku1.Id);
            addGroupDto.AddPlace(place1.Id);
            var group = await _skuGroupRepo.CreateSkuGroup(addGroupDto);
            _skuGroupsToDelete.AddUnique(group.Id);

            // check the response
            SkuGroupDtoValidator.SkuGroupDetailCheck(group);
            Validator.AreEqual(group.SkuGroupName, groupName.Name, "Name of group equals group name");
            Validator.IsTrue(group.Skus.Contains(sku1), "Group contains Sku");
            Validator.IsTrue(group.Places.Contains(place1), "Group contains place");

            // now try and get the group again
            var groupGet = await _skuGroupRepo.GetSkuGroupDetail(group.Id);

            // validate the response
            SkuGroupDtoValidator.SkuGroupDetailCheck(groupGet);
            Validator.AreEqual(group.SkuGroupName, groupGet.SkuGroupName, "Name of group equals group name");
            Validator.IsTrue(groupGet.Skus.Contains(sku1), "Group contains Sku");
            Validator.IsTrue(groupGet.Places.Contains(place1), "Group contains place");
        }

        [TestMethod]
        public async Task SkuGroup_GetAll()
        {
            // create name
            var name = Guid.NewGuid().ToString();
            var createDto = new AddSkuGroupNameDto(name);
            var groupName = await _skuGroupRepo.CreateSkuGroupName(createDto);
            _skuGroupNamesToDelete.AddUnique(groupName.Id);

            // check response
            SkuGroupDtoValidator.SkuGroupNameDetailCheck(groupName);
            Validator.AreEqual(name, groupName.Name, "Name strings are not equal");

            // create a sku
            var addDto = await SkuGenerator.GenerateSgtinSkuDto();
            var sku1 = await _skuRepo.CreateSku(addDto);
            _skusToDelete.AddUnique(sku1.Id);   // store to deletae later

            // create place
            var addPlace = await PlaceGenerator.GenerateRandomAddPlaceDto();
            var place1 = await _placeRepo.CreatePlace(addPlace);
            _placesToDelete.AddUnique(place1.Id);

            // now create a new group with 1 place and 1 sku
            var addGroupDto = new AddSkuGroupDto(groupName.Id);
            addGroupDto.AddSku(sku1.Id);
            addGroupDto.AddPlace(place1.Id);
            var group = await _skuGroupRepo.CreateSkuGroup(addGroupDto);
            _skuGroupsToDelete.AddUnique(group.Id);

            // check the response
            SkuGroupDtoValidator.SkuGroupDetailCheck(group);
            Validator.AreEqual(group.SkuGroupName, groupName.Name, "Name of group equals group name");
            Validator.IsTrue(group.Skus.Contains(sku1), "Group contains Sku");
            Validator.IsTrue(group.Places.Contains(place1), "Group contains place");

            // now try and get the group again
            var skuGroups = await _skuGroupRepo.QuerySkuGroups();

            // validate the response
            Validator.IsNotNull(skuGroups);
            Validator.IsInstanceOfType(skuGroups, typeof(PageResult<SkuGroupSummaryDto>));
            Validator.IsTrue(skuGroups.Contains(group), "Query contains SkuGroup");
        }

        [TestMethod]
        public async Task SkuGroup_GetForPlace()
        {
            // create name
            var name = Guid.NewGuid().ToString();
            var createDto = new AddSkuGroupNameDto(name);
            var groupName = await _skuGroupRepo.CreateSkuGroupName(createDto);
            _skuGroupNamesToDelete.AddUnique(groupName.Id);

            // check response
            SkuGroupDtoValidator.SkuGroupNameDetailCheck(groupName);
            Validator.AreEqual(name, groupName.Name, "Name strings are not equal");

            // create a sku
            var addDto = await SkuGenerator.GenerateSgtinSkuDto();
            var sku1 = await _skuRepo.CreateSku(addDto);
            _skusToDelete.AddUnique(sku1.Id);   // store to deletae later

            // create place
            var addPlace = await PlaceGenerator.GenerateRandomAddPlaceDto();
            var place1 = await _placeRepo.CreatePlace(addPlace);
            _placesToDelete.AddUnique(place1.Id);

            // now create a new group with 1 place and 1 sku
            var addGroupDto = new AddSkuGroupDto(groupName.Id);
            addGroupDto.AddSku(sku1.Id);
            addGroupDto.AddPlace(place1.Id);
            var group = await _skuGroupRepo.CreateSkuGroup(addGroupDto);
            _skuGroupsToDelete.AddUnique(group.Id);

            // check the response
            SkuGroupDtoValidator.SkuGroupDetailCheck(group);
            Validator.AreEqual(group.SkuGroupName, groupName.Name, "Name of group equals group name");
            Validator.IsTrue(group.Skus.Contains(sku1), "Group contains Sku");
            Validator.IsTrue(group.Places.Contains(place1), "Group contains place");

            // now try and get groups for this place
            var skuGroups = await _skuGroupRepo.GetSkuGroupsForPlace(place1.Id);

            // validate the response
            Validator.IsNotNull(skuGroups);
            Validator.IsInstanceOfType(skuGroups, typeof(IList<SkuGroupSummaryDto>));
            Validator.IsTrue(skuGroups.Count == 1);
            Validator.IsTrue(skuGroups.Contains(group), "Query contains SkuGroup");

            // now create another group for this same place
            // create name
            var name2 = Guid.NewGuid().ToString();
            var createDto2 = new AddSkuGroupNameDto(name2);
            var groupName2 = await _skuGroupRepo.CreateSkuGroupName(createDto2);
            _skuGroupNamesToDelete.AddUnique(groupName2.Id);

            // check response
            SkuGroupDtoValidator.SkuGroupNameDetailCheck(groupName2);
            Validator.AreEqual(name2, groupName2.Name, "Name strings are not equal");

            // create a sku
            var addDto2 = await SkuGenerator.GenerateSgtinSkuDto();
            var sku2 = await _skuRepo.CreateSku(addDto2);
            _skusToDelete.AddUnique(sku2.Id);   // store to deletae later

            // now create a new group with 1 place and 1 sku
            var addGroupDto2 = new AddSkuGroupDto(groupName2.Id);
            addGroupDto2.AddSku(sku2.Id);
            addGroupDto2.AddPlace(place1.Id);
            var group2 = await _skuGroupRepo.CreateSkuGroup(addGroupDto2);
            _skuGroupsToDelete.AddUnique(group2.Id);

            // now try and get groups for this place
            skuGroups = await _skuGroupRepo.GetSkuGroupsForPlace(place1.Id);

            // validate the response
            Validator.IsNotNull(skuGroups);
            Validator.IsInstanceOfType(skuGroups, typeof(IList<SkuGroupSummaryDto>));
            Validator.IsTrue(skuGroups.Count == 2);
            Validator.IsTrue(skuGroups.Contains(group), "Query contains SkuGroup");
            Validator.IsTrue(skuGroups.Contains(group2), "Query contains SkuGroup");
        }

        [TestMethod]
        public async Task SkuGroup_Update()
        {
            // create name
            var name = Guid.NewGuid().ToString();
            var createDto = new AddSkuGroupNameDto(name);
            var groupName = await _skuGroupRepo.CreateSkuGroupName(createDto);
            _skuGroupNamesToDelete.AddUnique(groupName.Id);

            // check response
            SkuGroupDtoValidator.SkuGroupNameDetailCheck(groupName);
            Validator.AreEqual(name, groupName.Name, "Name strings are not equal");

            // create a sku
            var addDto = await SkuGenerator.GenerateSgtinSkuDto();
            var sku1 = await _skuRepo.CreateSku(addDto);
            _skusToDelete.AddUnique(sku1.Id);   // store to deletae later

            // create place
            var addPlace = await PlaceGenerator.GenerateRandomAddPlaceDto();
            var place1 = await _placeRepo.CreatePlace(addPlace);
            _placesToDelete.AddUnique(place1.Id);

            // now create a new group with 1 place and 1 sku
            var addGroupDto = new AddSkuGroupDto(groupName.Id);
            addGroupDto.AddSku(sku1.Id);
            addGroupDto.AddPlace(place1.Id);
            var group = await _skuGroupRepo.CreateSkuGroup(addGroupDto);
            _skuGroupsToDelete.AddUnique(group.Id);

            // check the response
            SkuGroupDtoValidator.SkuGroupDetailCheck(group);
            Validator.AreEqual(group.SkuGroupName, groupName.Name, "Name of group equals group name");
            Validator.IsTrue(group.Skus.Count == 1);
            Validator.IsTrue(group.Places.Count == 1);
            Validator.IsTrue(group.Skus.Contains(sku1), "Group contains Sku");
            Validator.IsTrue(group.Places.Contains(place1), "Group contains place");

            // now try and get the group again
            var groupGet = await _skuGroupRepo.GetSkuGroupDetail(group.Id);

            // validate the response
            SkuGroupDtoValidator.SkuGroupDetailCheck(groupGet);
            Validator.AreEqual(group.SkuGroupName, groupGet.SkuGroupName, "Name of group equals group name");
            Validator.IsTrue(group.Skus.Count == 1);
            Validator.IsTrue(group.Places.Count == 1);
            Validator.IsTrue(groupGet.Skus.Contains(sku1), "Group contains Sku");
            Validator.IsTrue(groupGet.Places.Contains(place1), "Group contains place");

            // now update the group
            // create a sku
            var addDto2 = await SkuGenerator.GenerateSgtinSkuDto();
            var sku2 = await _skuRepo.CreateSku(addDto2);
            _skusToDelete.AddUnique(sku2.Id);   // store to deletae later

            // create place
            var addPlace2 = await PlaceGenerator.GenerateRandomAddPlaceDto();
            var place2 = await _placeRepo.CreatePlace(addPlace2);
            _placesToDelete.AddUnique(place2.Id);

            var updateDto = new UpdateSkuGroupDto(group);
            updateDto.AddPlace(place2.Id);
            updateDto.AddSku(sku2.Id);

            var groupUpdate = await _skuGroupRepo.UpdateSkuGroup(updateDto);

            // validate the response
            SkuGroupDtoValidator.SkuGroupDetailCheck(groupUpdate);
            Validator.AreEqual(group.SkuGroupName, groupUpdate.SkuGroupName, "Name of group equals group name");
            Validator.IsTrue(groupUpdate.Skus.Count == 2);
            Validator.IsTrue(groupUpdate.Places.Count == 2);
            Validator.IsTrue(groupUpdate.Skus.Contains(sku1), "Group contains Sku");
            Validator.IsTrue(groupUpdate.Skus.Contains(sku2), "Group contains Sku");
            Validator.IsTrue(groupUpdate.Places.Contains(place1), "Group contains place");
            Validator.IsTrue(groupUpdate.Places.Contains(place2), "Group contains place");

            // update again to remove first place and sku and change name
            // create name
            var name2 = Guid.NewGuid().ToString();
            var createDto2 = new AddSkuGroupNameDto(name2);
            var groupName2 = await _skuGroupRepo.CreateSkuGroupName(createDto2);
            _skuGroupNamesToDelete.AddUnique(groupName2.Id);

            updateDto.RemovePlace(place1.Id);
            updateDto.RemoveSku(sku1.Id);
            updateDto.SkuGroupNameId = groupName2.Id;

            var groupUpdate2 = await _skuGroupRepo.UpdateSkuGroup(updateDto);

            // validate the response
            SkuGroupDtoValidator.SkuGroupDetailCheck(groupUpdate2);
            Validator.AreEqual(groupName2.Name, groupUpdate2.SkuGroupName, "Name of group equals group name");
            Validator.IsTrue(groupUpdate2.Skus.Count == 1);
            Validator.IsTrue(groupUpdate2.Places.Count == 1);
            Validator.IsTrue(groupUpdate2.Skus.Contains(sku2), "Group contains Sku");
            Validator.IsTrue(groupUpdate2.Places.Contains(place2), "Group contains place");
        }

        [TestMethod]
        public async Task SkuGroup_Delete()
        {
            // create name
            var name = Guid.NewGuid().ToString();
            var createDto = new AddSkuGroupNameDto(name);
            var groupName = await _skuGroupRepo.CreateSkuGroupName(createDto);
            _skuGroupNamesToDelete.AddUnique(groupName.Id);

            // check response
            SkuGroupDtoValidator.SkuGroupNameDetailCheck(groupName);
            Validator.AreEqual(name, groupName.Name, "Name strings are not equal");

            // create a sku
            var addDto = await SkuGenerator.GenerateSgtinSkuDto();
            var sku1 = await _skuRepo.CreateSku(addDto);
            _skusToDelete.AddUnique(sku1.Id);   // store to deletae later

            // create place
            var addPlace = await PlaceGenerator.GenerateRandomAddPlaceDto();
            var place1 = await _placeRepo.CreatePlace(addPlace);
            _placesToDelete.AddUnique(place1.Id);

            // now create a new group with 1 place and 1 sku
            var addGroupDto = new AddSkuGroupDto(groupName.Id);
            addGroupDto.AddSku(sku1.Id);
            addGroupDto.AddPlace(place1.Id);
            var group = await _skuGroupRepo.CreateSkuGroup(addGroupDto);
            _skuGroupsToDelete.AddUnique(group.Id);

            // check the response
            SkuGroupDtoValidator.SkuGroupDetailCheck(group);
            Validator.AreEqual(group.SkuGroupName, groupName.Name, "Name of group equals group name");
            Validator.IsTrue(group.Skus.Contains(sku1), "Group contains Sku");
            Validator.IsTrue(group.Places.Contains(place1), "Group contains place");

            // delete the sku group
            var deleteResult = await _skuGroupRepo.DeleteSkuGroup(group.Id);

            // check the response
            Validator.IsTrue(deleteResult);
            // remove from delete list
            _skuGroupsToDelete.Remove(group.Id);

            // verify
            var query = QueryBuilder<SkuGroupSummaryDto>.NewQuery(s => s.Id, group.Id, ComparisonOperator.Equals).Build();
            var queryResult = await _skuGroupRepo.QuerySkuGroups(query);
            Validator.IsNotNull(queryResult);
            Validator.IsTrue(!queryResult.Contains(group));
            Validator.IsTrue(queryResult.Items.Count() == 0);

            // verify with get
            try
            {
                var nameDetail = await _skuGroupRepo.GetSkuGroupDetail(group.Id);

                Validator.IsTrue(false, "Deleted entity returned");
            }
            catch (SkuGroupRepoException e)
            {
                // this is expected                
            }
        }
    }
}
