using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.ErrorHandlers;
using Locafi.Client.Contract.Http;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Exceptions;
using Locafi.Client.Model.Dto.Skus;
using Locafi.Client.Model.Query;
using Locafi.Client.Model.Responses;
using Locafi.Client.Model.Uri;
using Locafi.Client.Model;
using Locafi.Client.Model.Dto;

namespace Locafi.Client.Repo
{
    public class SkuRepo : WebRepo, ISkuRepo
    {
        public SkuRepo(IAuthorisedHttpTransferConfigService downloader, ISerialiserService entitySerialiser)
            : base(new SimpleHttpTransferer(), downloader, entitySerialiser, SkuUri.ServiceName)
        {
        }

        public SkuRepo(IHttpTransferer transferer, IAuthorisedHttpTransferConfigService authorisedConfigService, ISerialiserService serialiser)
           : base(transferer, authorisedConfigService, serialiser, SkuUri.ServiceName)
        {
        }

        public async Task<SkuDetailDto> CreateSku(AddSkuDto addSkuDto)
        {
            var path = SkuUri.CreateSku;
            var result = await Post<SkuDetailDto>(addSkuDto, path);
            return result;
        }

        public async Task<PageResult<SkuSummaryDto>> QuerySkus(string oDataQueryOptions = null)
        {
            var path = SkuUri.GetSkus;

            // add the query options if required
            if (!string.IsNullOrEmpty(oDataQueryOptions))
            {
                if (oDataQueryOptions[0] != '?')
                    path += "?";

                path += oDataQueryOptions;
            }

            // make sure the query asks to return the item count
            if (!path.Contains("$count"))
            {
                if (path.Contains("?"))
                    path += "&$count=true";
                else
                    path += "?$count=true";
            }

            // run query
            var result = await Get<PageResult<SkuSummaryDto>>(path);
            return result;
        }

        [Obsolete]
        public async Task<PageResult<SkuSummaryDto>> QuerySkus(IRestQuery<SkuSummaryDto> query)
        {
            return await QuerySkus(query.AsRestQuery());
        }

        public async Task<IQueryResult<SkuSummaryDto>> QuerySkusContinuation(IRestQuery<SkuSummaryDto> query)
        {
            var result = await QuerySkus(query.AsRestQuery());
            return result.AsQueryResult(query);
        }

        public async Task<SkuDetailDto> GetSkuDetail(Guid skuId)
        {
            var path = SkuUri.GetSkuDetail(skuId);
            var result = await Get<SkuDetailDto>(path);
            return result;
        }

        public async Task Delete(Guid id)
        {
            var path = SkuUri.DeleteSku(id);
            await Delete(path);
        }

        public async Task<IList<SkuSummaryDto>> SearchSkus(SearchCollectionDto searchSkuQueryDto)
        {
            var path = SkuUri.SearchSkus;
            var result = await Post<List<SkuSummaryDto>>(searchSkuQueryDto, path);
            return result;
        }

        public override async Task Handle(HttpResponseMessage responseMessage, string url, string payload)
        {
            throw new SkuRepoException($"{url} -- {payload} -- " + await responseMessage.Content.ReadAsStringAsync());
        }

        public override Task Handle(IEnumerable<CustomResponseMessage> serverMessages, HttpStatusCode statusCode, string url, string payload)
        {
            throw new SkuRepoException(serverMessages, statusCode, url, payload);
        }
    }
}
