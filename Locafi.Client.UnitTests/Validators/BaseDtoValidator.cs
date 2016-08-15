using Locafi.Client.Model.Dto;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Locafi.Client.UnitTests.Validators
{
    public static class BaseDtoValidator
    {
        public static void CheckGuid(Guid guid, string msg = "")
        {
            Assert.IsNotNull(guid);
            Assert.IsFalse(guid == Guid.Empty, msg);
        }

        public static void CheckString(string str, string msg = "")
        {
            Assert.IsFalse(string.IsNullOrEmpty(str), msg);
        }

        public static void DtoBaseCheck(EntityDtoBase dtoBase, bool checkModifiedBy = false)
        {
            try {
                Assert.IsNotNull(dtoBase, "DtoBaseCheck: dto == null");

                Assert.IsNotNull(dtoBase.Id, "DtoBaseCheck: Id == null");
                Assert.IsFalse(dtoBase.Id == Guid.Empty, "DtoBaseCheck: Id == Empty");

                Assert.IsNotNull(dtoBase.CreatedByUserId, "DtoBaseCheck: CreatedByUserId == null");
                Assert.IsFalse(dtoBase.CreatedByUserId == Guid.Empty, "DtoBaseCheck: CreatedByUserId == Empty");
                BaseDtoValidator.CheckString(dtoBase.CreatedByUserFullName, "DtoBaseCheck: CreatedByUserFullName == null/Empty");

                Assert.IsNotNull(dtoBase.DateCreated, "DtoBaseCheck: DateCreated == null");
                Assert.IsFalse(dtoBase.DateCreated == DateTimeOffset.MinValue, "DtoBaseCheck: DateCreated == MinValue");

                if (checkModifiedBy)
                {
                    Assert.IsNotNull(dtoBase.LastModifiedByUserId, "DtoBaseCheck: LastModifiedByUserId == null");
                    Assert.IsFalse(dtoBase.LastModifiedByUserId == Guid.Empty, "DtoBaseCheck: LastModifiedByUserId == Empty");
                    BaseDtoValidator.CheckString(dtoBase.LastModifiedByUserFullName, "DtoBaseCheck: LastModifiedByUserFullName == null/Empty");

                    Assert.IsNotNull(dtoBase.DateLastModified, "DtoBaseCheck: DateLastModified == null");
                    Assert.IsFalse(dtoBase.DateLastModified == DateTimeOffset.MinValue, "DtoBaseCheck: DateLastModified == MinValue");
                }
            }catch(Exception e)
            {
                var msg = "Error Message: " + e.Message + "\r\n\r\nStack Trace: " + e.StackTrace;
                MessageBox.Show(msg, "Validation Error");
                throw e;
            }
}
    }
}
