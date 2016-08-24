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

namespace Locafi.Client.UnitTests.Tests
{
    [TestClass]
    public class PersonRepoTests
    {
        private IPersonRepo _personRepo;
        private ITemplateRepo _templateRepo;
        private IList<Guid> _personsToCleanup;

        [TestInitialize]
        public void Initialise()
        {
            _personRepo = WebRepoContainer.PersonRepo;
            _templateRepo = WebRepoContainer.TemplateRepo;
            _personsToCleanup = new List<Guid>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            // delete all that were created
            foreach (var Id in _personsToCleanup)
            {
                _personRepo.DeletePerson(Id).Wait();
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
    }
}
