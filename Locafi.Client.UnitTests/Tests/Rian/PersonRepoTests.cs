using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Model.Dto.Persons;
using Locafi.Client.Model.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Locafi.Client.UnitTests.Tests.Rian
{
    [TestClass]
    public class PersonRepoTests
    {
        private IPersonRepo _personRepo;
        private ITemplateRepo _templateRepo;
        private IList<Guid> _toCleanup;

        [TestInitialize]
        public void Initialise()
        {
            _personRepo = WebRepoContainer.PersonRepo;
            _templateRepo = WebRepoContainer.TemplateRepo;
            _toCleanup = new List<Guid>();
        }

        [TestMethod]
        public async Task Person_GetAll()
        {
            var persons = await _personRepo.GetAllPersons();
            Assert.IsNotNull(persons, "persons != null");
            Assert.IsInstanceOfType(persons, typeof(IEnumerable<PersonSummaryDto>));
        }
   //     [TestMethod]
        public async Task Person_Create()
        {
            var addPerson = await RandomAddPerson();
            
            var detail = await _personRepo.CreatePerson(addPerson);
            Assert.IsNotNull(detail, "detail != null");
            _toCleanup.Add(detail.Id);
            Assert.IsInstanceOfType(detail,typeof(PersonDetailDto));
            Assert.IsTrue(string.Equals(addPerson.EmailAddress,detail.EmailAddress));
            Assert.IsTrue(string.Equals(addPerson.GivenName, detail.GivenName));
            Assert.IsTrue(string.Equals(addPerson.Surname, detail.Surname));
            Assert.IsTrue(string.Equals(addPerson.TagNumber, detail.TagNumber));
        }
//       [TestMethod]
        public async Task Person_Delete()
        {
            var addPerson = await RandomAddPerson();
            var detail = await _personRepo.CreatePerson(addPerson);
            Assert.IsNotNull(detail, "detail != null"); // we got somethign back
            detail = await _personRepo.GetPersonById(detail.Id);
            Assert.IsNotNull(detail, "detail != null second"); // check we can lookup by Id
            await _personRepo.DeletePerson(detail.Id);
            detail = await _personRepo.GetPersonById(detail.Id);
            Assert.IsNull(detail, "detail != null third");
        }

      

        #region PrivateMethods

        private async Task<AddPersonDto> RandomAddPerson()
        {
            var ran = new Random();
            var templates = await _templateRepo.GetTemplatesForType(TemplateFor.Person);
            var template = templates[ran.Next(templates.Count - 1)];
            var email = $"{Guid.NewGuid()}@FakeDomain.com";

            return new AddPersonDto
            {
                TemplateId = template.Id,
                EmailAddress = email,
                GivenName = Guid.NewGuid().ToString(),
                Surname = Guid.NewGuid().ToString(),
                TagNumber = Guid.NewGuid().ToString(),
                TagType = "", // not used?
            };
        }

#endregion
    }
}
