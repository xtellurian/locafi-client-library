using Locafi.Client.Model.Dto.Skus;
using Locafi.Client.Model.Dto.Templates;
using Locafi.Client.Model.Enums;
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

            Validator.IsInstanceOfType(dto, typeof(SkuSummaryDto), "SkuSummaryCheck: dto not of type(SkuSummaryDto)");

            try {
                BaseDtoValidator.CheckString(dto.Name, "SkuSummaryCheck: Name == null/Empty");
                BaseDtoValidator.CheckGuid(dto.TemplateId, "SkuSummaryCheck: TemplateId == null/Empty");
                BaseDtoValidator.CheckString(dto.TemplateName, "SkuSummaryCheck: TemplateName == null/Empty");
                Assert.IsTrue(dto.IsSgtin == isSgtin, "SkuSummaryCheck: IsSgtin != isSgtin");

                if (isSgtin)
                {
                    BaseDtoValidator.CheckString(dto.CompanyPrefix, "SkuSummaryCheck: CompanyPrefix == null/Empty");
                    BaseDtoValidator.CheckString(dto.ItemReference, "SkuSummaryCheck: ItemReference == null/Empty");
                    Assert.IsTrue(dto.ItemReference.Length + dto.CompanyPrefix.Length == 13, "SkuSummaryCheck: CompanyPrefix + ItemReference != 13 characters");
                }
            }
            catch (Exception e)
            {
                var msg = "Error Message: " + e.Message + "\r\n\r\nStack Trace: " + e.StackTrace;
                MessageBox.Show(msg, "Validation Error");
                throw e;
            }
        }

        public static void SkuDetailCheck(SkuDetailDto dto, TemplateDetailDto templateDto = null, bool isSgtin = false)
        {
            SkuSummaryCheck(dto, isSgtin);

            Validator.IsInstanceOfType(dto, typeof(SkuDetailDto), "SkuDetailCheck: dto not of type(SkuDetailDto)");

            try {
                if (templateDto != null)
                {
                    // check for valid extended properties
                    foreach (var prop in templateDto.TemplateExtendedPropertyList)
                    {
                        var dtoProp = dto.SkuExtendedPropertyList.FirstOrDefault(p => p.ExtendedPropertyId == prop.ExtendedPropertyId);
                        Assert.IsNotNull(dtoProp, "SkuDetailCheck: " + prop.ExtendedPropertyName + " == null");
                    }
                }
            }
            catch (Exception e)
            {
                var msg = "Error Message: " + e.Message + "\r\n\r\nStack Trace: " + e.StackTrace;
                MessageBox.Show(msg, "Validation Error");
                throw e;
            }
        }

        public static void SkuStockCountCheck(SkuStockCountDto dto, bool isSgtin = false)
        {
            Validator.IsInstanceOfType(dto, typeof(SkuStockCountDto), "SkuStockCountCheck: dto not of type(SkuStockCountDto)");

            try
            {
                BaseDtoValidator.CheckGuid(dto.SkuId, "SkuStockCountCheck: SkuId == null/Empty");
                BaseDtoValidator.CheckString(dto.SkuName, "SkuStockCountCheck: SkuName == null/Empty");

                BaseDtoValidator.CheckGuid(dto.TemplateId, "SkuStockCountCheck: TemplateId == null/Empty");
                BaseDtoValidator.CheckString(dto.TemplateName, "SkuStockCountCheck: TemplateName == null/Empty");

                Assert.IsTrue(dto.IsSgtin == isSgtin, "SkuStockCountCheck: IsSgtin != isSgtin");
                Assert.IsInstanceOfType(dto.ItemStatus, typeof(ItemStateType), "SkuStockCountCheck: Name == null/Empty");

                BaseDtoValidator.CheckGuid(dto.PlaceId, "SkuStockCountCheck: PlaceId == null/Empty");
                BaseDtoValidator.CheckString(dto.PlaceName, "SkuStockCountCheck: PlaceName == null/Empty");
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
