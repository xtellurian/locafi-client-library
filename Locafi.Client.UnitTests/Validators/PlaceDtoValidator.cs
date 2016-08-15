using Locafi.Client.Model.Dto.Places;
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
    public static class PlaceDtoValidator
    {
        public static void PlaceSummaryCheck(PlaceSummaryDto dto)
        {
            BaseDtoValidator.DtoBaseCheck(dto);

            try
            {
                BaseDtoValidator.CheckString(dto.Name, "PlaceSummaryCheck: Name == null/Empty");

                BaseDtoValidator.CheckGuid(dto.TemplateId, "PlaceSummaryCheck: TemplateId == null/Empty");
                BaseDtoValidator.CheckString(dto.TemplateName, "PlaceSummaryCheck: TemplateName == null/Empty");
            }
            catch (Exception e)
            {
                var msg = "Error Message: " + e.Message + "\r\n\r\nStack Trace: " + e.StackTrace;
                MessageBox.Show(msg, "Validation Error");
                throw e;
            }
        }

        public static void PlaceDetailCheck(PlaceDetailDto dto, TemplateDetailDto templateDto = null)
        {
            PlaceSummaryCheck(dto);

            try
            {
                if (templateDto != null)
                {
                    // check for valid extended properties
                    foreach (var prop in templateDto.TemplateExtendedPropertyList)
                    {
                        var dtoProp = dto.PlaceExtendedPropertyList.FirstOrDefault(p => p.ExtendedPropertyId == prop.ExtendedPropertyId);
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
    }
}
