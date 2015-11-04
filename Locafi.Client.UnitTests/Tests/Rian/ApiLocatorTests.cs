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
            var result = await LocafiApiLocator.GetApiBaseUrl("something"); // at the moment, there are no usernames, it all returns the same;
            Assert.IsInstanceOfType(result, typeof(IEnumerable<string>));
            Assert.IsTrue(result.Count > 0); // we got at least one URI
        }
    }
}
