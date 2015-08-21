using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Locafi.Client.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Locafi.Client.UnitTests.Tests
{
    [TestClass]
    public class PlaceRepoTests : HttpRepoContainer
    {
        private const string BaseUrl = @"http://legacynavapi.azurewebsites.net/api/";
        private const string UserName = "Rian";
        private const string Password = "Ramp11";

        public PlaceRepoTests() : base(BaseUrl, UserName, Password)
        {
        }

        [TestMethod]
        public async Task AddNewPlace()
        {
            var place = new PlaceDto
            {
                Description = "Unit test Place",
                Name = "Teeeeeest",
                Id = Guid.Empty,
                ImageURIs = new List<string>(),


            };
            var result = await PlaceRepo.AddNewPlace(place);
            Assert.IsInstanceOfType(result,typeof(PlaceDto));


        }
        [TestMethod]
        public async Task GetAllPlaces()
        {
            var places = await PlaceRepo.GetAllPlaces();
            Assert.IsNotNull(places);
            Assert.IsInstanceOfType(places, typeof(IEnumerable<PlaceDto>));
        }

        public async Task UpdatePlace()
        {
            
        }

        
        public async Task DeletePlace()
        {
            
        }
    }
}
