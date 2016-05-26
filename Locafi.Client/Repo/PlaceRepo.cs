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
using Locafi.Client.Model.Dto.Places;
using Locafi.Client.Model.Query;
using Locafi.Client.Model.Responses;
using Locafi.Client.Model.Uri;
using Locafi.Client.Model;

namespace Locafi.Client.Repo
{
    public class PlaceRepo : WebRepo, IPlaceRepo
    {

        public PlaceRepo(IAuthorisedHttpTransferConfigService authorisedUnauthorizedConfigService, ISerialiserService serialiser) 
            : base(new SimpleHttpTransferer(), authorisedUnauthorizedConfigService, serialiser, PlaceUri.ServiceName)
        {
        }

        public PlaceRepo(IHttpTransferer transferer, IAuthorisedHttpTransferConfigService authorisedUnauthorizedConfigService, ISerialiserService serialiser)
           : base(transferer, authorisedUnauthorizedConfigService, serialiser, PlaceUri.ServiceName)
        {
        }

        public async Task<PageResult<PlaceSummaryDto>> QueryPlaces(string oDataQueryOptions = null)
        {
            var path = PlaceUri.GetPlaces;

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
            var result = await Get<PageResult<PlaceSummaryDto>>(path);
            return result;
        }

        public async Task<PageResult<PlaceSummaryDto>> QueryPlaces(IRestQuery<PlaceSummaryDto> query)
        {
            return await QueryPlaces(query.AsRestQuery());
        }

        public async Task<IQueryResult<PlaceSummaryDto>> QueryPlacesContinuation(IRestQuery<PlaceSummaryDto> query)
        {
            var results = await QueryPlaces(query.AsRestQuery());
            return results.AsQueryResult(query);
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

        public async Task<bool> Delete(Guid id)
        {
            var path = PlaceUri.DeletePlace(id);
            return await Delete(path);
        }

        public override async Task Handle(HttpResponseMessage responseMessage, string url, string payload)
        {
            throw new PlaceRepoException($"{url} -- {payload} -- " + await responseMessage.Content.ReadAsStringAsync());
        }

        public override Task Handle(IEnumerable<CustomResponseMessage> serverMessages, HttpStatusCode statusCode, string url, string payload)
        {
            throw new PlaceRepoException(serverMessages, statusCode, url, payload);
        }
    }
}
