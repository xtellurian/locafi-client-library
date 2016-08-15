using Locafi.Client.Model.Dto.Authentication;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Locafi.Client.UnitTests.Validators
{
    public static class AuthenticationDtoValidator
    {
        public static void AuthenticationResponseCheck(AuthenticationResponseDto dto, bool succesfullLogin)
        {
            try {
                Assert.IsNotNull(dto, "AuthenticationResponseCheck: dto == null");
                Assert.IsNotNull(dto.TokenGroup, "AuthenticationResponseCheck: TokenGroup == null");
                Assert.IsNotNull(dto.Messages, "AuthenticationResponseCheck: Messages == null");

                if (succesfullLogin)
                {
                    Assert.IsTrue(dto.Success, "AuthenticationResponseCheck: not a succesfull login");
                    BaseDtoValidator.CheckString(dto.TokenGroup.Token, "AuthenticationResponseCheck: Token == null/Empty");
                    BaseDtoValidator.CheckString(dto.TokenGroup.Refresh, "AuthenticationResponseCheck: Refresh == null/Empty");
                }
                else
                {
                    Assert.IsFalse(dto.Success, "AuthenticationResponseCheck: login was succesful");
                    Assert.IsTrue(dto.Messages.Count > 0, "AuthenticationResponseCheck: no error messages");
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
