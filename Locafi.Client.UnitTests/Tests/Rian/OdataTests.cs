using Locafi.Client.Odata;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Locafi.Client.UnitTests.Tests.Rian
{
    [TestClass]
    public class OdataTests
    {
        [TestMethod]
        public void Odata_example()
        {
            var x = new RandomOdataExampleCode();
            x.Something();
        }
    }
}
