using Locafi.Client.Model.Dto.CycleCountDtos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Locafi.Client.UnitTests.Validators
{
    public static class CycleCountDtoValidator
    {
        public static void CycleCountSummarycheck(CycleCountSummaryDto dto, bool isComplete = false)
        {
            BaseDtoValidator.DtoBaseCheck(dto);

            try {
                BaseDtoValidator.CheckGuid(dto.PlaceId, "CycleCountSummarycheck: PlaceId == null/Empty");
                BaseDtoValidator.CheckString(dto.PlaceName, "CycleCountSummarycheck: PlaceName == null/Empty");

                Assert.AreEqual(dto.Complete, isComplete, "CycleCountSummarycheck: incorrect Complete state (" + dto.Complete.ToString() + ")");
            }
            catch (Exception e)
            {
                var msg = "Error Message: " + e.Message + "\r\n\r\nStack Trace: " + e.StackTrace;
                MessageBox.Show(msg, "Validation Error");
                throw e;
            }
        }

        public static void CycleCountDetailcheck(CycleCountDetailDto dto, bool isComplete = false)
        {
            CycleCountSummarycheck(dto, isComplete);

            try {
                Assert.IsNotNull(dto.CreatedItems, "CycleCounDetailcheck: CreatedItems == null");
                Assert.IsNotNull(dto.MovedItems, "CycleCounDetailcheck: MovedItems == null");
                Assert.IsNotNull(dto.PresentItems, "CycleCounDetailcheck: PresentItems == null");
                Assert.IsNotNull(dto.SelectedSkus, "CycleCounDetailcheck: SelectedSkus == null");
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
