// This is a contract file
// Breaking changes to this contract should be accompanied by a version change in the third version number
// ie ... 1.2.42.11 -> 1.2.43.0

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.Items;
using Locafi.Client.Model.Query;
using Locafi.Client.Model.Search;
using Locafi.Client.Model.Dto;
using Locafi.Client.Model.Dto.FileUpload;
using Locafi.Client.Model;

namespace Locafi.Client.Contract.Repo
{
    public interface IItemRepo
    {
        //Task<ItemSummaryDto> AddItem(AddItemDto item);
        //Task<ItemSummaryDto> GetItemFromTag(string tagNumber);
        //Task<ItemSummaryDto> GetItemById(string id);

        Task<ItemDetailDto> GetItemDetail(Guid id);
        Task<ItemDetailDto> CreateItem(AddItemDto item);
        Task<ItemDetailDto> UpdateTag(UpdateItemTagDto updateItemTagDto);
        Task<ItemDetailDto> UpdateItemPlace(UpdateItemPlaceDto updateItemPlaceDto);
        Task<ItemDetailDto> UpdateItem(UpdateItemDto updateItemDto);
        Task<bool> DeleteItem(Guid itemId);
        Task<PageResult<ItemSummaryDto>> QueryItems(string oDataQueryOptions = null);
        Task<PageResult<ItemSummaryDto>> QueryItems(IRestQuery<ItemSummaryDto> query);
        Task<IQueryResult<ItemSummaryDto>> QueryItemsContinuation(IRestQuery<ItemSummaryDto> query);
        Task<IList<ItemSummaryDto>> SearchItems(SearchCollectionDto searchItemQueryDto);
        Task<ISearchResult<ItemSummaryDto>> SearchItems(IRestSearch<ItemSummaryDto> search);
        Task<IList<ItemSummaryDto>> UploadItems(FileUploadDto file);
        Task<bool> ClearItems(ClearItemStateDto clearItemsDto);
        Task<PageResult<ItemStateHistoryDto>> GetItemStateHistory(IRestQuery<ItemStateHistoryDto> query);
        Task<IQueryResult<ItemStateHistoryDto>> GetItemStateHistoryContinuation(IRestQuery<ItemStateHistoryDto> query);
        Task<PageResult<ItemPlaceMovementHistoryDto>> GetItemPlaceHistory(IRestQuery<ItemPlaceMovementHistoryDto> query);
        Task<IQueryResult<ItemPlaceMovementHistoryDto>> GetItemPlaceHistoryContinuation(IRestQuery<ItemPlaceMovementHistoryDto> query);
    }
}