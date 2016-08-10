using System;
using System.Linq;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.SkuGroups;
using Locafi.Client.Model.Query;
using Locafi.Client.Model.Query.PropertyComparison;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Locafi.Client.Model.Query.Builder;

namespace Locafi.Client.UnitTests.Tests
{
    [TestClass]
    public class SkuGroupTests : WebRepoTestsBase
    {
        private const string TestGroupName = "First Test Group";
        private const string SecondTestGroupName = "Second Test Group";
        
        [TestMethod]
        public async Task SkuGroup_CreateDelete()
        {
            var ran = new Random();
            // first we need 2 group names
            var groupName =
                await
                    SkuGroupRepo.QuerySkuGroupNamesContinuation(SkuGroupNameQuery.NewQuery(g => g.Name, TestGroupName,
                        ComparisonOperator.Equals));
            Assert.IsTrue(groupName.Entities.Count <= 1, "There should not be multiple of these"); // there should be at most 1 group name like this
            
            var groupName1 = groupName.Entities.FirstOrDefault(n=>string.Equals(n.Name, TestGroupName)) ?? await SkuGroupRepo.CreateSkuGroupName(new AddSkuGroupNameDto(TestGroupName)); // create if not exists
            var groupnames = await SkuGroupRepo.QuerySkuGroupNames();
            var groupName2 = groupnames.Items.FirstOrDefault(n=>string.Equals(n.Name, SecondTestGroupName)) ?? await SkuGroupRepo.CreateSkuGroupName(new AddSkuGroupNameDto(SecondTestGroupName)); // create if not exists

            // get a sku to add
            var skus = await SkuRepo.QuerySkus();
            var sku = skus.Items.ElementAt(ran.Next(skus.Items.Count() - 1));

            // get a place to add the group to
            var places = await PlaceRepo.QueryPlaces();
            var place = places.Items.ElementAt(ran.Next(places.Items.Count() - 1));

            // now create a new group with 1 place and 1 sku
            var addGroupDto = new AddSkuGroupDto(groupName1.Id);
            addGroupDto.AddSku(sku.Id);
            addGroupDto.AddPlace(place.Id);

            // check that a skugroup like this doesn't exist already
            var skuGroups = await SkuGroupRepo.GetSkuGroupsForPlace(place.Id);
            var sg = skuGroups.Where(s => s.SkuGroupNameId == addGroupDto.SkuGroupNameId).FirstOrDefault();
            if (sg != null)
            {
                // delete the place from the sku group first
                var sgd = await SkuGroupRepo.GetSkuGroupDetail(sg.Id);
                var sgu = new UpdateSkuGroupDto();
                sgu.SkuGroupId = sgd.Id;
                sgu.SkuGroupNameId = sgd.SkuGroupNameId;
                sgu.PlaceIds = sgd.Places.Select(p => p.Id).ToList();
                sgu.SkuIds = sgd.Skus.Select(s => s.Id).ToList();
                sgu.RemovePlace(place.Id);
                await SkuGroupRepo.UpdateSkuGroup(sgu);
            }

            var group = await SkuGroupRepo.CreateSkuGroup(addGroupDto);
            var id = group.Id;
            Assert.IsNotNull(group, "Group was null");
            Assert.AreEqual(group.SkuGroupName, groupName1.Name, "Name of group equals group name");
            Assert.IsTrue(group.Skus.Contains(sku), "Group contains Sku");
            Assert.IsTrue(group.Places.Contains(place), "Group contains place");

            //get group detail
            var groupDetail = await SkuGroupRepo.GetSkuGroupDetail(id);
            Assert.AreEqual(group,groupDetail, "group == groupDetail by Id");
            Assert.IsTrue(group.Skus.Contains(sku), "GroupDetail contains Sku");
            Assert.IsTrue(group.Places.Contains(place), "GroupDetail contains place");

            // get by place
            var groupsByPlace = await SkuGroupRepo.GetSkuGroupsForPlace(place.Id);
            Assert.IsNotNull(groupsByPlace, "groupsByPlace != null");
            Assert.IsTrue(groupsByPlace.Contains(group), "groupsByPlace.Contains(group)");

            //update group name
            var updateDto = new UpdateSkuGroupDto(groupDetail); // change group name to group name 2
            updateDto.SkuGroupNameId = groupName2.Id;
            groupDetail = await SkuGroupRepo.UpdateSkuGroup(updateDto);
            Assert.AreEqual(groupName2.Name, groupDetail.SkuGroupName, "Updated names are equal");

            // remove place and sku, and then re-add them
//            updateDto = new UpdateSkuGroupDto(id);
            updateDto.RemoveSku(sku.Id);
            updateDto.RemovePlace(place.Id);
            groupDetail = await SkuGroupRepo.UpdateSkuGroup(updateDto);
            Assert.IsFalse(groupDetail.Skus.Contains(sku), "Successfully removed sku");
            Assert.IsFalse(groupDetail.Places.Contains(place), "Succesfully removed place");

            //re-add those places and skus
//            updateDto = new UpdateSkuGroupDto(id);
            updateDto.AddPlace(place.Id);
            updateDto.AddSku(sku.Id);
            groupDetail = await SkuGroupRepo.UpdateSkuGroup(updateDto);
            Assert.IsTrue(groupDetail.Skus.Contains(sku), "Re-added sku");
            Assert.IsTrue(groupDetail.Places.Contains(place), "Re-added place");


            // delete group
            var deleteResult = await SkuGroupRepo.DeleteSkuGroup(group.Id);
            Assert.IsTrue(deleteResult, "deleteResult == true");
        }
    }
}
