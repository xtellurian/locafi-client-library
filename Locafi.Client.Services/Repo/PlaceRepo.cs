using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.Services;
using Locafi.Client.Data;
using Locafi.Client.Model.Dto.Places;
using Locafi.Client.Services.Odata;

namespace Locafi.Client.Services.Repo
{
    public class PlaceRepo : WebRepo, IPlaceRepo
    {
        private readonly ISerialiserService _serialiser;

        public PlaceRepo(IHttpTransferConfigService configService, ISerialiserService serialiser) : base(configService, serialiser, "Places/")
        {
            _serialiser = serialiser;
        }

        public async Task<IList<PlaceSummaryDto>> GetAllPlaces()
        {
            return await QueryPlaces();
        }


        public async Task<PlaceDetailDto> CreatePlace(AddPlaceDto addPlaceDto)
        {
            var path = @"/CreatePlace";
            var result = await Post<PlaceDetailDto>(addPlaceDto, path);
            return result;
        }
       

        //public async Task<PlaceDto> GetPlaceById(Guid id)
        //{
        //    var result = await base.Get("?$filter=Id eq '" + id + "'");
        //    return result.Value.FirstOrDefault();
        //}

        //public async Task<PlaceDto> GetPlaceById(string id)
        //{
        //    var result = await base.Get("?$filter=Id eq '" + id + "'");
        //    return result.Value.FirstOrDefault();
        //}

        protected async Task<IList<PlaceSummaryDto>> QueryPlaces(string queryString = "")
        {
            var path = $"GetPlaces{queryString}";
            var result = await Get<IList<PlaceSummaryDto>>(path);
            return result;
        }
    }
}
