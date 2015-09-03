using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.Errors;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Exceptions;
using Locafi.Client.Model.Dto.Inventory;
using Locafi.Client.Model.Query;
using Locafi.Client.Model.Responses;
using Locafi.Client.Model.Uri;

namespace Locafi.Client.Repo
{
    public class InventoryRepo : WebRepo, IInventoryRepo, IWebRepoErrorHandler
    {
        public InventoryRepo(IAuthorisedHttpTransferConfigService authorisedUnauthorizedConfigService, ISerialiserService serialiser) 
            : base(authorisedUnauthorizedConfigService, serialiser, InventoryUri.ServiceName)
        {
        }

        public async Task<IList<InventorySummaryDto>> GetAllInventories()
        {
            var path = InventoryUri.GetInventories;
            var result = await Get<IList<InventorySummaryDto>>(path);
            return result;
        }

        public async Task<InventoryDetailDto> GetInventory(Guid id)
        {
            var path = InventoryUri.GetInventory(id);
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
            var path = InventoryUri.CreateInventory;
            var result = await Post<InventoryDetailDto>(dto, path);
            return result;
        }

        public async Task<InventoryDetailDto> AddSnapshot(InventorySummaryDto inventory, Guid snapshotId)
        {
            var path = InventoryUri.AddSnapshot(inventory, snapshotId);
            var result = await Post<InventoryDetailDto>(inventory, path);
            return result;
        }

        public async Task<InventoryDetailDto> Resolve(Guid inventoryId, ResolveInventoryDto reasons)
        {
            var path = InventoryUri.Resolve(inventoryId);
            var result = await Post<InventoryDetailDto>(reasons, path);
            return result;
        }

        public async Task Delete(Guid id)
        {
            var path = InventoryUri.Delete(id);
            await Delete(path);
        }

        public async Task<IList<InventorySummaryDto>> QueryInventories(IRestQuery<InventorySummaryDto> query)
        {
            var result = await QueryInventories(query.AsRestQuery());
            return result;
        }

        protected async Task<IList<InventorySummaryDto>> QueryInventories(string queryString)
        {
            var path = $"{InventoryUri.GetInventories}{queryString}";
            var result = await Get<IList<InventorySummaryDto>>(path);
            return result;
        }

        public async Task Handle(HttpResponseMessage responseMessage)
        {
            throw new InventoryException(await responseMessage.Content.ReadAsStringAsync());
        }

        public Task Handle(IEnumerable<CustomResponseMessage> serverMessages)
        {
            throw new InventoryException(serverMessages);
        }
    }
}
