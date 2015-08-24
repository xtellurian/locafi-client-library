using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.Services;
using Locafi.Client.Data;
using Locafi.Client.Model.Dto.Items;
using Locafi.Client.Model.Extensions;
using Locafi.Client.Services.Odata;
using Newtonsoft.Json;

namespace Locafi.Client.Services.Repo
{
    public class ItemRepo : WebRepo, IItemRepo
    {
        private readonly ISerialiserService _serialiser;

        public ItemRepo(IHttpTransferConfigService transferConfig, ISerialiserService serialiser) : base(transferConfig, serialiser, "Items")
        {
            _serialiser = serialiser;
        }

        public async Task<long> GetItemCount()
        {
            var path = @"GetItems/GetCount";
            var result = await Get<long>(path);
            return result;
        }

        public async Task<ItemDetailDto> GetItemDetail(string itemId)
        {
            var path = $"GetItem/{itemId}";
            var result = await Get<ItemDetailDto>(path);
            return result;
        }

        public async Task<ItemDetailDto> CreateItem(AddItemDto item)
        {
            const string path = @"/CreateItem";
            var result = await Post<ItemDetailDto>(item, path);
            return result;
        }

        public async Task<ItemDetailDto> UpdateTag(UpdateItemTagDto updateItemTagDto)
        {
            var path = updateItemTagDto.UpdateTagUri();
            var result = await Post<ItemDetailDto>(updateItemTagDto, path);
            return result;
        }

        public async Task<ItemDetailDto> UpdateItemPlace(UpdateItemPlaceDto updateItemPlaceDto)
        {
            var path = updateItemPlaceDto.UpdatePlaceUri();
            var result = await Post<ItemDetailDto>(updateItemPlaceDto, path);
            return result;
        }

        public async Task<ItemDetailDto> UpdateItem(UpdateItemDto updateItemDto)
        {
            var path = updateItemDto.UpdateUri();
            var result = await Post<ItemDetailDto>(updateItemDto, path);
            return result;
        }

        public async Task DeleteItem(string itemId)
        {
            await Delete(itemId);
        }

        protected async Task<IList<ItemSummaryDto>> QueryItems (string filterString)
        {
            var path = $"GetItems{filterString}";
            var result = await Get<IList<ItemSummaryDto>>(path);
            return result;
        }

        //public async Task<ItemDto> GetItemFromTag(string tagNumber)
        //{
        //    var path = $"?$filter=TagNumber eq '{tagNumber}'";
        //    var result = await Get(path);
        //    return result.Value.FirstOrDefault();
        //}

        //public async Task<ItemDto> GetItemById(string id)
        //{
        //    var path = $"?$filter=Id eq '{id}'";
        //    var result = await Get(path);
        //    return result.Value.FirstOrDefault();
        //}
    }
}
