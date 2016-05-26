using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Locafi.Client.Exceptions;
using Locafi.Client.Model.Dto.SkuGroups;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Locafi.Client.UnitTests.Tests.Rian
{
    [TestClass]
    public class SkuGroupNameTests : WebRepoTestsBase
    {

        [TestMethod]
        public async Task SkuGroupName_CreateUpdateDelete()
        {
            var name = Guid.NewGuid().ToString();
            var createDto = new AddSkuGroupNameDto(name);
            var groupName = await SkuGroupRepo.CreateSkuGroupName(createDto);
            // Assert returned result is correct
            Assert.IsNotNull(groupName, "groupName != null");
            Assert.AreEqual(name,groupName.Name, "Name strings are equal");
            var id = groupName.Id;

            // Get the one we just made by Id
            var groupNameObject = await SkuGroupRepo.GetNameById(groupName.Id);
            Assert.AreEqual(groupName, groupNameObject, "Group Names Objects are Equal");

            //update existing
            var name2 = Guid.NewGuid().ToString();
            var updateDto = new UpdateSkuGroupNameDto(id, name2);
            var groupNameObject2 = await SkuGroupRepo.UpdateSkuGroupName(updateDto);
            Assert.AreEqual(groupNameObject, groupNameObject2);
            Assert.AreEqual(groupNameObject2.Name, name2);

            // get it again by Id
            groupNameObject = await SkuGroupRepo.GetNameById(id);
            Assert.AreEqual(groupNameObject.Name, name2);


            var deleteResult = await SkuGroupRepo.DeleteSkuGroupName(groupName.Id);
            Assert.IsTrue(deleteResult, "deleteResult");

            try
            {
                groupNameObject = await SkuGroupRepo.GetNameById(groupName.Id);
                Assert.IsTrue(false); // should never reach this, since the above should throw an excetion
            }
            catch (SkuGroupRepoException skuGEx)
            {
                Assert.IsTrue(skuGEx.StatusCode == HttpStatusCode.NotFound);
            }

        }




    }
}
