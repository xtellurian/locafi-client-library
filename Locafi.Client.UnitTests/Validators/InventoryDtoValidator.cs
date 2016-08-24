using Locafi.Client.Model.Dto.CycleCountDtos;
using Locafi.Client.Model.Dto.Inventory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Locafi.Client.UnitTests.Validators
{
    public static class InventoryDtoValidator
    {
        public static void InventorySummarycheck(InventorySummaryDto dto, bool isComplete = false)
        {
            BaseDtoValidator.DtoBaseCheck(dto);

            try {
                BaseDtoValidator.CheckGuid(dto.PlaceId, "InventorySummarycheck: PlaceId == null/Empty");
                BaseDtoValidator.CheckString(dto.PlaceName, "InventorySummarycheck: PlaceName == null/Empty");

                Assert.AreEqual(dto.Complete, isComplete, "InventorySummarycheck: incorrect Complete state (" + dto.Complete.ToString() + ")");
            }
            catch (Exception e)
            {
                var msg = "Error Message: " + e.Message + "\r\n\r\nStack Trace: " + e.StackTrace;
                MessageBox.Show(msg, "Validation Error");
                throw e;
            }
        }

        public static void InventoryDetailcheck(InventoryDetailDto dto, bool isComplete = false)
        {
            InventorySummarycheck(dto, isComplete);

            try {
                Assert.IsNotNull(dto.FoundItemsExpected, "InventoryDetailcheck: FoundItemsExpected == null");
                Assert.IsNotNull(dto.FoundItemsUnexpected, "InventoryDetailcheck: FoundItemsUnexpected == null");
                Assert.IsNotNull(dto.MissingItems, "InventoryDetailcheck: MissingItems == null");
                Assert.IsNotNull(dto.SelectedSkus, "InventoryDetailcheck: SelectedSkus == null");
            }
            catch (Exception e)
            {
                var msg = "Error Message: " + e.Message + "\r\n\r\nStack Trace: " + e.StackTrace;
                MessageBox.Show(msg, "Validation Error");
                throw e;
            }
        }
    }
}
