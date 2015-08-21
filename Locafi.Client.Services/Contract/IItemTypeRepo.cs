using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Data;

namespace Locafi.Client.Services.Contract
{
    public interface IItemTypeRepo
    {
        Task<List<ItemTypeDto>> GetAllItemTypes();
    }
}