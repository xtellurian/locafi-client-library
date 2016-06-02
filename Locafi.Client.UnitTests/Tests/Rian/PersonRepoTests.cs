﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Model.Dto.Persons;
using Locafi.Client.Model.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Locafi.Client.Model.Dto.Tags;
using Locafi.Client.Exceptions;

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
            var persons = await _personRepo.QueryPersons();
            Assert.IsNotNull(persons, "persons != null");
            Assert.IsInstanceOfType(persons, typeof(IEnumerable<PersonSummaryDto>));
        }

        [TestMethod]
        public async Task Person_Create()
        {
            var addPerson = await RandomAddPerson();
            
            var detail = await _personRepo.CreatePerson(addPerson);
            Assert.IsNotNull(detail, "detail != null");
            _toCleanup.Add(detail.Id);
            Assert.IsInstanceOfType(detail,typeof(PersonDetailDto));
            Assert.IsTrue(string.Equals(addPerson.Email,detail.Email));
            Assert.IsTrue(string.Equals(addPerson.GivenName, detail.GivenName));
            Assert.IsTrue(string.Equals(addPerson.Surname, detail.Surname));
            Assert.IsTrue(string.Equals(addPerson.PersonTagList[0].TagNumber, detail.TagNumber));
        }

        [TestMethod]
        public async Task Person_Delete()
        {
            var addPerson = await RandomAddPerson();
            var detail = await _personRepo.CreatePerson(addPerson);
            Assert.IsNotNull(detail, "detail != null"); // we got somethign back
            detail = await _personRepo.GetPersonById(detail.Id);
            Assert.IsNotNull(detail, "detail != null second"); // check we can lookup by Id
            await _personRepo.DeletePerson(detail.Id);
            try {
                detail = await _personRepo.GetPersonById(detail.Id);
            }catch(PersonException ex)
            {
                Assert.AreEqual(ex.ServerMessages.Count, 0);
            }
        }

      

        #region PrivateMethods

        private async Task<AddPersonDto> RandomAddPerson()
        {
            var ran = new Random();
            var templates = await _templateRepo.GetTemplatesForType(TemplateFor.Person);
            var template = templates.Items.ElementAt(ran.Next(templates.Items.Count() - 1));
            var email = $"{Guid.NewGuid().ToString().Substring(0,16)}@FakeDomain.com";

            return new AddPersonDto
            {
                TemplateId = template.Id,
                Email = email,
                GivenName = Guid.NewGuid().ToString(),
                Surname = Guid.NewGuid().ToString(),
                PersonTagList = new List<WriteTagDto>() { new WriteTagDto() {TagNumber = Guid.NewGuid().ToString(), TagType = TagType.PassiveRfid } }
            };
        }

#endregion
    }
}
