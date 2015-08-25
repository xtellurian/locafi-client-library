using System;
using System.Collections.Generic;
using System.Linq;
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
        private List<string> _placeIds;
            
            
        [TestInitialize]
        public void Initialize()
        {
            _placeRepo = WebRepoContainer.PlaceRepo;
            _placeIds = new List<string>();
        }

        [TestMethod]
        public async Task CreatePlace()
        {
            var addPlace = GenerateAddPlaceDto();
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
            var ran = new Random();
            var places = await _placeRepo.GetAllPlaces();
            var place = places[ran.Next(places.Count - 1)];

            

        }

        
        [TestMethod]
        public async Task DeletePlace()
        {
            var addPlace = GenerateAddPlaceDto(); // create randomly generated new place
            var place = await UploadNewPlace(addPlace); // uplaod that place to the server
            Assert.IsNotNull(place); // check we got something back
            Assert.IsInstanceOfType(place,typeof(PlaceDetailDto)); // check its the right type

            var allPlaces = await _placeRepo.GetAllPlaces(); // get all the current places
            Assert.IsTrue(allPlaces.Contains(place)); // check our place is in there
            await _placeRepo.DeletePlace(place.Id.ToString()); // try to delete our place

            allPlaces = await _placeRepo.GetAllPlaces(); // get all places again
            Assert.IsFalse(allPlaces.Any(p=> p.Id == place.Id)); // check our place is actually gone
        }

       

        [TestCleanup]
        public async Task Cleanup()
        {
            foreach (var id in _placeIds)
            {
               // await _placeRepo.DeletePlace(id); //TODO: when implemented
            }
        }

        // private methods

        private async Task<PlaceDetailDto> UploadNewPlace(AddPlaceDto addPlace)
        {
            var place = await _placeRepo.CreatePlace(addPlace);
            _placeIds.Add(place.Id.ToString());
            return place;
        }


        private static AddPlaceDto GenerateAddPlaceDto()
        {
            var description = Guid.NewGuid().ToString();
            var name = Guid.NewGuid().ToString();
            var tagNumber = Guid.NewGuid().ToString();

            var addPlace = new AddPlaceDto
            {
                Description = description,
                Name = name,
                TagNumber = tagNumber,
                TemplateId = Guid.Empty,
                PlaceExtendedPropertyList = new List<WriteEntityExtendedPropertyDto>(),
                TagType = 0
            };
            return addPlace;
        }
    }
}
