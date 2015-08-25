using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.Services;
using Locafi.Client.Data;
using Locafi.Client.Model.Actions;
using Locafi.Client.Model.Extensions;

namespace Locafi.Client.Services.Repo
{
    public class InventoryRepo : WebRepo, IInventoryRepo
    {
        public InventoryRepo(IAuthorisedHttpTransferConfigService authorisedConfigService, ISerialiserService serialiser) 
            : base(authorisedConfigService, serialiser, "Inventory")
        {
        }

        public async Task<IList<InventoryDto>> GetAllInventories()
        {
            var result = await Get<IList<InventoryDto>>();
            return result;
        }

        public async Task<InventoryDto> GetInventory(string id)
        {
            var result = await Get<InventoryDto>(id);
            return result;
        }

        public async Task<InventoryDto> CreateInventory(string name, Guid placeId)
        {
            var dto = new InventoryBaseDto
            {
                Name = name,
                PlaceId = placeId
            };
            var result = await Post<InventoryDto>(dto, dto.RelativeUri(InventoryAction.Create, null));
            return result;

        }

        protected async Task<IList<InventoryDto>> QueryInventories(string queryString)
        {
            var result = await Get<IList<InventoryDto>>(queryString);
            return result;
        }
    }
}
