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
        public async Task<int> GetItemCount(IRestQuery<ItemSummaryDto> query)
        {
            var path = $"{ItemUri.GetItemCount}{query.AsRestQuery()}";
            var result = await Get<int>(path);
            return result;
        }
        [Obsolete]
        public async Task<IList<ItemSummaryDto>> QueryItems(IRestQuery<ItemSummaryDto> query)
        {
            var result = await QueryItems(query.AsRestQuery());
            return result;
        }

        public async Task<IQueryResult<ItemSummaryDto>> QueryItemsAsync(IRestQuery<ItemSummaryDto> query)
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
            var path = ItemUri.UpdateTag(updateItemTagDto);
            var result = await Post<ItemDetailDto>(updateItemTagDto, path);
            return result;
        }

        public async Task<ItemDetailDto> UpdateItemPlace(UpdateItemPlaceDto updateItemPlaceDto)
        {
            var path = ItemUri.UpdatePlace(updateItemPlaceDto);
            var result = await Post<ItemDetailDto>(updateItemPlaceDto, path);
            return result;
        }

        public async Task<ItemDetailDto> UpdateItem(UpdateItemDto updateItemDto)
        {
            var path = ItemUri.UpdateUri(updateItemDto);
            var result = await Post<ItemDetailDto>(updateItemDto, path);
            return result;
        }

        public async Task<bool> DeleteItem(Guid itemId)
        {
            var path = ItemUri.DeleteItem(itemId);
            return await Delete(path);
        }

        protected async Task<IList<ItemSummaryDto>> QueryItems (string filterString)
        {
            var path = $"{ItemUri.GetItems}{filterString}";
            var result = await Get<List<ItemSummaryDto>>(path);
            return result;
        }

        public async Task<ISearchResult<ItemSummaryDto>> SearchItems(IRestSearch<ItemSummaryDto> search)
        {
            var result = await SearchItems(search.AsRestSearch());
            return result.AsSearchResult(search);
        }

        public async Task<IList<ItemSummaryDto>> SearchItems(SearchCollectionDto searchItemQueryDto)
        {
            var path = ItemUri.SearchItemUri();
            var result = await Post<List<ItemSummaryDto>>(searchItemQueryDto, path);
            return result;
        }

        //public async Task<IList<ItemSummaryDto>> UploadItems(FileUploadDto file)
        //{
        //    var path = ItemUri.UploadItems;
        //    var result = await Post<List<ItemSummaryDto>>(file, path);
        //    return result;
        //}

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
