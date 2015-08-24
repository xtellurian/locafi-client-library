using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Data;
using Locafi.Client.Model.Dto.Places;

namespace Locafi.Client.Contract.Services
{
    public interface IPlaceRepo
    {
        Task<IList<PlaceSummaryDto>> GetAllPlaces();
       // Task<PlaceDto> AddNewPlace(PlaceDto place);
        Task<PlaceDetailDto> CreatePlace(AddPlaceDto addPlaceDto);
    }
}