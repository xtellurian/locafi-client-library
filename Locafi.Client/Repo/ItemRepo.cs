using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.Http;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Exceptions;
using Locafi.Client.Model.Dto.Items;
using Locafi.Client.Model.Query;
using Locafi.Client.Model.RelativeUri;
using Locafi.Client.Model.Responses;
using Locafi.Client.Model.Uri;
using Locafi.Client.Model.Dto;
using Locafi.Client.Model.Dto.FileUpload;
using Locafi.Client.Model.Search;
using Locafi.Client.Model;

namespace Locafi.Client.Repo
{
    public class ItemRepo : WebRepo, IItemRepo
    {
        private readonly ISerialiserService _serialiser;

        public ItemRepo(IAuthorisedHttpTransferConfigService transferAuthorisedConfig, ISerialiserService serialiser) 
            : base(new SimpleHttpTransferer(), transferAuthorisedConfig, serialiser, ItemUri.ServiceName)
        {
            _serialiser = serialiser;
        }

        public ItemRepo(IHttpTransferer transferer, IAuthorisedHttpTransferConfigService authorisedConfigService, ISerialiserService serialiser)
           : base(transferer, authorisedConfigService, serialiser, ItemUri.ServiceName)
        {
        }

        public async Task<PageResult<ItemSummaryDto>> QueryItems(string oDataQueryOptions = null)
        {
            var path = ItemUri.GetItems;

            // add the query options if required
            if (!string.IsNullOrEmpty(oDataQueryOptions))
            {
                if (oDataQueryOptions[0] != '?')
                    path += "?";

                path += oDataQueryOptions;
            }

            // make sure the query asks to return the item count
            if (!path.Contains("$count"))
            {
                if (path.Contains("?"))
                    path += "&$count=true";
                else
                    path += "?$count=true";
            }

            // run query
            var result = await Get<PageResult<ItemSummaryDto>>(path);
            return result;
        }

        public async Task<PageResult<ItemSummaryDto>> QueryItems(IRestQuery<ItemSummaryDto> query)
        {
            var result = await QueryItems(query.AsRestQuery());
            return result;
        }

        public async Task<IQueryResult<ItemSummaryDto>> QueryItemsContinuation(IRestQuery<ItemSummaryDto> query)
        {
            var result = await QueryItems(query.AsRestQuery());
            return result.AsQueryResult(query);
        }

        public async Task<ItemDetailDto> GetItemDetail(Guid id)
        {
            var path = ItemUri.GetItem(id);
            var result = await Get<ItemDetailDto>(path);
            return result;
        }

        public async Task<ItemDetailDto> CreateItem(AddItemDto item)
        {
            var path = ItemUri.CreateItem;
            var result = await Post<ItemDetailDto>(item, path);
            return result;
        }

        public async Task<ItemDetailDto> UpdateTag(UpdateItemTagDto updateItemTagDto)
        {
            var path = ItemUri.UpdateTag;
            var result = await Post<ItemDetailDto>(updateItemTagDto, path);
            return result;
        }

        public async Task<ItemDetailDto> UpdateItemPlace(UpdateItemPlaceDto updateItemPlaceDto)
        {
            var path = ItemUri.UpdatePlace;
            var result = await Post<ItemDetailDto>(updateItemPlaceDto, path);
            return result;
        }

        public async Task<ItemDetailDto> UpdateItem(UpdateItemDto updateItemDto)
        {
            var path = ItemUri.UpdateItem;
            var result = await Post<ItemDetailDto>(updateItemDto, path);
            return result;
        }

        public async Task<bool> DeleteItem(Guid itemId)
        {
            var path = ItemUri.DeleteItem(itemId);
            return await Delete(path);
        }

        public async Task<ISearchResult<ItemSummaryDto>> SearchItems(IRestSearch<ItemSummaryDto> search)
        {
            var result = await SearchItems(search.AsRestSearch());
            return result.AsSearchResult(search);
        }

        public async Task<IList<ItemSummaryDto>> SearchItems(SearchCollectionDto searchItemQueryDto)
        {
            var path = ItemUri.SearchItems;
            var result = await Post<List<ItemSummaryDto>>(searchItemQueryDto, path);
            return result;
        }

        public async Task<IList<ItemSummaryDto>> UploadItems(FileUploadDto file)
        {
            var path = ItemUri.UploadItems;
            var result = await Post<List<ItemSummaryDto>>(file, path);
            return result;
        }

        public async Task<bool> ClearItems(ClearItemStateDto clearItemsDto)
        {
            var path = ItemUri.ClearItems;
            var result = await Post(clearItemsDto, path);
            return result;
        }

        private async Task<PageResult<ItemStateHistoryDto>> GetItemStateHistory(string oDataQueryOptions = null)
        {
            var path = ItemUri.GetItemStateHistory;

            // add the query options if required
            if (!string.IsNullOrEmpty(oDataQueryOptions))
            {
                if (oDataQueryOptions[0] != '?')
                    path += "?";

                path += oDataQueryOptions;
            }

            // make sure the query asks to return the item count
            if (!path.Contains("$count"))
            {
                if (path.Contains("?"))
                    path += "&$count=true";
                else
                    path += "?$count=true";
            }

            // run query
            var result = await Get<PageResult<ItemStateHistoryDto>>(path);
            return result;
        }

        public async Task<PageResult<ItemStateHistoryDto>> GetItemStateHistory(IRestQuery<ItemStateHistoryDto> query)
        {
            return await GetItemStateHistory(query.AsRestQuery());
        }

        public async Task<IQueryResult<ItemStateHistoryDto>> GetItemStateHistoryContinuation(IRestQuery<ItemStateHistoryDto> query)
        {
            var result = await GetItemStateHistory(query.AsRestQuery());
            return result.AsQueryResult(query);
        }

        private async Task<PageResult<ItemPlaceMovementHistoryDto>> GetItemPlaceHistory(string oDataQueryOptions = null)
        {
            var path = ItemUri.GetItemPlaceHistory;

            // add the query options if required
            if (!string.IsNullOrEmpty(oDataQueryOptions))
            {
                if (oDataQueryOptions[0] != '?')
                    path += "?";

                path += oDataQueryOptions;
            }

            // make sure the query asks to return the item count
            if (!path.Contains("$count"))
            {
                if (path.Contains("?"))
                    path += "&$count=true";
                else
                    path += "?$count=true";
            }

            // run query
            var result = await Get<PageResult<ItemPlaceMovementHistoryDto>>(path);
            return result;
        }

        public async Task<PageResult<ItemPlaceMovementHistoryDto>> GetItemPlaceHistory(IRestQuery<ItemPlaceMovementHistoryDto> query)
        {
            return await GetItemPlaceHistory(query.AsRestQuery());
        }

        public async Task<IQueryResult<ItemPlaceMovementHistoryDto>> GetItemPlaceHistoryContinuation(IRestQuery<ItemPlaceMovementHistoryDto> query)
        {
            var result = await GetItemPlaceHistory(query.AsRestQuery());
            return result.AsQueryResult(query);
        }

        public override async Task Handle(HttpResponseMessage responseMessage, string url, string payload)
        {
            throw new ItemRepoException($"{url} -- {payload} -- " + await responseMessage.Content.ReadAsStringAsync());
        }

        public override Task Handle(IEnumerable<CustomResponseMessage> serverMessages, HttpStatusCode statusCode, string url, string payload)
        {
            throw new ItemRepoException(serverMessages, statusCode, url, payload);
        }
    }
}
