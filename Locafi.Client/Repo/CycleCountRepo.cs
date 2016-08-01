using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.Http;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Exceptions;
using Locafi.Client.Model;
using Locafi.Client.Model.Dto.CycleCountDtos;
using Locafi.Client.Model.Query;
using Locafi.Client.Model.RelativeUri;
using Locafi.Client.Model.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Repo
{
    public class CycleCountRepo : WebRepo, ICycleCountRepo
    {
        public CycleCountRepo(IAuthorisedHttpTransferConfigService authorisedConfigService, ISerialiserService serialiser) 
            : base(new SimpleHttpTransferer(), authorisedConfigService, serialiser, CycleCountUri.ServiceName)
        {
        }

        public CycleCountRepo(IHttpTransferer transferer, IAuthorisedHttpTransferConfigService authorisedConfigService, ISerialiserService serialiser)
           : base(transferer, authorisedConfigService, serialiser, CycleCountUri.ServiceName)
        {
        }

        public async Task<PageResult<CycleCountSummaryDto>> QueryCycleCounts(string oDataQueryOptions = null)
        {
            var path = CycleCountUri.GetCycleCounts;

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
            var result = await Get<PageResult<CycleCountSummaryDto>>(path);
            return result;
        }

        public async Task<PageResult<CycleCountSummaryDto>> QueryCycleCounts(IRestQuery<CycleCountSummaryDto> query)
        {
            var result = await QueryCycleCounts(query.AsRestQuery());
            return result;
        }

        public async Task<IQueryResult<CycleCountSummaryDto>> QueryCycleCountsWithContinuation(IRestQuery<CycleCountSummaryDto> query)
        {
            var result = await QueryCycleCounts(query.AsRestQuery());
            return result.AsQueryResult(query);
        }

        public async Task<CycleCountDetailDto> GetCycleCount(Guid id)
        {
            var path = CycleCountUri.GetCycleCount(id);
            var result = await Get<CycleCountDetailDto>(path);
            return result;
        }

        public async Task<CycleCountDetailDto> CreateCycleCount(AddCycleCountDto addDto)
        {
            var path = CycleCountUri.AddCycleCount;
            var result = await Post<CycleCountDetailDto>(addDto, path);
            return result;
        }

        public async Task<CycleCountDetailDto> ResolveCycleCount(ResolveCycleCountDto resolveDto)
        {
            var path = CycleCountUri.ResolveCycleCount;
            var result = await Post<CycleCountDetailDto>(resolveDto, path);
            return result;
        }

        public async override Task Handle(HttpResponseMessage responseMessage, string url, string payload)
        {
            throw new CycleCountRepoExeption($"{url} -- {payload} -- " + await responseMessage.Content.ReadAsStringAsync());
        }

        public override Task Handle(IEnumerable<CustomResponseMessage> serverMessages, HttpStatusCode statusCode, string url, string payload)
        {
            throw new CycleCountRepoExeption(serverMessages, statusCode, url, payload);
        }
    }
}
