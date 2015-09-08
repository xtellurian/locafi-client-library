﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Locafi.Client.Contract.Errors;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Exceptions;
using Locafi.Client.Model.ConfigurationContract;
using Locafi.Client.Model.Dto.Persons;
using Locafi.Client.Model.Responses;
using Locafi.Client.Model.Uri;

namespace Locafi.Client.Repo
{
    public class PersonRepo : WebRepo, IPersonRepo, IWebRepoErrorHandler
    {
        public PersonRepo(IAuthorisedHttpTransferConfigService unauthorizedConfigService, ISerialiserService serialiser) 
            : base(unauthorizedConfigService, serialiser, PersonUri.ServiceName)
        {
        }

        public async Task<IList<PersonSummaryDto>> GetAllPersons()
        {
            var path = PersonUri.GetPersons;
            var items = await Get<IList<PersonSummaryDto>>(path);
            return items;
        }

        public async Task<PersonDetailDto> GetPersonById(Guid id)
        {
            var path = PersonUri.GetPerson(id);
            var result = await Get<PersonDetailDto>(path);
            return result;
        }

        public async Task<PersonDetailDto> CreatePerson(AddPersonDto addPerson)
        {
            var path = PersonUri.CreatePerson;
            var result = await Post<PersonDetailDto>(addPerson, path);
            return result;
        }

        public async Task DeletePerson(Guid id)
        {
            var path = PersonUri.DeletePerson(id);
            await Delete(path);
        }

        protected async Task<IList<PersonSummaryDto>> QueryPersons(string queryString)
        {
            var result = await Get<IList<PersonSummaryDto>>(queryString);
            return result;
        }

        public async override Task Handle(HttpResponseMessage responseMessage)
        {
            throw new PersonException(await responseMessage.Content.ReadAsStringAsync());
        }

        public override Task Handle(IEnumerable<CustomResponseMessage> serverMessages, HttpStatusCode statusCode)
        {
            throw new PersonException(serverMessages, statusCode);
        }
    }
}
