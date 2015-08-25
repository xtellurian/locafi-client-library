using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.Services;
using Locafi.Client.Data;
using Locafi.Client.Services.Odata;

namespace Locafi.Client.Services.Repo
{
    public class PersonRepo : WebRepo, IPersonRepo
    {
        public PersonRepo(IAuthorisedHttpTransferConfigService configService, ISerialiserService serialiser) : base(configService, serialiser, "Person")
        {
        }

        public async Task<IList<PersonDto>> GetAllPersons()
        {
            var items = await Get<IList<PersonDto>>();
            return items;
        }

        public async Task<PersonDto> GetPersonById(Guid id)
        {
            var result = await Get<PersonDto>("?$filter=Id eq '" + id + "'");
            return result;
        }

        public async Task<PersonDto> GetPersonById(string id)
        {
            var result = await Get<PersonDto>("?$filter=Id eq '" + id + "'");
            return result;
        }
    }
}
