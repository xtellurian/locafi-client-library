using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using LegacyNavigatorApi.Models.Dtos;
using Locafi.Client.Contract.Services;
using Locafi.Client.Data;
using Locafi.Client.Model.Dto.Places;
using Locafi.Client.Model.Query;
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
        public async Task Place_Create()
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
        public async Task Place_GetAll()
        {
            var places = await _placeRepo.GetAllPlaces();
            Assert.IsNotNull(places);
            Assert.IsInstanceOfType(places, typeof(IEnumerable<PlaceSummaryDto>));
        }

        [TestMethod]
        public async Task Place_Query()
        {
            var addPlace = GenerateAddPlaceDto();
            var place = await _placeRepo.CreatePlace(addPlace);
            Assert.IsNotNull(place);

            var query1 = new SimplePlaceQuery(place.Name);
            var result1 = await _placeRepo.QueryPlaces(query1);
            Assert.IsNotNull(result1);
            Assert.IsTrue(result1.Contains(place));
        }

        public async Task Place_Update()
        {
            var ran = new Random();
            var places = await _placeRepo.GetAllPlaces();
            var place = places[ran.Next(places.Count - 1)];

            

        }

        
        [TestMethod]
        public async Task Place_Delete()
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
        public async void Cleanup()
        {
            foreach (var id in _placeIds)
            {
               // await _placeRepo.Place_Delete(id); //TODO: when implemented
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
