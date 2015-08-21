using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Data;
using Locafi.Client.Services.Contract;
using Locafi.Client.Services.Odata;

namespace Locafi.Client.Services.Repo
{
    public class PlaceRepo : WebRepoBase<ODataCollection<PlaceDto>>, IPlaceRepo
    {
        public PlaceRepo(IHttpTransferConfigService configService, ISerialiserService serialiser) : base(configService, serialiser, "Places/")
        {
        }

        public async Task<IList<PlaceDto>> GetAllPlaces()
        {
            var result = await base.Get();
            return result.Value;
        }

        public async Task<PlaceDto> GetPlaceById(Guid id)
        {
            var result = await base.Get("?$filter=Id eq '" + id + "'");
            return result.Value.FirstOrDefault();
        }

        public async Task<PlaceDto> GetPlaceById(string id)
        {
            var result = await base.Get("?$filter=Id eq '" + id + "'");
            return result.Value.FirstOrDefault();
        }
    }
}
