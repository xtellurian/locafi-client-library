using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.Services;
using Locafi.Client.Data;
using Locafi.Client.Model.Dto.Persons;
using Locafi.Client.Services.Odata;

namespace Locafi.Client.Services.Repo
{
    public class PersonRepo : WebRepo, IPersonRepo
    {
        public PersonRepo(IAuthorisedHttpTransferConfigService unauthorizedConfigService, ISerialiserService serialiser) : base(unauthorizedConfigService, serialiser, "Person")
        {
        }

        public async Task<IList<PersonSummaryDto>> GetAllPersons()
        {
            var items = await Get<IList<PersonSummaryDto>>();
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
