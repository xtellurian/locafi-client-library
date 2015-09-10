using System;
using Locafi.Client.Model.Dto.Inventory;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Locafi.Client.UnitTests.Tests.Rian
{
    [TestClass]
    public class ConversionTests
    {
        [TestMethod]
        public void Convert_Inventory()
        {
            var detail = new InventoryDetailDto {Id = Guid.NewGuid()};
            var summary = new InventorySummaryDto(detail);
            Assert.AreEqual(detail,summary);
        }
    }
}
