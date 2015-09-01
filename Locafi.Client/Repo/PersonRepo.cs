using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.Services;
using Locafi.Client.Model.Dto.Persons;
using Locafi.Client.Services;

namespace Locafi.Client.Repo
{
    public class PersonRepo : WebRepo, IPersonRepo
    {
        public PersonRepo(IAuthorisedHttpTransferConfigService unauthorizedConfigService, ISerialiserService serialiser) 
            : base(unauthorizedConfigService, serialiser, "Persons")
        {
        }

        public async Task<IList<PersonSummaryDto>> GetAllPersons()
        {
            var path = "GetPersons";
            var items = await Get<IList<PersonSummaryDto>>(path);
            return items;
        }

        public async Task<PersonDetailDto> GetPersonById(Guid id)
        {
            var path = $"GetPerson/{id}";
            var result = await Get<PersonDetailDto>(path);
            return result;
        }

        protected async Task<IList<PersonSummaryDto>> QueryPersons(string queryString)
        {
            var result = await Get<IList<PersonSummaryDto>>(queryString);
            return result;
        }
    }
}
