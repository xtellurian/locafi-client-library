using Locafi.Client.Model.Dto.SkuGroups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Locafi.Client.UnitTests.Validators
{
    public static class SkuGroupDtoValidator
    {
        public static void SkuGroupNameDetailCheck(SkuGroupNameDetailDto dto)
        {
            try
            {
                BaseDtoValidator.CheckGuid(dto.Id, "SkuGroupNameDetailCheck: Id == null/Empty");

                BaseDtoValidator.CheckString(dto.Name, "SkuGroupNameDetailCheck: Name == null/Empty");
            }
            catch (Exception e)
            {
                var msg = "Error Message: " + e.Message + "\r\n\r\nStack Trace: " + e.StackTrace;
                MessageBox.Show(msg, "Validation Error");
                throw e;
            }
        }

        public static void SkuGroupSummaryCheck(SkuGroupSummaryDto dto)
        {
            BaseDtoValidator.DtoBaseCheck(dto);

            try
            {
                BaseDtoValidator.CheckGuid(dto.SkuGroupNameId, "SkuGroupSummaryCheck: SkuGroupNameId == null/Empty");
                BaseDtoValidator.CheckString(dto.SkuGroupName, "SkuGroupSummaryCheck: SkuGroupName == null/Empty");
            }
            catch (Exception e)
            {
                var msg = "Error Message: " + e.Message + "\r\n\r\nStack Trace: " + e.StackTrace;
                MessageBox.Show(msg, "Validation Error");
                throw e;
            }
        }

        public static void SkuGroupDetailCheck(SkuGroupDetailDto dto)
        {
            SkuGroupSummaryCheck(dto);

            try
            {
                Validator.IsNotNull(dto.Skus, "SkuGroupDetailCheck: SkuGroupNameId == null/Empty");
                Validator.IsNotNull(dto.Places, "SkuGroupDetailCheck: SkuGroupName == null/Empty");
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
