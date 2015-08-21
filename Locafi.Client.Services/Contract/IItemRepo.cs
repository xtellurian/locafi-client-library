using System.Threading.Tasks;
using Locafi.Client.Data;

namespace Locafi.Client.Services.Contract
{
    public interface IItemRepo
    {
        Task<ItemDto> AddItem(AddItemDto item);
        Task<ItemDto> GetItemFromTag(string tagNumber);
        Task<ItemDto> GetItemById(string id);
    }
}