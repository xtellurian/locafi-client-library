using Locafi.Client.Model.Dto.Skus;
using Locafi.Client.Model.Dto.Templates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Locafi.Client.UnitTests.Validators
{
    public static class SkuDtoValidator
    {
        public static void SkuSummaryCheck(SkuSummaryDto dto, bool isSgtin = false)
        {
            BaseDtoValidator.DtoBaseCheck(dto);

            try {
                BaseDtoValidator.CheckString(dto.Name, "SkuSummaryCheck: Name == null/Empty");
                BaseDtoValidator.CheckGuid(dto.TemplateId, "SkuSummaryCheck: TemplateId == null/Empty");
                BaseDtoValidator.CheckString(dto.TemplateName, "SkuSummaryCheck: TemplateName == null/Empty");

                if (isSgtin)
                {
                    BaseDtoValidator.CheckString(dto.CompanyPrefix, "SkuSummaryCheck: CompanyPrefix == null/Empty");
                    BaseDtoValidator.CheckString(dto.ItemReference, "SkuSummaryCheck: ItemReference == null/Empty");
                }
            }
            catch (Exception e)
            {
                var msg = "Error Message: " + e.Message + "\r\n\r\nStack Trace: " + e.StackTrace;
                MessageBox.Show(msg, "Validation Error");
                throw e;
            }
        }

        public static void SkuDetailCheck(SkuDetailDto dto, TemplateDetailDto templateDto, bool isSgtin = false)
        {
            SkuSummaryCheck(dto);

            try {
                // check for valid extended properties
                foreach (var prop in templateDto.TemplateExtendedPropertyList)
                {
                    var dtoProp = dto.SkuExtendedPropertyList.FirstOrDefault(p => p.ExtendedPropertyId == prop.ExtendedPropertyId);
                    Assert.IsNotNull(dtoProp, "SkuDetailCheck: " + prop.ExtendedPropertyName + " == null");
                }
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
