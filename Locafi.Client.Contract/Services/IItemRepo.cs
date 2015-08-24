using System.Threading.Tasks;
using Locafi.Client.Data;
using Locafi.Client.Model.Dto.Items;

namespace Locafi.Client.Contract.Services
{
    public interface IItemRepo
    {
        //Task<ItemSummaryDto> AddItem(AddItemDto item);
        //Task<ItemSummaryDto> GetItemFromTag(string tagNumber);
        //Task<ItemSummaryDto> GetItemById(string id);
        Task<long> GetItemCount();
        Task<ItemDetailDto> GetItemDetail(string itemId);
        Task<ItemDetailDto> CreateItem(AddItemDto item);
        Task<ItemDetailDto> UpdateTag(UpdateItemTagDto updateItemTagDto);
        Task<ItemDetailDto> UpdateItemPlace(UpdateItemPlaceDto updateItemPlaceDto);
        Task<ItemDetailDto> UpdateItem(UpdateItemDto updateItemDto);
        Task DeleteItem(string itemId);
    }
}