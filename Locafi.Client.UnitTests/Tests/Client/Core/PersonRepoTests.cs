using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Model.Dto.Persons;
using Locafi.Client.Model.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Locafi.Client.Model.Dto.Tags;
using Locafi.Client.Exceptions;
using Locafi.Client.UnitTests.Validators;
using Locafi.Client.Model;
using Locafi.Client.Model.Query.Builder;
using Locafi.Client.Model.Dto.Places;
using Locafi.Client.Model.Query;
using Locafi.Client.UnitTests.EntityGenerators;
using Locafi.Client.UnitTests.Extensions;
using Locafi.Client.Model.Dto;

namespace Locafi.Client.UnitTests.Tests
{
    [TestClass]
    public class PersonRepoTests
    {
        private IPersonRepo _personRepo;
        private ITemplateRepo _templateRepo;
        private IExtendedPropertyRepo _extPropRepo;

        private IList<Guid> _personsToCleanup;
        private List<Guid> _templatesToDelete;
        private List<Guid> _extpropsToDelete;

        [TestInitialize]
        public void Initialise()
        {
            _personRepo = WebRepoContainer.PersonRepo;
            _templateRepo = WebRepoContainer.TemplateRepo;
            _extPropRepo = WebRepoContainer.ExtendedPropertyRepo;

            _personsToCleanup = new List<Guid>();
            _templatesToDelete = new List<Guid>();
            _extpropsToDelete = new List<Guid>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            // delete all that were created
            foreach (var Id in _personsToCleanup)
            {
                _personRepo.DeletePerson(Id).Wait();
            }

            foreach (var id in _templatesToDelete)
            {
                _templateRepo.DeleteTemplate(id).Wait();
            }

            foreach (var id in _extpropsToDelete)
            {
                _extPropRepo.DeleteExtendedProperty(id).Wait();
            }
        }

        [TestMethod]
        public async Task Person_Create()
        {
            // create person
            var addPerson = await PersonGenerator.GenerateRandomAddPersonDto();
            var detail = await _personRepo.CreatePerson(addPerson);
            _personsToCleanup.AddUnique(detail.Id);

            // check result
            PersonDtoValidator.PersonDetailCheck(detail);
            Validator.IsInstanceOfType(detail,typeof(PersonDetailDto));
            Validator.IsTrue(string.Equals(addPerson.Email,detail.Email));
            Validator.IsTrue(string.Equals(addPerson.GivenName, detail.GivenName));
            Validator.IsTrue(string.Equals(addPerson.Surname, detail.Surname));
            Validator.IsTrue(string.Equals(addPerson.ImageUrl, detail.ImageUrl));
            Validator.IsTrue(string.Equals(addPerson.PersonTagList[0].TagNumber.ToUpper(), detail.TagNumber));
        }

        [TestMethod]
        public async Task Person_Get()
        {
            // create person
            var addPerson = await PersonGenerator.GenerateRandomAddPersonDto();
            var detail = await _personRepo.CreatePerson(addPerson);
            _personsToCleanup.AddUnique(detail.Id);

            // check result
            PersonDtoValidator.PersonDetailCheck(detail);
            Validator.IsInstanceOfType(detail, typeof(PersonDetailDto));
            Validator.IsTrue(string.Equals(addPerson.Email, detail.Email));
            Validator.IsTrue(string.Equals(addPerson.GivenName, detail.GivenName));
            Validator.IsTrue(string.Equals(addPerson.Surname, detail.Surname));
            Validator.IsTrue(string.Equals(addPerson.PersonTagList[0].TagNumber.ToUpper(), detail.TagNumber));

            // get the person
            detail = await _personRepo.GetPersonById(detail.Id);

            // check result
            PersonDtoValidator.PersonDetailCheck(detail);
            Validator.IsInstanceOfType(detail, typeof(PersonDetailDto));
            Validator.IsTrue(string.Equals(addPerson.Email, detail.Email));
            Validator.IsTrue(string.Equals(addPerson.GivenName, detail.GivenName));
            Validator.IsTrue(string.Equals(addPerson.Surname, detail.Surname));
            Validator.IsTrue(string.Equals(addPerson.PersonTagList[0].TagNumber.ToUpper(), detail.TagNumber));
        }

        [TestMethod]
        public async Task Person_GetAll()
        {
            // create person
            var addPerson = await PersonGenerator.GenerateRandomAddPersonDto();
            var detail = await _personRepo.CreatePerson(addPerson);
            _personsToCleanup.AddUnique(detail.Id);

            // query persons
            var persons = await _personRepo.QueryPersons();

            // check result
            Validator.IsNotNull(persons, "persons != null");
            Validator.IsInstanceOfType(persons, typeof(PageResult<PersonSummaryDto>));
            Validator.IsTrue(persons.Items.Count() > 0);
            Validator.IsTrue(persons.Contains(detail));
        }

        [TestMethod]
        public async Task Person_Update()
        {
            // create person
            var addPerson = await PersonGenerator.GenerateRandomAddPersonDto();
            var detail = await _personRepo.CreatePerson(addPerson);
            _personsToCleanup.AddUnique(detail.Id);

            // check result
            PersonDtoValidator.PersonDetailCheck(detail);
            Validator.IsInstanceOfType(detail, typeof(PersonDetailDto));
            Validator.IsTrue(string.Equals(addPerson.Email, detail.Email));
            Validator.IsTrue(string.Equals(addPerson.GivenName, detail.GivenName));
            Validator.IsTrue(string.Equals(addPerson.Surname, detail.Surname));
            Validator.IsTrue(string.Equals(addPerson.PersonTagList[0].TagNumber.ToUpper(), detail.TagNumber));

            // update the person
            var updateDto = new UpdatePersonDto(detail);
            updateDto.GivenName += " - Update";
            updateDto.Surname += " - Update";
            updateDto.Email += " - Update";
            updateDto.ImageUrl += " - Update";
            var updateDetail = await _personRepo.UpdatePerson(updateDto);

            // check the response
            PersonDtoValidator.PersonDetailCheck(updateDetail);
            Validator.IsInstanceOfType(detail, typeof(PersonDetailDto));
            Validator.IsFalse(string.Equals(addPerson.Email, updateDetail.Email));
            Validator.IsFalse(string.Equals(addPerson.GivenName, updateDetail.GivenName));
            Validator.IsFalse(string.Equals(addPerson.Surname, updateDetail.Surname));
            Validator.IsFalse(string.Equals(addPerson.ImageUrl, updateDetail.ImageUrl));
            Validator.IsTrue(string.Equals(addPerson.PersonTagList[0].TagNumber.ToUpper(), updateDetail.TagNumber));
            Validator.IsTrue(string.Equals(updateDto.Email, updateDetail.Email));
            Validator.IsTrue(string.Equals(updateDto.GivenName, updateDetail.GivenName));
            Validator.IsTrue(string.Equals(updateDto.Surname, updateDetail.Surname));
            Validator.IsTrue(string.Equals(updateDto.ImageUrl, updateDetail.ImageUrl));
        }

        [TestMethod]
        public async Task Person_UpdateTag()
        {
            // create person
            var addPerson = await PersonGenerator.GenerateRandomAddPersonDto();
            var detail = await _personRepo.CreatePerson(addPerson);
            _personsToCleanup.AddUnique(detail.Id);

            // check result
            PersonDtoValidator.PersonDetailCheck(detail);
            Validator.IsInstanceOfType(detail, typeof(PersonDetailDto));
            Validator.IsTrue(string.Equals(addPerson.Email, detail.Email));
            Validator.IsTrue(string.Equals(addPerson.GivenName, detail.GivenName));
            Validator.IsTrue(string.Equals(addPerson.Surname, detail.Surname));
            Validator.IsTrue(string.Equals(addPerson.PersonTagList[0].TagNumber.ToUpper(), detail.TagNumber));

            // update the person tag
            var updateDto = new UpdatePersonTagDto()
            {
                Id = detail.Id,
                PersonTagList = new List<WriteTagDto>()
                {
                    new WriteTagDto()
                    {
                        TagNumber = Guid.NewGuid().ToString(),
                        TagType = TagType.PassiveRfid
                    }
                }
            };
            var updateDetail = await _personRepo.UpdatePersonTag(updateDto);

            // check the response
            PersonDtoValidator.PersonDetailCheck(updateDetail);
            Validator.IsInstanceOfType(detail, typeof(PersonDetailDto));
            Validator.IsTrue(string.Equals(addPerson.Email, updateDetail.Email));
            Validator.IsTrue(string.Equals(addPerson.GivenName, updateDetail.GivenName));
            Validator.IsTrue(string.Equals(addPerson.Surname, updateDetail.Surname));
            Validator.IsFalse(string.Equals(addPerson.PersonTagList[0].TagNumber.ToUpper(), updateDetail.TagNumber));
            Validator.IsTrue(string.Equals(updateDto.PersonTagList[0].TagNumber.ToUpper(), updateDetail.TagNumber));
        }

        [TestMethod]
        public async Task Person_Delete()
        {
            // create person
            var addPerson = await PersonGenerator.GenerateRandomAddPersonDto();
            var detail = await _personRepo.CreatePerson(addPerson);
            _personsToCleanup.AddUnique(detail.Id);

            // check result
            PersonDtoValidator.PersonDetailCheck(detail);
            Validator.IsInstanceOfType(detail, typeof(PersonDetailDto));
            Validator.IsTrue(string.Equals(addPerson.Email, detail.Email));
            Validator.IsTrue(string.Equals(addPerson.GivenName, detail.GivenName));
            Validator.IsTrue(string.Equals(addPerson.Surname, detail.Surname));
            Validator.IsTrue(string.Equals(addPerson.PersonTagList[0].TagNumber.ToUpper(), detail.TagNumber));

            // get the person
            detail = await _personRepo.GetPersonById(detail.Id);

            // check result
            PersonDtoValidator.PersonDetailCheck(detail);
            Validator.IsInstanceOfType(detail, typeof(PersonDetailDto));
            Validator.IsTrue(string.Equals(addPerson.Email, detail.Email));
            Validator.IsTrue(string.Equals(addPerson.GivenName, detail.GivenName));
            Validator.IsTrue(string.Equals(addPerson.Surname, detail.Surname));
            Validator.IsTrue(string.Equals(addPerson.PersonTagList[0].TagNumber.ToUpper(), detail.TagNumber));

            // delete the person
            var deleteResult = await _personRepo.DeletePerson(detail.Id);
            Validator.IsTrue(deleteResult);
            // remove from delete list
            _personsToCleanup.Remove(detail.Id);

            // verify
            var query = QueryBuilder<PersonSummaryDto>.NewQuery(p => p.Id, detail.Id, ComparisonOperator.Equals).Build();
            var queryResult = await _personRepo.QueryPersons(query); // get the person again
            Validator.IsFalse(queryResult.Any(p => p.Id == detail.Id)); // check our person is actually gone            

            // verify with get
            try
            {
                var sameItem = await _personRepo.GetPersonById(detail.Id);

                Validator.IsTrue(false, "Deleted entity returned");
            }
            catch (Exception e)
            {
                // this is expected                
            }
        }

        [TestMethod]
        public async Task Person_TestAllExtendedPropertyTypes()
        {
            // create full person template
            var addTemplateDto = await TemplateGenerator.GenerateAddTemplateDtoWithFullExtProps(TemplateFor.Person);
            var template = await _templateRepo.CreateTemplate(addTemplateDto);
            _templatesToDelete.AddUnique(template.Id);
            _extpropsToDelete.AddRangeUnique(template.TemplateExtendedPropertyList.Select(e => e.ExtendedPropertyId));

            // create person
            var addDto = await PersonGenerator.GenerateRandomAddPersonDto(null,template);
            var result = await _personRepo.CreatePerson(addDto);
            _personsToCleanup.AddUnique(result.Id);

            PersonDtoValidator.PersonDetailCheck(result);

            // check every extended property
            var tempalteDetail = await _templateRepo.GetById(template.Id);
            foreach (var templateExtendedProperty in tempalteDetail.TemplateExtendedPropertyList)
            {
                var extendedProperty = result.PersonExtendedPropertyList
                    .FirstOrDefault(e => e.ExtendedPropertyId == templateExtendedProperty.ExtendedPropertyId);
                Validator.IsNotNull(extendedProperty, "Extended property was null");
                var addExtendedProperty = addDto.PersonExtendedPropertyList
                    .FirstOrDefault(e => e.ExtendedPropertyId == templateExtendedProperty.ExtendedPropertyId);
                Validator.IsTrue(ExtendedPropertyDtoValidator.CanParseDtoValue(extendedProperty));
                Validator.IsTrue(ExtendedPropertyDtoValidator.ParsedValuesAreEqual(extendedProperty, addExtendedProperty));
            }

            // now do a get to check it works
            var getResult = await _personRepo.GetPersonById(result.Id);
            PersonDtoValidator.PersonDetailCheck(getResult);

            // check every extended property
            foreach (var templateExtendedProperty in tempalteDetail.TemplateExtendedPropertyList)
            {
                var extendedProperty = getResult.PersonExtendedPropertyList
                    .FirstOrDefault(e => e.ExtendedPropertyId == templateExtendedProperty.ExtendedPropertyId);
                Validator.IsNotNull(extendedProperty, "Extended property was null");
                var addExtendedProperty = addDto.PersonExtendedPropertyList
                    .FirstOrDefault(e => e.ExtendedPropertyId == templateExtendedProperty.ExtendedPropertyId);
                Validator.IsTrue(ExtendedPropertyDtoValidator.CanParseDtoValue(extendedProperty));
                Validator.IsTrue(ExtendedPropertyDtoValidator.ParsedValuesAreEqual(extendedProperty, addExtendedProperty));
            }

            // now update extended properties

            // build update item dto, but only change the extended properties
            var updateDto = new UpdatePersonDto()
            {
                Id = result.Id,
                Email = $"{Guid.NewGuid().ToString().Substring(0, 16)}@FakeDomain.com",
                GivenName = "Random - " + template.Name,
                Surname = "Random - " + template.Name + " - Surname",
                TemplateId = result.TemplateId
            };

            // loop through each extended property and change
            foreach (var prop in result.PersonExtendedPropertyList)
            {
                var newProp = new WriteEntityExtendedPropertyDto()
                {
                    ExtendedPropertyId = prop.ExtendedPropertyId
                };

                switch (prop.ExtendedPropertyDataType)
                {
                    //                    case TemplateDataTypes.AutoId: newProp.Value = new Random(DateTime.UtcNow.Millisecond).Next().ToString(); break;
                    case TemplateDataTypes.Bool: newProp.Value = true.ToString(); break;
                    case TemplateDataTypes.DateTime: newProp.Value = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssK"); break;
                    case TemplateDataTypes.Decimal: newProp.Value = (((double)new Random(DateTime.UtcNow.Millisecond).Next()) / 10.0).ToString(); break;
                    case TemplateDataTypes.Number: newProp.Value = new Random(DateTime.UtcNow.Millisecond).Next().ToString(); break;
                    case TemplateDataTypes.String: newProp.Value = Guid.NewGuid().ToString(); break;
                }

                updateDto.PersonExtendedPropertyList.Add(newProp);
            }

            // update the entity
            var updateResult = await _personRepo.UpdatePerson(updateDto);

            // check the result
            PersonDtoValidator.PersonDetailCheck(updateResult);
            Validator.IsTrue(string.Equals(updateDto.GivenName, updateResult.GivenName));
            Validator.IsTrue(string.Equals(updateDto.Surname, updateResult.Surname));
            Validator.IsTrue(string.Equals(updateDto.Email, updateResult.Email));
            // check the extended properties were changed
            foreach (var prop in updateResult.PersonExtendedPropertyList)
            {
                var dtoProp = updateDto.PersonExtendedPropertyList.FirstOrDefault(p => p.ExtendedPropertyId == prop.ExtendedPropertyId);
                Validator.IsTrue(ExtendedPropertyDtoValidator.CanParseDtoValue(prop));
                Validator.IsTrue(ExtendedPropertyDtoValidator.ParsedValuesAreEqual(prop, dtoProp));
            }

            // now do a get to check it works
            getResult = await _personRepo.GetPersonById(updateResult.Id);
            PersonDtoValidator.PersonDetailCheck(getResult);

            // check every extended property
            foreach (var prop in getResult.PersonExtendedPropertyList)
            {
                var dtoProp = updateDto.PersonExtendedPropertyList.FirstOrDefault(p => p.ExtendedPropertyId == prop.ExtendedPropertyId);
                Validator.IsTrue(ExtendedPropertyDtoValidator.CanParseDtoValue(prop));
                Validator.IsTrue(ExtendedPropertyDtoValidator.ParsedValuesAreEqual(prop, dtoProp));
            }

            // now update with null values to check this works

            updateDto.PersonExtendedPropertyList.Clear();
            // loop through each extended property and change
            foreach (var prop in result.PersonExtendedPropertyList)
            {
                var newProp = new WriteEntityExtendedPropertyDto()
                {
                    ExtendedPropertyId = prop.ExtendedPropertyId
                };

                newProp.Value = null;

                updateDto.PersonExtendedPropertyList.Add(newProp);
            }

            // update the entity
            updateResult = await _personRepo.UpdatePerson(updateDto);

            // check the result
            PersonDtoValidator.PersonDetailCheck(updateResult);
            Validator.IsTrue(string.Equals(updateDto.GivenName, updateResult.GivenName));
            Validator.IsTrue(string.Equals(updateDto.Surname, updateResult.Surname));
            Validator.IsTrue(string.Equals(updateDto.Email, updateResult.Email));
            // check the extended properties were changed
            foreach (var prop in updateResult.PersonExtendedPropertyList)
            {
                var dtoProp = updateDto.PersonExtendedPropertyList.FirstOrDefault(p => p.ExtendedPropertyId == prop.ExtendedPropertyId);
                Validator.IsTrue(ExtendedPropertyDtoValidator.CanParseDtoValue(prop));
                Validator.IsTrue(ExtendedPropertyDtoValidator.ParsedValuesAreEqual(prop, dtoProp));
            }

            // now do a get to check it works
            getResult = await _personRepo.GetPersonById(updateResult.Id);
            PersonDtoValidator.PersonDetailCheck(getResult);

            // check every extended property
            foreach (var prop in getResult.PersonExtendedPropertyList)
            {
                var dtoProp = updateDto.PersonExtendedPropertyList.FirstOrDefault(p => p.ExtendedPropertyId == prop.ExtendedPropertyId);
                Validator.IsTrue(ExtendedPropertyDtoValidator.CanParseDtoValue(prop));
                Validator.IsTrue(ExtendedPropertyDtoValidator.ParsedValuesAreEqual(prop, dtoProp));
            }
        }
    }
}
