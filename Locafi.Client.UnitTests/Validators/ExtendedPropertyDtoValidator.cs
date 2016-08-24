using Locafi.Client.Model.Dto.ExtendedProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Locafi.Client.UnitTests.Validators
{
    public static class ExtendedPropertyDtoValidator
    {
        public static void ExtendedPropertySummaryCheck(ExtendedPropertySummaryDto dto)
        {
            BaseDtoValidator.DtoBaseCheck(dto);

            Validator.IsInstanceOfType(dto, typeof(ExtendedPropertySummaryDto));

            Validator.IsNotNull(dto.TemplateType, "ExtendedPropertySummaryCheck: TemplateType == null");
            Validator.IsNotNull(dto.DataType, "ExtendedPropertySummaryCheck: DataType == null");

            try
            {
                BaseDtoValidator.CheckString(dto.Name, "ExtendedPropertySummaryCheck: Name == null/Empty");
            }
            catch (Exception e)
            {
                var msg = "Error Message: " + e.Message + "\r\n\r\nStack Trace: " + e.StackTrace;
                MessageBox.Show(msg, "Validation Error");
                throw e;
            }
        }

        public static void ExtendedPropertyDetailCheck(ExtendedPropertyDetailDto dto)
        {
            ExtendedPropertySummaryCheck(dto);

            Validator.IsInstanceOfType(dto, typeof(ExtendedPropertyDetailDto));
        }
    }
}
