using System;
using System.Linq;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.SkuGroups;
using Locafi.Client.Model.Query;
using Locafi.Client.Model.Query.PropertyComparison;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Locafi.Client.UnitTests.Tests.Rian
{
    [TestClass]
    public class SkuGroupTests : WebRepoTestsBase
    {
        private const string TestGroupName = "Test Group";

        
        [TestMethod]
        public async Task SkuGroup_CreateDelete()
        {
            var ran = new Random();
            // first we need a name
            var groupNames =
                await
                    SkuGroupRepo.QuerySkuGroupNames(SkuGroupNameQuery.NewQuery(g => g.Name, TestGroupName,
                        ComparisonOperator.Equals));
            Assert.IsTrue(groupNames.Entities.Count <= 1, "There should not be multiple of these"); // there should be at most 1 group name like this
            
            var groupName = groupNames.Entities.FirstOrDefault() ?? await SkuGroupRepo.CreateSkuGroupName(new AddSkuGroupNameDto(TestGroupName)); // create if not exists

            // get a sku to add
            var skus = await SkuRepo.GetAllSkus();
            var sku = skus[ran.Next(skus.Count - 1)];

            // get a place to add the group to
            var places = await PlaceRepo.GetAllPlaces();
            var place = places[ran.Next(places.Count - 1)];

            // now create a new, empty group
            var addGroupDto = new AddSkuGroupDto(groupName.Id);
            addGroupDto.AddSku(sku.Id);
            addGroupDto.AddPlace(place.Id);

            var group = await SkuGroupRepo.CreateSkuGroup(addGroupDto);

            Assert.IsNotNull(group, "Group was null");
            Assert.AreEqual(group.SkuGroupName, groupName.Name, "Name of group equals group name");

            var deleteResult = await SkuGroupRepo.DeleteSkuGroup(group.Id);
            Assert.IsTrue(deleteResult, "deleteResult == true");
        }
    }
}
