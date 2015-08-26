using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Data;
using Locafi.Client.Model.Dto.Items;
using Locafi.Client.Model.Query;

namespace Locafi.Client.Contract.Services
{
    public interface IItemRepo
    {
        //Task<ItemSummaryDto> AddItem(AddItemDto item);
        //Task<ItemSummaryDto> GetItemFromTag(string tagNumber);
        //Task<ItemSummaryDto> GetItemById(string id);
        Task<long> GetItemCount();
        Task<ItemDetailDto> GetItemDetail(Guid id);
        Task<ItemDetailDto> CreateItem(AddItemDto item);
        Task<ItemDetailDto> UpdateTag(UpdateItemTagDto updateItemTagDto);
        Task<ItemDetailDto> UpdateItemPlace(UpdateItemPlaceDto updateItemPlaceDto);
        Task<ItemDetailDto> UpdateItem(UpdateItemDto updateItemDto);
        Task DeleteItem(Guid itemId);
        Task<IList<ItemSummaryDto>> QueryItems(ISimpleRestQuery<ItemSummaryDto> query);
    }
}