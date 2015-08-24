using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Data;

namespace Locafi.Client.Contract.Services
{
    public interface IPlaceRepo
    {
        Task<IList<PlaceDto>> GetAllPlaces();
        Task<PlaceDto> AddNewPlace(PlaceDto place);
    }
}