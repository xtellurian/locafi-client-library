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
    public class PersonRepo : WebRepo<ODataCollection<PersonDto>>, IPersonRepo
    {
        public PersonRepo(IHttpTransferConfigService configService, ISerialiserService serialiser) : base(configService, serialiser, "Person")
        {
        }

        public async Task<List<PersonDto>> GetAllPersons()
        {
            var items = await Get();
            return items.Value;
        }

        public async Task<PersonDto> GetPersonById(Guid id)
        {
            var result = await Get("?$filter=Id eq '" + id + "'");
            return result.Value.First();
        }

        public async Task<PersonDto> GetPersonById(string id)
        {
            var result = await Get("?$filter=Id eq '" + id + "'");
            return result.Value.First();
        }
    }
}
