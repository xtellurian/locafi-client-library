using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Data;

namespace Locafi.Client.Services.Contract
{
    public interface IPersonRepo
    {
        Task<List<PersonDto>> GetAllPersons();
    }
}