// This is a contract file
// Breaking changes to this contract should be accompanied by a version change in the third version number
// ie ... 1.2.42.11 -> 1.2.43.0

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.Items;
using Locafi.Client.Model.Query;

namespace Locafi.Client.Contract.Repo
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
        Task<bool> DeleteItem(Guid itemId);
        Task<IList<ItemSummaryDto>> QueryItems(IRestQuery<ItemSummaryDto> query);
    }
}