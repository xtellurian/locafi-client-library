using Locafi.Client.Model.Dto.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Locafi.Client.UnitTests.Validators
{
    public static class TemplateDtoValidator
    {
        public static void TemplateSummaryCheck(TemplateSummaryDto dto)
        {
            BaseDtoValidator.DtoBaseCheck(dto);

            Validator.IsInstanceOfType(dto, typeof(TemplateSummaryDto));

            Validator.IsNotNull(dto.TemplateType, "TemplateSummaryCheck: TemplateType == null");

            try
            {
                BaseDtoValidator.CheckString(dto.Name, "TemplateSummaryCheck: Name == null/Empty");
            }
            catch (Exception e)
            {
                var msg = "Error Message: " + e.Message + "\r\n\r\nStack Trace: " + e.StackTrace;
                MessageBox.Show(msg, "Validation Error");
                throw e;
            }
        }

        public static void TemplateDetailCheck(TemplateDetailDto dto)
        {
            TemplateSummaryCheck(dto);

            Validator.IsInstanceOfType(dto, typeof(TemplateDetailDto));

            Validator.IsNotNull(dto.TemplateExtendedPropertyList, "TemplateDetailCheck: TemplateExtendedPropertyList == null");
        }
    }
}
