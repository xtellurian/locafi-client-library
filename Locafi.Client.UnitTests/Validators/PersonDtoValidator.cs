using Locafi.Client.Model.Dto.Persons;
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
    public static class PersonDtoValidator
    {
        public static void PersonSummaryCheck(PersonSummaryDto dto)
        {
            BaseDtoValidator.DtoBaseCheck(dto);

            try
            {
                BaseDtoValidator.CheckString(dto.GivenName, "PersonSummaryCheck: Name == null/Empty");
                BaseDtoValidator.CheckString(dto.Surname, "PersonSummaryCheck: Surname == null/Empty");
                BaseDtoValidator.CheckString(dto.Email, "PersonSummaryCheck: Email == null/Empty");

                BaseDtoValidator.CheckGuid(dto.TemplateId, "PersonSummaryCheck: TemplateId == null/Empty");
                BaseDtoValidator.CheckString(dto.TemplateName, "PersonSummaryCheck: TemplateName == null/Empty");
            }
            catch (Exception e)
            {
                var msg = "Error Message: " + e.Message + "\r\n\r\nStack Trace: " + e.StackTrace;
                MessageBox.Show(msg, "Validation Error");
                throw e;
            }
        }

        public static void PersonDetailCheck(PersonDetailDto dto, TemplateDetailDto templateDto = null)
        {
            PersonSummaryCheck(dto);

            try
            {
                if (templateDto != null)
                {
                    // check for valid extended properties
                    foreach (var prop in templateDto.TemplateExtendedPropertyList)
                    {
                        var dtoProp = dto.PersonExtendedPropertyList.FirstOrDefault(p => p.ExtendedPropertyId == prop.ExtendedPropertyId);
                        Assert.IsNotNull(dtoProp, "PersonDetailCheck: " + prop.ExtendedPropertyName + " == null");
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
