using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Model.Dto;
using Locafi.Client.Model.Dto.Places;
using Locafi.Client.Model.Enums;
using Locafi.Client.Model.Query;
using Locafi.Client.Model.Query.PropertyComparison;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Locafi.Client.UnitTests.Tests
{
    [TestClass]
    public class PlaceRepoTests
    {
        private IPlaceRepo _placeRepo;
        private ITemplateRepo _templateRepo;
        private List<Guid> _toCleanup;
            
            
        [TestInitialize]
        public void Initialize()
        {
            _placeRepo = WebRepoContainer.PlaceRepo;
            _templateRepo = WebRepoContainer.TemplateRepo;
            _toCleanup = new List<Guid>();
        }

        [TestMethod]
        public async Task Place_Create()
        {
            var addPlace = await GenerateRandomAddPlaceDto();
            var result = await _placeRepo.CreatePlace(addPlace);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result,typeof(PlaceDetailDto));
            Assert.IsTrue(string.Equals(addPlace.Description, result.Description));
            Assert.IsTrue(string.Equals(addPlace.Name, result.Name));
            Assert.IsTrue(string.Equals(addPlace.TagNumber, result.TagNumber));

            _toCleanup.Add(result.Id);
        }
        

        [TestMethod]
        public async Task Place_GetAll()
        {
            var places = await _placeRepo.GetAllPlaces();
            Assert.IsNotNull(places);
            Assert.IsInstanceOfType(places, typeof(IEnumerable<PlaceSummaryDto>));
        }
        [TestMethod]
        public async Task Place_GetDetail()
        {
            // add one place in case there are none
            var addPlace = await GenerateRandomAddPlaceDto();
            var result = await _placeRepo.CreatePlace(addPlace);
            Assert.IsNotNull(result, "result != null");
            _toCleanup.Add(result.Id); // cleanup that place later

            var places = await _placeRepo.GetAllPlaces();
            Assert.IsTrue(places.Count > 0, "places.Count > 0");
            foreach (var summary in places)
            {
                var detail = await _placeRepo.GetPlaceById(summary.Id);
                Assert.IsNotNull(detail, "detail != null");
                Assert.AreEqual(detail,summary);
            }
        }


        [TestMethod]
        public async Task Place_Query()
        {
            var addPlace = await GenerateRandomAddPlaceDto();
            var place = await _placeRepo.CreatePlace(addPlace);
            Assert.IsNotNull(place);

            var q = new PlaceQuery();
            q.CreateQuery((p) => p.Name, place.Name, ComparisonOperator.Contains);
            var r = await _placeRepo.QueryPlaces(q);
            Assert.IsNotNull(r);
            Assert.IsTrue(r.Contains(place));

            q.CreateQuery(p=> p.LastModifiedByUserId, place.LastModifiedByUserId, ComparisonOperator.Equals);
            r = await _placeRepo.QueryPlaces(q);
            Assert.IsNotNull(r);
            Assert.IsTrue(r.Contains(place));

            q.CreateQuery(p=> p.DateCreated, place.DateCreated.AddDays(2), ComparisonOperator.LessThan);
            r = await _placeRepo.QueryPlaces(q);
            Assert.IsNotNull(r);
            Assert.IsTrue(r.Contains(place));

            q.CreateQuery(p=>p.Name, "", ComparisonOperator.Equals);
            r = await _placeRepo.QueryPlaces(q);

            q.CreateQuery((p) => p.DateCreated, place.DateCreated.Subtract(new TimeSpan(2, 0, 0, 0)), ComparisonOperator.GreaterThan);
            r = await _placeRepo.QueryPlaces(q);
            Assert.IsNotNull(r);
            Assert.IsTrue(r.Contains(place));
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
            var addPlace = await GenerateRandomAddPlaceDto(); // create randomly generated new place
            var place = await _placeRepo.CreatePlace(addPlace);
            _toCleanup.Add(place.Id);

            Assert.IsNotNull(place); // check we got something back
            Assert.IsInstanceOfType(place,typeof(PlaceDetailDto)); // check its the right type

            var allPlaces = await _placeRepo.GetAllPlaces(); // get all the current places
            Assert.IsTrue(allPlaces.Contains(place)); // check our place is in there
            await _placeRepo.Delete(place.Id); // try to delete our place

            allPlaces = await _placeRepo.GetAllPlaces(); // get all places again
            Assert.IsFalse(allPlaces.Any(p=> p.Id == place.Id)); // check our place is actually gone
        }

       

        [TestCleanup]
        public async void Cleanup()
        {
            foreach (var id in _toCleanup)
            {
               await _placeRepo.Delete(id); //TODO: when implemented
            }
        }

#region Private Methods


        private async Task<AddPlaceDto> GenerateRandomAddPlaceDto()
        {
            var ran = new Random();
            var description = Guid.NewGuid().ToString();
            var name = Guid.NewGuid().ToString();
            var tagNumber = Guid.NewGuid().ToString();
            var templates = await _templateRepo.GetTemplatesForType(TemplateFor.Place);
            var template = templates[ran.Next(templates.Count - 1)];

            var addPlace = new AddPlaceDto
            {
                Description = description,
                Name = name,
                TagNumber = tagNumber,
                TemplateId = template.Id,
                PlaceExtendedPropertyList = new List<WriteEntityExtendedPropertyDto>(),
                TagType = 0
            };
            return addPlace;
        }

        #endregion
    }
}
