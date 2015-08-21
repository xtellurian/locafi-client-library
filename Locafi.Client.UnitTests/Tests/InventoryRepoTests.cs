using System;
using System.Threading.Tasks;
using Locafi.Client.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Locafi.Client.UnitTests.Tests
{
    [TestClass]
    public class InventoryRepoTests : HttpRepoContainer
    {
        private const string BaseUrl = @"http://legacynavapi.azurewebsites.net/api/";
        private const string UserName = "Rian";
        private const string Password = "Ramp11";
        public InventoryRepoTests() : base(BaseUrl, UserName, Password)
        {
        }

       


    }
}
