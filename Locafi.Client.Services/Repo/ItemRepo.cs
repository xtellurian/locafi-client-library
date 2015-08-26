﻿using System;
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
using Locafi.Client.Model.Query;
using Locafi.Client.Services.Odata;
using Newtonsoft.Json;

namespace Locafi.Client.Services.Repo
{
    public class ItemRepo : WebRepo, IItemRepo
    {
        private readonly ISerialiserService _serialiser;

        public ItemRepo(IAuthorisedHttpTransferConfigService transferAuthorisedConfig, ISerialiserService serialiser) : base(transferAuthorisedConfig, serialiser, "Items")
        {
            _serialiser = serialiser;
        }

        public async Task<long> GetItemCount()
        {
            var path = @"GetItems/GetCount";
            var result = await Get<long>(path);
            return result;
        }

        public async Task<IList<ItemSummaryDto>> QueryItems(ISimpleRestQuery<ItemSummaryDto> query)
        {
            var result = await QueryItems(query.AsRestQuery());
            return result;
        }

        public async Task<ItemDetailDto> GetItemDetail(Guid id)
        {
            var path = $"GetItem/{id}";
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

        public async Task DeleteItem(Guid itemId)
        {
            await Delete(itemId.ToString());
        }

        protected async Task<IList<ItemSummaryDto>> QueryItems (string filterString)
        {
            var path = $"GetItems{filterString}";
            var result = await Get<IList<ItemSummaryDto>>(path);
            return result;
        }
    }
}