using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.ErrorHandlers;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Exceptions;
using Locafi.Client.Model.Dto.Places;
using Locafi.Client.Model.Query;
using Locafi.Client.Model.Responses;
using Locafi.Client.Model.Uri;

namespace Locafi.Client.Repo
{
    public class PlaceRepo : WebRepo, IPlaceRepo, IWebRepoErrorHandler
    {
        private readonly ISerialiserService _serialiser;

        public PlaceRepo(IAuthorisedHttpTransferConfigService authorisedUnauthorizedConfigService, ISerialiserService serialiser) 
            : base(authorisedUnauthorizedConfigService, serialiser, PlaceUri.ServiceName)
        {
            _serialiser = serialiser;
        }

        public async Task<IList<PlaceSummaryDto>> GetAllPlaces()
        {
            var path = PlaceUri.GetPlaces;
            var result = await Get<IList<PlaceSummaryDto>>(path);
            return result;
        }


        public async Task<PlaceDetailDto> CreatePlace(AddPlaceDto addPlaceDto)
        {
            var path = PlaceUri.CreatePlace;
            var result = await Post<PlaceDetailDto>(addPlaceDto, path);
            return result;
        }

        public async Task<PlaceDetailDto> GetPlaceById(Guid id)
        {
            var path = PlaceUri.GetPlace(id);
            var result = await Get<PlaceDetailDto>(path);
            return result;
        }

        [Obsolete]
        public async Task<IList<PlaceSummaryDto>> QueryPlaces(IRestQuery<PlaceSummaryDto> query)
        {
            return await QueryPlaces(query.AsRestQuery());
        }

        public async Task<IQueryResult<PlaceSummaryDto>> QueryPlacesAsync(IRestQuery<PlaceSummaryDto> query)
        {
            var results = await QueryPlaces(query.AsRestQuery());
            IRestQuery<PlaceSummaryDto> continuationQuery = null;
            if (results.Count == query.Take)
            {
                // there may me more results
                query.Skip = query.Skip + query.Take; // go to next batch of entites
                continuationQuery = query;
            }
            return new QueryResult<PlaceSummaryDto>(results, continuationQuery);
        }

        public async Task<bool> Delete(Guid id)
        {
            var path = PlaceUri.DeletePlace(id);
            return await Delete(path);
        }

        protected async Task<IList<PlaceSummaryDto>> QueryPlaces(string queryString = null)
        {
            var path = $"{PlaceUri.GetPlaces}{queryString}";
            var result = await Get<IList<PlaceSummaryDto>>(path);
            return result;
        }

        public override async Task Handle(HttpResponseMessage responseMessage)
        {
            throw new PlaceRepoException(await responseMessage.Content.ReadAsStringAsync());
        }

        public override Task Handle(IEnumerable<CustomResponseMessage> serverMessages, HttpStatusCode statusCode)
        {
            throw new PlaceRepoException(serverMessages, statusCode);
        }
    }
}
