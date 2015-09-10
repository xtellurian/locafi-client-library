// This is a contract file
// Breaking changes to this contract should be accompanied by a version change in the third version number
// ie ... 1.2.42.11 -> 1.2.43.0

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.Places;
using Locafi.Client.Model.Query;

namespace Locafi.Client.Contract.Repo
{
    public interface IPlaceRepo
    {
        Task<IList<PlaceSummaryDto>> GetAllPlaces();
        Task<PlaceDetailDto> CreatePlace(AddPlaceDto addPlaceDto);
        Task<IList<PlaceSummaryDto>> QueryPlaces(IRestQuery<PlaceSummaryDto> query);
        Task Delete(Guid placeId);
        Task<PlaceDetailDto> GetPlaceById(Guid id);
    }
}