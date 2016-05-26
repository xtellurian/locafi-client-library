using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.ErrorHandlers;
using Locafi.Client.Contract.Http;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Exceptions;
using Locafi.Client.Model.Dto.Persons;
using Locafi.Client.Model.Query;
using Locafi.Client.Model.Responses;
using Locafi.Client.Model.Uri;
using Locafi.Client.Model;

namespace Locafi.Client.Repo
{
    public class PersonRepo : WebRepo, IPersonRepo, IWebRepoErrorHandler
    {
        public PersonRepo(IAuthorisedHttpTransferConfigService configService, ISerialiserService serialiser) 
            : base(new SimpleHttpTransferer(), configService, serialiser, PersonUri.ServiceName)
        {
        }

        public PersonRepo(IHttpTransferer transferer, IAuthorisedHttpTransferConfigService authorisedConfigService, ISerialiserService serialiser)
           : base(transferer, authorisedConfigService, serialiser, PersonUri.ServiceName)
        {
        }

        public async Task<PageResult<PersonSummaryDto>> QueryPersons(string oDataQueryOptions = null)
        {
            var path = PersonUri.GetPersons;

            // add the query options if required
            if (!string.IsNullOrEmpty(oDataQueryOptions))
            {
                if (oDataQueryOptions[0] != '?')
                    path += "?";

                path += oDataQueryOptions;
            }

            // make sure the query asks to return the item count
            if (!path.Contains("$count"))
            {
                if (path.Contains("?"))
                    path += "&$count=true";
                else
                    path += "?$count=true";
            }

            // run query
            var items = await Get<PageResult<PersonSummaryDto>>(path);
            return items;
        }

        public async Task<PageResult<PersonSummaryDto>> QueryPersons(IRestQuery<PersonSummaryDto> query)
        {
            return await QueryPersons(query.AsRestQuery());
        }

        public async Task<IQueryResult<PersonSummaryDto>> QueryPersonsContinuation(IRestQuery<PersonSummaryDto> query)
        {
            var result = await QueryPersons(query.AsRestQuery());
            return result.AsQueryResult(query);
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

        public async override Task Handle(HttpResponseMessage responseMessage, string url, string payload)
        {
            throw new PersonException($"{url} -- {payload} -- " + await responseMessage.Content.ReadAsStringAsync());
        }

        public override Task Handle(IEnumerable<CustomResponseMessage> serverMessages, HttpStatusCode statusCode, string url, string payload)
        {
            throw new PersonException(serverMessages, statusCode, url, payload);
        }
    }
}
