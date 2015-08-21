using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Data;
using Locafi.Client.Services.Contract;
using Locafi.Client.Services.Odata;
using Newtonsoft.Json;

namespace Locafi.Client.Services.Repo
{
    public class ItemRepo : WebRepo<ODataCollection<ItemDto>>, IItemRepo
    {
        private readonly ISerialiserService _serialiser;

        public ItemRepo(IHttpTransferConfigService transferConfig, ISerialiserService serialiser) : base(transferConfig, serialiser, "Items")
        {
            _serialiser = serialiser;
        }

        //public async Task<bool> UpdateItem(ItemDto item)
        //{
        //    var result = await PutResult(item, "('" + item.Id + "')");
        //    return result.IsSuccessCode;
        //}

        public async Task<ItemDto> AddItem(AddItemDto item)
        {
            var result = await PostResult(item);
            var obj = _serialiser.Deserialise<ItemDto>(result);
            return obj;
        }

        public async Task<ItemDto> GetItemFromTag(string tagNumber)
        {
            var path = $"?$filter=TagNumber eq '{tagNumber}'";
            var result = await Get(path);
            return result.Value.FirstOrDefault();
        }

        public async Task<ItemDto> GetItemById(string id)
        {
            var path = $"?$filter=Id eq '{id}'";
            var result = await Get(path);
            return result.Value.FirstOrDefault();
        }
    }
}
