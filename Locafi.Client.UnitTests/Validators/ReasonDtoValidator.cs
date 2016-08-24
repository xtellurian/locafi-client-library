using Locafi.Client.Model.Dto.Reasons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Locafi.Client.UnitTests.Validators
{
    public static class ReasonDtoValidator
    {
        public static void ReasonSummaryCheck(ReasonSummaryDto dto)
        {
            BaseDtoValidator.DtoBaseCheck(dto);

            try
            {
                BaseDtoValidator.CheckString(dto.ReasonNumber, "ReasonSummaryCheck: ReasonNumber == null/Empty");

                BaseDtoValidator.CheckString(dto.Description, "ReasonSummaryCheck: Description == null/Empty");
            }
            catch (Exception e)
            {
                var msg = "Error Message: " + e.Message + "\r\n\r\nStack Trace: " + e.StackTrace;
                MessageBox.Show(msg, "Validation Error");
                throw e;
            }
        }

        public static void ReasonDetailCheck(ReasonDetailDto dto)
        {
            ReasonSummaryCheck(dto);
        }
    }
}
