using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Model.Dto;
using Locafi.Client.Model.Dto.Places;
using Locafi.Client.Model.Enums;
using Locafi.Client.Model.Query;
using Locafi.Client.Model.Query.PropertyComparison;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Locafi.Client.UnitTests.Validators;
using Locafi.Client.Model;
using Locafi.Client.Model.Query.Builder;
using Locafi.Client.Model.Dto.Tags;
using Locafi.Client.UnitTests.EntityGenerators;
using Locafi.Client.UnitTests.Extensions;

namespace Locafi.Client.UnitTests.Tests
{
    [TestClass]
    public class PlaceRepoTests
    {
        private IPlaceRepo _placeRepo;
        private ITemplateRepo _templateRepo;
        private List<Guid> _placesToCleanup;
            
            
        [TestInitialize]
        public void Initialize()
        {
            _placeRepo = WebRepoContainer.PlaceRepo;
            _templateRepo = WebRepoContainer.TemplateRepo;
            _placesToCleanup = new List<Guid>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            // delete all places that were created
            foreach (var placeId in _placesToCleanup)
            {
                _placeRepo.Delete(placeId).Wait();
            }
        }

        [TestMethod]
        public async Task Place_Create()
        {
            // create place
            var addPlace = await PlaceGenerator.GenerateRandomAddPlaceDto();
            var result = await _placeRepo.CreatePlace(addPlace);
            _placesToCleanup.AddUnique(result.Id);

            // Check Result
            PlaceDtoValidator.PlaceDetailCheck(result);
            Validator.IsInstanceOfType(result,typeof(PlaceDetailDto));
            Validator.IsTrue(string.Equals(addPlace.Description, result.Description));
            Validator.IsTrue(string.Equals(addPlace.Name, result.Name));
        }

        [TestMethod]
        public async Task Place_GetAll()
        {
            // ensure that there is at least one place to get
            var addPlace = await PlaceGenerator.GenerateRandomAddPlaceDto();
            var result = await _placeRepo.CreatePlace(addPlace);
            _placesToCleanup.AddUnique(result.Id);

            // Check Result
            PlaceDtoValidator.PlaceDetailCheck(result);

            // get all places
            var places = await _placeRepo.QueryPlaces(QueryBuilder<PlaceSummaryDto>.NewQuery().TakeAll().Build());

            // check that we have at least 1 place returned
            Validator.IsNotNull(places);
            Validator.IsInstanceOfType(places, typeof(PageResult<PlaceSummaryDto>));
            Validator.IsTrue(places.Items.Count() > 0);
        }

        [TestMethod]
        public async Task Place_GetDetail()
        {
            // add one place in case there are none
            var addPlace = await PlaceGenerator.GenerateRandomAddPlaceDto();
            var result = await _placeRepo.CreatePlace(addPlace);
            _placesToCleanup.AddUnique(result.Id); // cleanup that place later

            // Check Result
            PlaceDtoValidator.PlaceDetailCheck(result);

            // now get the place back
            var place = await _placeRepo.GetPlaceById(result.Id);

            // check the result
            PlaceDtoValidator.PlaceDetailCheck(place);
            Validator.AreEqual(result, place);
        }


        [TestMethod]
        public async Task Place_QueryContinuation()
        {
            PlaceDetailDto place = null;
            // create a bunch of places to query
            for (int i = 0; i < 10; i++)
            {
                var addPlace = await PlaceGenerator.GenerateRandomAddPlaceDto();
                place = await _placeRepo.CreatePlace(addPlace);
                _placesToCleanup.AddUnique(place.Id); // cleanup that place later

                // check result
                PlaceDtoValidator.PlaceDetailCheck(place);
            }

            // query one of the place's we created
            var q = PlaceQuery.NewQuery((p) => p.Name, place.Name, ComparisonOperator.Contains);
            var r = await _placeRepo.QueryPlacesContinuation(q);

            // check result
            Validator.IsNotNull(r);
            Validator.IsInstanceOfType(r, typeof(IQueryResult<PlaceSummaryDto>));
            Validator.IsTrue(r.Entities.Contains(place));

            // now query just to get the frist 2 places
            var query = QueryBuilder<PlaceSummaryDto>.NewQuery().Take(2).Build(); // get first 2 places
            r = await _placeRepo.QueryPlacesContinuation(query);

            // check result
            Validator.IsNotNull(r);
            Validator.IsNotNull(r.ContinuationQuery);
            Validator.IsInstanceOfType(r, typeof(IQueryResult<PlaceSummaryDto>));

            // now use the continuation query to get the next lot of results
            var r2 = await _placeRepo.QueryPlacesContinuation(r.ContinuationQuery);

            // check result
            Validator.IsNotNull(r2);
            Validator.IsNotNull(r2.Entities);
            Validator.AreEqual(r2.Entities.Count, 2);
        }

        [TestMethod]
        public async Task Place_Query()
        {
            // create place
            var addPlace = await PlaceGenerator.GenerateRandomAddPlaceDto();
            var place = await _placeRepo.CreatePlace(addPlace);
            _placesToCleanup.AddUnique(place.Id); // cleanup that place later

            // check result
            PlaceDtoValidator.PlaceDetailCheck(place);

            // query the place we created based on name
            var q = new PlaceQuery();
            q.CreateQuery((p) => p.Name, place.Name, ComparisonOperator.Contains);
            var r = await _placeRepo.QueryPlaces(q);

            r = await _placeRepo.QueryPlaces();

            // check result
            Validator.IsNotNull(r);
            Validator.IsTrue(r.Contains(place));
            Validator.IsInstanceOfType(r, typeof(PageResult<PlaceSummaryDto>));

            // query the place we created based on user id
            q.CreateQuery(p => p.CreatedByUserId, place.CreatedByUserId, ComparisonOperator.Equals);
            r = await _placeRepo.QueryPlaces(q);

            // check result
            Validator.IsNotNull(r);
            Validator.IsTrue(r.Contains(place));
            Validator.IsInstanceOfType(r, typeof(PageResult<PlaceSummaryDto>));

            // failing due to long lists
            //q.CreateQuery(p=> p.DateCreated, place.DateCreated.AddDays(2), ComparisonOperator.LessThan);
            //r = await _placeRepo.QueryPlaces(q);
            //Assert.IsNotNull(r);
            //Assert.IsTrue(r.Contains(place));

            // query the place we created based date created
            q.CreateQuery((p) => p.DateCreated, place.DateCreated.Subtract(new TimeSpan(0, 0, 0, 10)), ComparisonOperator.GreaterThan);
            r = await _placeRepo.QueryPlaces(q);

            // check result
            Validator.IsNotNull(r);
            Validator.IsTrue(r.Contains(place));
            Validator.IsInstanceOfType(r, typeof(PageResult<PlaceSummaryDto>));
        }

        [TestMethod]
        public async Task Place_Update()
        {
            // create place
            var addPlace = await PlaceGenerator.GenerateRandomAddPlaceDto();
            var place = await _placeRepo.CreatePlace(addPlace);
            _placesToCleanup.AddUnique(place.Id); // cleanup that place later

            // check result
            PlaceDtoValidator.PlaceDetailCheck(place);

            // update place
            var updateDto = new UpdatePlaceDto(place)
            {
                Name = place.Name + " - Update",
                Description = place.Description + " - update"
            };
            // loop through each extended property and change
            foreach (var prop in place.PlaceExtendedPropertyList)
            {
                var newProp = updateDto.PlaceExtendedPropertyList.Where(p => p.ExtendedPropertyId == prop.ExtendedPropertyId).FirstOrDefault();

                switch (prop.ExtendedPropertyDataType)
                {
                    case TemplateDataTypes.AutoId: newProp.Value = new Random(DateTime.UtcNow.Millisecond).Next().ToString(); break;
                    case TemplateDataTypes.Bool: newProp.Value = true.ToString(); break;
                    case TemplateDataTypes.DateTime: newProp.Value = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssK"); break;
                    case TemplateDataTypes.Decimal: newProp.Value = (((double)new Random(DateTime.UtcNow.Millisecond).Next()) / 10.0).ToString(); break;
                    case TemplateDataTypes.Number: newProp.Value = new Random(DateTime.UtcNow.Millisecond).Next().ToString(); break;
                    case TemplateDataTypes.String: newProp.Value = Guid.NewGuid().ToString(); break;
                }
            }

            // update the place
            var updatePlaceResult = await _placeRepo.UpdatePlace(updateDto);

            // check the result
            PlaceDtoValidator.PlaceDetailCheck(updatePlaceResult);
            Validator.IsTrue(string.Equals(updateDto.Name, updatePlaceResult.Name));
            Validator.IsTrue(string.Equals(updateDto.Description, updatePlaceResult.Description));
            Validator.AreEqual(updateDto.TemplateId, updatePlaceResult.TemplateId);
            // check the extended properties were changed
            foreach (var prop in updatePlaceResult.PlaceExtendedPropertyList)
            {
                var dtoProp = updateDto.PlaceExtendedPropertyList.FirstOrDefault(p => p.ExtendedPropertyId == prop.ExtendedPropertyId);
                Validator.AreEqual(prop.Value, dtoProp?.Value);
            }
        }

        [TestMethod]
        public async Task Place_UpdateTag()
        {
            // create place
            var addPlace = await PlaceGenerator.GenerateRandomAddPlaceDto();
            var place = await _placeRepo.CreatePlace(addPlace);
            _placesToCleanup.AddUnique(place.Id); // cleanup that place later

            // check result
            PlaceDtoValidator.PlaceDetailCheck(place);

            // update place
            var updateDto = new UpdatePlaceTagDto()
            {
                Id = place.Id,
                PlaceTagList = new List<WriteTagDto>()
                {
                    new WriteTagDto()
                    {
                        TagNumber = Guid.NewGuid().ToString(),
                        TagType = TagType.PassiveRfid
                    }
                }
            };

            // update the place tag
            var updatePlaceResult = await _placeRepo.UpdatePlaceTag(updateDto);

            // check the result
            PlaceDtoValidator.PlaceDetailCheck(updatePlaceResult);
            Validator.IsTrue(string.Equals(updateDto.PlaceTagList.First().TagNumber, updatePlaceResult.TagNumber));
            Validator.IsTrue(string.Equals(updateDto.PlaceTagList.First().TagNumber, updatePlaceResult.PlaceTagList.First().TagNumber));
            Validator.IsFalse(string.Equals(place.PlaceTagList.First().TagNumber, updatePlaceResult.PlaceTagList.First().TagNumber));
        }

        [TestMethod]
        public async Task Place_Delete()
        {
            var addPlace = await PlaceGenerator.GenerateRandomAddPlaceDto(); // create randomly generated new place
            var place = await _placeRepo.CreatePlace(addPlace);
            _placesToCleanup.AddUnique(place.Id); // cleanup that place later

            // check result
            PlaceDtoValidator.PlaceDetailCheck(place);
            Validator.IsInstanceOfType(place,typeof(PlaceDetailDto)); // check its the right type

            var query = QueryBuilder<PlaceSummaryDto>.NewQuery(p => p.Id, place.Id, ComparisonOperator.Equals).Build();
            var queryResult = await _placeRepo.QueryPlaces(query); // get the place

            // check our place is in there
            Validator.IsTrue(queryResult.Contains(place));

            // try to delete our place
            var deleteResult = await _placeRepo.Delete(place.Id);
            // check result
            Validator.IsTrue(deleteResult);
            // remove from delete list
            _placesToCleanup.Remove(place.Id);

            // verify
            queryResult = await _placeRepo.QueryPlaces(query); // get the place again
            Validator.IsFalse(queryResult.Any(p=> p.Id == place.Id)); // check our place is actually gone         

            // verify with get
            try
            {
                var sameItem = await _placeRepo.GetPlaceById(place.Id);

                Validator.IsTrue(false, "Deleted entity returned");
            }
            catch (Exception e)
            {
                // this is expected                
            }
        }
    }
}
