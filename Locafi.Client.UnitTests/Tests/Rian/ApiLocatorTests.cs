using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Locafi.Client.UnitTests.Tests.Rian
{
    [TestClass]
    public class ApiLocatorTests
    {
        [TestMethod]
        public async Task ApiLocator_GetBaseUri()
        {
            var result = await LocafiApiLocator.GetApiBaseUrl("admin@ramp.com.au"); // this should have an API assocaited with it
            Assert.IsInstanceOfType(result, typeof(IDictionary<string,string>));
            Assert.IsTrue(result.Count > 0); // we got at least one URI
        }
    }
}
