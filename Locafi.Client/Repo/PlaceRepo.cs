using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Model.Dto.Places;
using Locafi.Client.Model.Query;
using Locafi.Client.Services;

namespace Locafi.Client.Repo
{
    public class PlaceRepo : WebRepo, IPlaceRepo
    {
        private readonly ISerialiserService _serialiser;

        public PlaceRepo(IAuthorisedHttpTransferConfigService authorisedUnauthorizedConfigService, ISerialiserService serialiser) 
            : base(authorisedUnauthorizedConfigService, serialiser, "Places")
        {
            _serialiser = serialiser;
        }

        public async Task<IList<PlaceSummaryDto>> GetAllPlaces()
        {
            var path = "GetPlaces";
            var result = await Get<IList<PlaceSummaryDto>>(path);
            return result;
        }


        public async Task<PlaceDetailDto> CreatePlace(AddPlaceDto addPlaceDto)
        {
            var path = "CreatePlace";
            var result = await Post<PlaceDetailDto>(addPlaceDto, path);
            return result;
        }

        public async Task<PlaceDetailDto> GetPlaceById(Guid id)
        {
            var path = $"GetPlace/{id}";
            var result = await Get<PlaceDetailDto>(path);
            return result;
        }

        public async Task<IList<PlaceSummaryDto>> QueryPlaces(IRestQuery<PlaceSummaryDto> query)
        {
            return await QueryPlaces(query.AsRestQuery());
        }

        public async Task Delete(Guid id)
        {
            var path = $"DeletePlace/{id}";
            await Delete(path);
        }

        protected async Task<IList<PlaceSummaryDto>> QueryPlaces(string queryString = null)
        {
            var path = $"GetPlaces{queryString}";
            var result = await Get<IList<PlaceSummaryDto>>(path);
            return result;
        }
    }
}
