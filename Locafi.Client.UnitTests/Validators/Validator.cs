using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Locafi.Client.UnitTests.Validators
{
    public static class Validator
    {
        public static void IsTrue(bool condition, string msg = "")
        {
            try
            {
                Assert.IsTrue(condition, msg);
            }catch(Exception e)
            {
                var eMsg = "Error Message: " + e.Message + "\r\nStack Trace: " + e.StackTrace;
                MessageBox.Show(eMsg, "Validation Error");
                throw e;
            }
        }

        public static void IsFalse(bool condition, string msg = "")
        {
            try
            {
                Assert.IsFalse(condition, msg);
            }
            catch (Exception e)
            {
                var eMsg = "Error Message: " + e.Message + "\r\nStack Trace: " + e.StackTrace;
                MessageBox.Show(eMsg, "Validation Error");
                throw e;
            }
        }

        public static void AreEqual(object expected, object actual, string msg = "")
        {
            try
            {
                Assert.AreEqual(expected, actual, msg);
            }
            catch (Exception e)
            {
                var eMsg = "Error Message: " + e.Message + "\r\nStack Trace: " + e.StackTrace;
                MessageBox.Show(eMsg, "Validation Error");
                throw e;
            }
        }

        public static void AreNotEqual(object expected, object actual, string msg = "")
        {
            try
            {
                Assert.AreNotEqual(expected, actual, msg);
            }
            catch (Exception e)
            {
                var eMsg = "Error Message: " + e.Message + "\r\nStack Trace: " + e.StackTrace;
                MessageBox.Show(eMsg, "Validation Error");
                throw e;
            }
        }

        public static void IsNull(object value, string msg = "")
        {
            try
            {
                Assert.IsNull(value, msg);
            }
            catch (Exception e)
            {
                var eMsg = "Error Message: " + e.Message + "\r\nStack Trace: " + e.StackTrace;
                MessageBox.Show(eMsg, "Validation Error");
                throw e;
            }
        }

        public static void IsNotNull(object value, string msg = "")
        {
            try
            {
                Assert.IsNotNull(value, msg);
            }
            catch (Exception e)
            {
                var eMsg = "Error Message: " + e.Message + "\r\nStack Trace: " + e.StackTrace;
                MessageBox.Show(eMsg, "Validation Error");
                throw e;
            }
        }

        public static void IsInstanceOfType(object value, Type type, string msg = "")
        {
            try
            {
                Assert.IsInstanceOfType(value, type, msg);
            }
            catch (Exception e)
            {
                var eMsg = "Error Message: " + e.Message + "\r\nStack Trace: " + e.StackTrace;
                MessageBox.Show(eMsg, "Validation Error");
                throw e;
            }
        }
    }
}
