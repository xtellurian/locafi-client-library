using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.Errors;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Exceptions;
using Locafi.Client.Model.Dto.Items;
using Locafi.Client.Model.Query;
using Locafi.Client.Model.Responses;
using Locafi.Client.Model.Uri;

namespace Locafi.Client.Repo
{
    public class ItemRepo : WebRepo, IItemRepo
    {
        private readonly ISerialiserService _serialiser;

        public ItemRepo(IAuthorisedHttpTransferConfigService transferAuthorisedUnauthorizedConfig, ISerialiserService serialiser) 
            : base(transferAuthorisedUnauthorizedConfig, serialiser, ItemUri.ServiceName)
        {
            _serialiser = serialiser;
        }

        public async Task<long> GetItemCount()
        {
            var path = ItemUri.GetCount;
            var result = await Get<long>(path);
            return result;
        }

        public async Task<IList<ItemSummaryDto>> QueryItems(IRestQuery<ItemSummaryDto> query)
        {
            var result = await QueryItems(query.AsRestQuery());
            return result;
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
            var result = await Get<IList<ItemSummaryDto>>(path);
            return result;
        }

        public async override Task Handle(HttpResponseMessage responseMessage)
        {
            throw new ItemException(await responseMessage.Content.ReadAsStringAsync());
        }

        public override Task Handle(IEnumerable<CustomResponseMessage> serverMessages, HttpStatusCode statusCode)
        {
            throw new ItemException(serverMessages, statusCode);
        }
    }
}
