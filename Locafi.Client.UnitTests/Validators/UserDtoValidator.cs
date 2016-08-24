using Locafi.Client.Model.Dto;
using Locafi.Client.Model.Dto.Templates;
using Locafi.Client.Model.Dto.Users;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Locafi.Client.UnitTests.Validators
{
    public static class UserDtoValidator
    {
        public static void UserSummaryCheck(UserSummaryDto dto)
        {
            BaseDtoValidator.DtoBaseCheck(dto);

            try
            {
                BaseDtoValidator.CheckString(dto.GivenName, "UserSummaryCheck: Name == null/Empty");
                BaseDtoValidator.CheckString(dto.Surname, "UserSummaryCheck: Surname == null/Empty");
                BaseDtoValidator.CheckString(dto.Email, "UserSummaryCheck: Email == null/Empty");

                BaseDtoValidator.CheckGuid(dto.RoleId, "UserSummaryCheck: RoleId == null/Empty");
                BaseDtoValidator.CheckString(dto.RoleName, "UserSummaryCheck: RoleName == null/Empty");

                BaseDtoValidator.CheckGuid(dto.TemplateId, "UserSummaryCheck: TemplateId == null/Empty");
                BaseDtoValidator.CheckString(dto.TemplateName, "UserSummaryCheck: TemplateName == null/Empty");
            }
            catch (Exception e)
            {
                var msg = "Error Message: " + e.Message + "\r\n\r\nStack Trace: " + e.StackTrace;
                MessageBox.Show(msg, "Validation Error");
                throw e;
            }
        }

        public static void UserDetailCheck(UserDetailDto dto, TemplateDetailDto templateDto = null)
        {
            UserSummaryCheck(dto);

            try
            {
                BaseDtoValidator.CheckGuid(dto.PersonId, "UserDetailCheck: PersonId == null/Empty");

                if (templateDto != null)
                {
                    // check for valid extended properties
                    foreach (var prop in templateDto.TemplateExtendedPropertyList)
                    {
                        var dtoProp = dto.PersonExtendedPropertyList.FirstOrDefault(p => p.ExtendedPropertyId == prop.ExtendedPropertyId);
                        Assert.IsNotNull(dtoProp, "UserDetailCheck: " + prop.ExtendedPropertyName + " == null");
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

        public static void LoggedInUserDetailCheck(UserDetailDto dto, TemplateDetailDto templateDto = null)
        {
            try
            {
                Assert.IsNotNull(dto, "LoggedInUserDetailCheck: dto == null");

                Assert.IsInstanceOfType(dto, typeof(EntityDtoBase), "LoggedInUserDetailCheck: dto not of type(EntityDtoBase)");

                Assert.IsNotNull(dto.Id, "LoggedInUserDetailCheck: Id == null");
                Assert.IsFalse(dto.Id == Guid.Empty, "LoggedInUserDetailCheck: Id == Empty");

                //Assert.IsNotNull(dto.CreatedByUserId, "LoggedInUserDetailCheck: CreatedByUserId == null");
                //Assert.IsFalse(dto.CreatedByUserId == Guid.Empty, "LoggedInUserDetailCheck: CreatedByUserId == Empty");
                //BaseDtoValidator.CheckString(dto.CreatedByUserFullName, "LoggedInUserDetailCheck: CreatedByUserFullName == null/Empty");

                Assert.IsNotNull(dto.DateCreated, "LoggedInUserDetailCheck: DateCreated == null");
                Assert.IsFalse(dto.DateCreated == DateTimeOffset.MinValue, "LoggedInUserDetailCheck: DateCreated == MinValue");

                BaseDtoValidator.CheckString(dto.GivenName, "LoggedInUserDetailCheck: Name == null/Empty");
                BaseDtoValidator.CheckString(dto.Surname, "LoggedInUserDetailCheck: Surname == null/Empty");
                BaseDtoValidator.CheckString(dto.Email, "LoggedInUserDetailCheck: Email == null/Empty");

                BaseDtoValidator.CheckGuid(dto.RoleId, "LoggedInUserDetailCheck: RoleId == null/Empty");
                BaseDtoValidator.CheckString(dto.RoleName, "LoggedInUserDetailCheck: RoleName == null/Empty");

                BaseDtoValidator.CheckGuid(dto.TemplateId, "LoggedInUserDetailCheck: TemplateId == null/Empty");
                BaseDtoValidator.CheckString(dto.TemplateName, "LoggedInUserDetailCheck: TemplateName == null/Empty");

                BaseDtoValidator.CheckGuid(dto.PersonId, "LoggedInUserDetailCheck: PersonId == null/Empty");

                if (templateDto != null)
                {
                    // check for valid extended properties
                    foreach (var prop in templateDto.TemplateExtendedPropertyList)
                    {
                        var dtoProp = dto.PersonExtendedPropertyList.FirstOrDefault(p => p.ExtendedPropertyId == prop.ExtendedPropertyId);
                        Assert.IsNotNull(dtoProp, "LoggedInUserDetailCheck: " + prop.ExtendedPropertyName + " == null");
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
