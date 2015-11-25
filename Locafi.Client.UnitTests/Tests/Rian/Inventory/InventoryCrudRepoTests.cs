using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Exceptions;
using Locafi.Client.Model.Dto.Inventory;
using Locafi.Client.Model.Dto.SkuGroups;
using Locafi.Client.Model.Query;
using Locafi.Client.Model.Query.Builder;
using Locafi.Client.Model.Query.PropertyComparison;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Locafi.Client.UnitTests.Tests.Rian.Inventory
{
    [TestClass]
    public class InventoryCrudRepoTests : WebRepoTestsBase

{



    [TestMethod]
    public async Task InventoryCrud_GetAllInventories()
    {
        var inventories = await InventoryRepo.GetAllInventories();
        Assert.IsNotNull(inventories);
        Assert.IsInstanceOfType(inventories, typeof (IEnumerable<InventorySummaryDto>));
    }

    [TestMethod]
    public async Task InventoryCrud_QueryInventories()
    {
        // Create Inventory for us to Query
        var ran = new Random();
        var name = Guid.NewGuid().ToString();
        var places = await PlaceRepo.GetAllPlaces();
        var place = places[ran.Next(places.Count - 1)];
        var inventory = await InventoryRepo.CreateInventory(place.Id, name);
        Assert.IsNotNull(inventory, "Couldn't create that inventory");

        var query = new InventoryQuery();
        //Query on Name
        query.CreateQuery(q => q.Name, inventory.Name, ComparisonOperator.Equals);
        var result = await InventoryRepo.QueryInventories(query);
        Assert.IsNotNull(result, "Result was null on Name query");
        Assert.IsTrue(result.Contains(inventory));

        query.CreateQuery(q => q.PlaceId, inventory.PlaceId, ComparisonOperator.Equals);
        result = await InventoryRepo.QueryInventories(query);
        Assert.IsNotNull(result, "Result was null on PlaceId query");
        Assert.IsTrue(result.Contains(inventory));
    }

    [TestMethod]
    public async Task InventoryCrud_GetDetail()
    {
        var inventories = await InventoryRepo.GetAllInventories();
        Assert.IsNotNull(inventories, "inventories != null");
        foreach (var inventory in inventories)
        {
            var detail = await InventoryRepo.GetInventory(inventory.Id);
            Assert.IsNotNull(detail, $"{inventory.Id} got null detail");
            Assert.IsInstanceOfType(detail, typeof (InventoryDetailDto));
            Assert.AreEqual(inventory, detail);
        }
    }

        [TestMethod]
        public async Task InventoryCrud_Create()
        {
            var ran = new Random();
            var name = Guid.NewGuid().ToString();
            var places = await PlaceRepo.GetAllPlaces();
            var place = places[ran.Next(places.Count - 1)];
            
            // create a new inventory
            var result = await InventoryRepo.CreateInventory(place.Id, name);
            Assert.IsInstanceOfType(result, typeof (InventoryDetailDto));
            Assert.IsTrue(string.Equals(result.Name, name));
            Assert.AreEqual(place.Id, result.PlaceId);

            // cool that worked, now delete it
            await InventoryRepo.Delete(result.Id);
            

            var groups = await SkuGroupRepo.QuerySkuGroups(UriQuery<SkuGroupSummaryDto>.NoFilter(0, 10));
            var group = groups.Entities.FirstOrDefault();
            Assert.IsNotNull(group);

            result = await InventoryRepo.CreateInventory(place.Id, "", group.Id);
            Assert.AreEqual(group.Id, result.SkuGroupId, "Groups Are Equal");

            // ok, now lets get that again by id
            result = await InventoryRepo.GetInventory(result.Id);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.SkuGroupId, group.Id);

            // cool now delete it
            await InventoryRepo.Delete(result.Id);
        }



    //     [TestMethod]
    public async Task InventoryCrud_Delete()
    {
        var ran = new Random();
        var name = Guid.NewGuid().ToString();
        var places = await PlaceRepo.GetAllPlaces();
        var place = places[ran.Next(places.Count - 1)];
        var inventory = await InventoryRepo.CreateInventory( place.Id, name);

        var inventories = await InventoryRepo.GetAllInventories(); // get all
        Assert.IsTrue(inventories.Contains(inventory), "inventories.Contains(inventory)");
            // assert all contains the one we just made

        await InventoryRepo.Delete(inventory.Id); // delete the one we just made
        inventories = await InventoryRepo.GetAllInventories(); // get all again
        Assert.IsFalse(inventories.Contains(inventory), "inventories.Contains(inventory)");
            // assert the one we made no longer exists
    }

}
}
