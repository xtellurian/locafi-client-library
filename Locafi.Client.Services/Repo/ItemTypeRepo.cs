using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Data;
using Locafi.Client.Services.Contract;
using Locafi.Client.Services.Odata;

namespace Locafi.Client.Services.Repo
{
    public class ItemTypeRepo : WebRepo<ODataCollection<ItemTypeDto>>, IItemTypeRepo
    {
        public ItemTypeRepo(IHttpTransferConfigService downloader, ISerialiserService entitySerialiser) : base(downloader, entitySerialiser, "ItemType/")
        {
        }

        public async Task<List<ItemTypeDto>> GetAllItemTypes()
        {
            var result = await Get();
            return result.Value;
        }
    }
}
