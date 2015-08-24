using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using LegacyNavigatorApi.Models.Dtos;
using Locafi.Client.Contract.Services;
using Locafi.Client.Data;
using Locafi.Client.Model.Dto.Places;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Locafi.Client.UnitTests.Tests
{
    [TestClass]
    public class PlaceRepoTests
    {
        private IPlaceRepo _placeRepo;

        [TestInitialize]
        public void Initialize()
        {
            _placeRepo = WebRepoContainer.PlaceRepo;
        }

        [TestMethod]
        public async Task CreatePlace()
        {
            var addPlace = new AddPlaceDto 
            {
                Description = "Unit test Place",
                Name = "Teeeeeest",
                TagNumber = "sdjkbvjskdn",
                TemplateId = Guid.Empty,
                PlaceExtendedPropertyList = new List<WriteEntityExtendedPropertyDto>(),
                TagType = 0

            };
            var result = await _placeRepo.CreatePlace(addPlace);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result,typeof(PlaceDetailDto));
            Assert.IsTrue(string.Equals(addPlace.Description, result.Description));
            Assert.IsTrue(string.Equals(addPlace.Name, result.Name));
            Assert.IsTrue(string.Equals(addPlace.TagNumber, result.TagNumber));

        }
        [TestMethod]
        public async Task GetAllPlaces()
        {
            var places = await _placeRepo.GetAllPlaces();
            Assert.IsNotNull(places);
            Assert.IsInstanceOfType(places, typeof(IEnumerable<PlaceSummaryDto>));
        }

        public async Task UpdatePlace()
        {
            
        }

        
        public async Task DeletePlace()
        {
            
        }
    }
}
