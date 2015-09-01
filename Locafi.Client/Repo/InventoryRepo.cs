using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.Services;
using Locafi.Client.Errors;
using Locafi.Client.Exceptions;
using Locafi.Client.Model.Dto.Inventory;
using Locafi.Client.Model.Extensions;
using Locafi.Client.Services;

namespace Locafi.Client.Repo
{
    public class InventoryRepo : WebRepo, IInventoryRepo, IWebRepoErrorHandler
    {
        public InventoryRepo(IAuthorisedHttpTransferConfigService authorisedUnauthorizedConfigService, ISerialiserService serialiser) 
            : base(authorisedUnauthorizedConfigService, serialiser, "Inventory")
        {
        }

        public async Task<IList<InventorySummaryDto>> GetAllInventories()
        {
            var path = "GetInventories";
            var result = await Get<IList<InventorySummaryDto>>(path);
            return result;
        }

        public async Task<InventoryDetailDto> GetInventory(Guid id)
        {
            var path = $"GetInventory/{id}";
            var result = await Get<InventoryDetailDto>(path);
            return result;
        }

        public async Task<InventoryDetailDto> CreateInventory(string name, Guid placeId)
        {
            var dto = new AddInventoryDto
            {
                Name = name,
                PlaceId = placeId
            };
            var path = "Create";
            var result = await Post<InventoryDetailDto>(dto, path);
            return result;
        }

        public async Task<InventoryDetailDto> AddSnapshot(InventorySummaryDto inventory, Guid snapshotId)
        {
            var path = inventory.AddSnapshotUri(snapshotId);
            var result = await Post<InventoryDetailDto>(inventory, path);
            return result;
        }

        public async Task<InventoryDetailDto> Resolve(InventorySummaryDto inventory)
        {
            var path = inventory.ResolveUri();
            var result = await Post<InventoryDetailDto>(inventory, path);
            return result;
        }

        public async Task Delete(Guid id)
        {
            var path = $"DeleteInventory/{id}";
            await Delete(path);
        }

        protected async Task<IList<InventorySummaryDto>> QueryInventories(string queryString)
        {
            var result = await Get<IList<InventorySummaryDto>>(queryString);
            return result;
        }

        public async Task Handle(HttpResponseMessage responseMessage)
        {
            throw new InventoryException(await responseMessage.Content.ReadAsStringAsync());
        }
    }
}
