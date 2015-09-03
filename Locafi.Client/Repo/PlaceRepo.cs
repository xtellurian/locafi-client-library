﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Model.Dto.Places;
using Locafi.Client.Model.Query;
using Locafi.Client.Model.Uri;

namespace Locafi.Client.Repo
{
    public class PlaceRepo : WebRepo, IPlaceRepo
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

        public async Task<IList<PlaceSummaryDto>> QueryPlaces(IRestQuery<PlaceSummaryDto> query)
        {
            return await QueryPlaces(query.AsRestQuery());
        }

        public async Task Delete(Guid id)
        {
            var path = PlaceUri.DeletePlace(id);
            await Delete(path);
        }

        protected async Task<IList<PlaceSummaryDto>> QueryPlaces(string queryString = null)
        {
            var path = $"{PlaceUri.GetPlaces}{queryString}";
            var result = await Get<IList<PlaceSummaryDto>>(path);
            return result;
        }
    }
}
