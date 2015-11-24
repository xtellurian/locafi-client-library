using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Locafi.Client.UnitTests.Tests.Rian
{
    [TestClass]
    public class ApiLocatorTests
    {
        private const string KnownEmailAddress = "admin@ramp.com.au";
        private const string KnownDeviceSerialNumber = "123";

        [TestMethod]
        public async Task ApiLocator_GetBaseUri()
        {
            var result = await LocafiApiLocator.GetApiBaseUrl(KnownEmailAddress); // this should have an API assocaited with it
            Assert.IsInstanceOfType(result, typeof(IDictionary<string,string>));
            Assert.IsTrue(result.Count > 0); // we got at least one URI

            result = await LocafiApiLocator.GetApiBaseUrl(Guid.NewGuid().ToString()); // for an unknown user
            Assert.IsTrue(result.Keys.Count > 0, "At least the public API should be returned");
        }


        [TestMethod]
        public async Task ApiLocator_GetBaseUriForDevice()
        {
            var result = await LocafiApiLocator.GetApiBaseUrlForDevice(KnownDeviceSerialNumber);
            Assert.IsNotNull(result, "Result was null when getting known serial number");
            Assert.IsInstanceOfType(result, typeof(string));
            var uri = new Uri(result); // to check is valid URI


            result = await LocafiApiLocator.GetApiBaseUrlForDevice(Guid.NewGuid().ToString()); // unknown device
            Assert.IsNull(result);
        }
    }
}
