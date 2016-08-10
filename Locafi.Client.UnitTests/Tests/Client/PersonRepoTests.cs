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

        [TestMethod]
        public async Task Person_GetAll()
        {
            // create person
            var addPerson = await RandomAddPerson();
            var detail = await _personRepo.CreatePerson(addPerson);
            _personsToCleanup.Add(detail.Id);

            // query persons
            var persons = await _personRepo.QueryPersons();

            // check result
            Validator.IsNotNull(persons, "persons != null");
            Validator.IsInstanceOfType(persons, typeof(PageResult<PersonSummaryDto>));
            Validator.IsTrue(persons.Items.Count() > 0);
            Validator.IsTrue(persons.Contains(detail));
        }

        [TestMethod]
        public async Task Person_Create()
        {
            // create person
            var addPerson = await RandomAddPerson();
            var detail = await _personRepo.CreatePerson(addPerson);
            _personsToCleanup.Add(detail.Id);

            // check result
            PersonDtoValidator.PersonDetailCheck(detail);
            Validator.IsInstanceOfType(detail,typeof(PersonDetailDto));
            Validator.IsTrue(string.Equals(addPerson.Email,detail.Email));
            Validator.IsTrue(string.Equals(addPerson.GivenName, detail.GivenName));
            Validator.IsTrue(string.Equals(addPerson.Surname, detail.Surname));
            Validator.IsTrue(string.Equals(addPerson.PersonTagList[0].TagNumber.ToUpper(), detail.TagNumber));
        }

        [TestMethod]
        public async Task Person_Get()
        {
            // create person
            var addPerson = await RandomAddPerson();
            var detail = await _personRepo.CreatePerson(addPerson);
            _personsToCleanup.Add(detail.Id);

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
        public async Task Person_Delete()
        {
            // create person
            var addPerson = await RandomAddPerson();
            var detail = await _personRepo.CreatePerson(addPerson);
            _personsToCleanup.Add(detail.Id);

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

            // verify
            var query = QueryBuilder<PersonSummaryDto>.NewQuery(p => p.Id, detail.Id, ComparisonOperator.Equals).Build();
            var queryResult = await _personRepo.QueryPersons(query); // get the person again
            Validator.IsFalse(queryResult.Any(p => p.Id == detail.Id)); // check our person is actually gone
        }

        // TODO: Test updates

        #region PrivateMethods

        private async Task<AddPersonDto> RandomAddPerson()
        {
            var ran = new Random();
            var templates = await _templateRepo.GetTemplatesForType(TemplateFor.Person);
            var template = await _templateRepo.GetById(templates.Items.ElementAt(ran.Next(templates.Items.Count() - 1)).Id);
            var email = $"{Guid.NewGuid().ToString().Substring(0,16)}@FakeDomain.com";
            var name = "Random - " + template.Name + " " + ran.Next().ToString();
            var surname = name + " - Surname";

            return new AddPersonDto(template)
            {
                TemplateId = template.Id,
                Email = email,
                GivenName = name,
                Surname = surname,
                PersonTagList = new List<WriteTagDto>()
                {
                    new WriteTagDto()
                    {
                        TagNumber = Guid.NewGuid().ToString(),
                        TagType = TagType.PassiveRfid
                    }
                }
            };
        }

#endregion
    }
}
