using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.Http;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Exceptions;
using Locafi.Client.Model.Dto.Inventory;
using Locafi.Client.Model.Dto.Items;
using Locafi.Client.Model.Query;
using Locafi.Client.Model.RelativeUri;
using Locafi.Client.Model.Responses;
using Locafi.Client.Model.Uri;

namespace Locafi.Client.Repo
{
    public class InventoryRepo : WebRepo, IInventoryRepo
    {
        public InventoryRepo(IAuthorisedHttpTransferConfigService authorisedUnauthorizedConfigService, ISerialiserService serialiser) 
            : base(new SimpleHttpTransferer(), authorisedUnauthorizedConfigService, serialiser, InventoryUri.ServiceName)
        {
        }

        public InventoryRepo(IHttpTransferer transferer, IAuthorisedHttpTransferConfigService authorisedUnauthorizedConfigService, ISerialiserService serialiser)
           : base(transferer, authorisedUnauthorizedConfigService, serialiser, InventoryUri.ServiceName)
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

        public async Task<InventoryDetailDto> CreateInventory(Guid placeId, string name = null, Guid? skuGroupId = null)
        {
            var dto = new AddInventoryDto
            {
                Name = name,
                PlaceId = placeId,
                SkuGroupId = skuGroupId
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

        public async Task<InventoryDetailDto> AddItem(InventorySummaryDto inventory, Guid itemId)
        {
            var path = InventoryUri.AddItem(inventory, itemId);
            var result = await Post<InventoryDetailDto>(inventory, path);
            return result;
        }

        public async Task<InventoryDetailDto> Resolve(Guid inventoryId, ResolveInventoryDto reasons)
        {
            var path = InventoryUri.Resolve(inventoryId);
            var result = await Post<InventoryDetailDto>(reasons, path);
            return result;
        }

        public async Task<InventoryDetailDto> Complete(Guid inventoryId)
        {
            var path = InventoryUri.Complete(inventoryId);
            var result = await Post<InventoryDetailDto>(null, path);
            return result;
        }

        public async Task<bool> Delete(Guid id)
        {
            var path = InventoryUri.Delete(id);
            return await Delete(path);
        }

        [Obsolete]
        public async Task<IList<InventorySummaryDto>> QueryInventories(IRestQuery<InventorySummaryDto> query)
        {
            var result = await QueryInventories(query.AsRestQuery());
            return result;
        }

        public async Task<IQueryResult<InventorySummaryDto>> QueryInventoriesAsync(IRestQuery<InventorySummaryDto> query)
        {
            var result = await QueryInventories(query.AsRestQuery());
            return result.AsQueryResult(query);
        }

        protected async Task<IList<InventorySummaryDto>> QueryInventories(string queryString)
        {
            var path = $"{InventoryUri.GetInventories}{queryString}";
            var result = await Get<IList<InventorySummaryDto>>(path);
            return result;
        }

        public async override Task Handle(HttpResponseMessage responseMessage)
        {
            throw new InventoryException(await responseMessage.Content.ReadAsStringAsync());
        }

        public override Task Handle(IEnumerable<CustomResponseMessage> serverMessages, HttpStatusCode statusCode)
        {
            throw new InventoryException(serverMessages, statusCode);
        }
    }
}
