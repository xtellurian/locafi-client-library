using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.ErrorHandlers;
using Locafi.Client.Contract.Http;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Exceptions;
using Locafi.Client.Model.Dto.Reasons;
using Locafi.Client.Model.Enums;
using Locafi.Client.Model.Responses;
using Locafi.Client.Model.Uri;
using Locafi.Client.Model;
using Locafi.Client.Model.Query;
using Locafi.Client.Model.Query.Builder;
using System.Linq;

namespace Locafi.Client.Repo
{
    public class ReasonRepo : WebRepo, IReasonRepo, IWebRepoErrorHandler
    {
        public ReasonRepo(IAuthorisedHttpTransferConfigService authorisedConfigService, ISerialiserService serialiser) 
            : base(new SimpleHttpTransferer(), authorisedConfigService, serialiser, ReasonUri.ServiceName)
        {
        }

        public ReasonRepo(IHttpTransferer transferer, IAuthorisedHttpTransferConfigService authorisedConfigService, ISerialiserService serialiser)
           : base(transferer, authorisedConfigService, serialiser, ReasonUri.ServiceName)
        {
        }

        public async Task<PageResult<ReasonSummaryDto>> QueryReasons(string oDataQueryOptions = null)
        {
            var path = ReasonUri.GetReasons;

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
            var result = await Get<PageResult<ReasonSummaryDto>>(path);
            return result;
        }

        public async Task<PageResult<ReasonSummaryDto>> QueryReasons(IRestQuery<ReasonSummaryDto> query)
        {
            return await QueryReasons(query.AsRestQuery());
        }

        public async Task<IQueryResult<ReasonSummaryDto>> QueryReasonsContinuation(IRestQuery<ReasonSummaryDto> query)
        {
            var result = await QueryReasons(query.AsRestQuery());
            return result.AsQueryResult(query);
        }

        public async Task<ReasonDetailDto> GetReason(Guid id)
        {
            var path = ReasonUri.GetReason(id);
            return await Get<ReasonDetailDto>(path);
        }

        public async Task<ReasonDetailDto> CreateReason(AddReasonDto reasonDto)
        {
            var path = ReasonUri.CreateReason;
            var result = await Post<ReasonDetailDto>(reasonDto, path);
            return result;
        }

        public async Task<ReasonDetailDto> UpdateReason(UpdateReasonDto dto)
        {
            var path = ReasonUri.UpdateReason;
            var result = await Post<ReasonDetailDto>(dto, path);
            return result;
        }
/*
        public async Task<IList<ReasonDetailDto>> GetReasonsFor(ReasonFor reason)
        {
            var query = QueryBuilder<ReasonDetailDto>.NewQuery(r => r.ReasonFor, reason, ComparisonOperator.Equals).Build();
            var result = await QueryReasons(query);
            return result.Items.ToList();
        }
*/
        public async Task<bool> Delete(Guid id)
        {
            var path = ReasonUri.DeleteReason(id);
            return await Delete(path);
        }

        public override async Task Handle(HttpResponseMessage responseMessage, string url, string payload)
        {
            throw new ReasonRepoException($"{url} -- {payload} -- " + await responseMessage.Content.ReadAsStringAsync());
        }

        public override Task Handle(IEnumerable<CustomResponseMessage> serverMessages, HttpStatusCode statusCode, string url, string payload)
        {
            throw new ReasonRepoException(serverMessages, statusCode, url, payload);
        }
    }
}
