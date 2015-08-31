using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Contract.Services;
using Locafi.Client.Data;
using Locafi.Client.Model.Dto.Persons;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Locafi.Client.UnitTests.Tests
{
    [TestClass]
    public class PersonRepoTests
    {
        private IPersonRepo _personRepo;
        [TestInitialize]
        public void Initialise()
        {
            _personRepo = WebRepoContainer.PersonRepo;
        }

        [TestMethod]
        public async Task Person_GetAll()
        {
            var persons = await _personRepo.GetAllPersons();
            Assert.IsNotNull(persons);
            Assert.IsInstanceOfType(persons, typeof(IEnumerable<PersonSummaryDto>));
        }

        public async Task Person_Create()
        {
            var person = new AddPersonDto
            {

            };
            
        }
    }
}
