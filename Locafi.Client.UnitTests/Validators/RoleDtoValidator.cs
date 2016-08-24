using Locafi.Client.Model.Dto.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Locafi.Client.UnitTests.Validators
{
    public static class RoleDtoValidator
    {
        public static void RoleSummaryCheck(RoleSummaryDto dto)
        {
            BaseDtoValidator.DtoBaseCheck(dto);

            Validator.IsInstanceOfType(dto, typeof(RoleSummaryDto));

            try
            {
                BaseDtoValidator.CheckString(dto.Name, "RoleSummaryCheck: Name == null/Empty");
            }
            catch (Exception e)
            {
                var msg = "Error Message: " + e.Message + "\r\n\r\nStack Trace: " + e.StackTrace;
                MessageBox.Show(msg, "Validation Error");
                throw e;
            }
        }

        public static void RoleDetailCheck(RoleDetailDto dto)
        {
            RoleSummaryCheck(dto);

            Validator.IsNotNull(dto.Claims, "RoleDetailCheck: Claims == Null");

            Validator.IsInstanceOfType(dto, typeof(RoleDetailDto));

            try
            {
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
