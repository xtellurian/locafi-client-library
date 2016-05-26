// This is a contract file
// Breaking changes to this contract should be accompanied by a version change in the third version number
// ie ... 1.2.42.11 -> 1.2.43.0

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.Places;
using Locafi.Client.Model.Query;
using Locafi.Client.Model;

namespace Locafi.Client.Contract.Repo
{
    public interface IPlaceRepo
    {
        Task<PageResult<PlaceSummaryDto>> QueryPlaces(string oDataQueryOptions = null);
        Task<PageResult<PlaceSummaryDto>> QueryPlaces(IRestQuery<PlaceSummaryDto> query);
        Task<IQueryResult<PlaceSummaryDto>> QueryPlacesContinuation(IRestQuery<PlaceSummaryDto> query);
        Task<PlaceDetailDto> CreatePlace(AddPlaceDto addPlaceDto);
        Task<bool> Delete(Guid placeId);
        Task<PlaceDetailDto> GetPlaceById(Guid id);
    }
}