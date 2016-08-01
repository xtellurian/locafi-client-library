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
using Locafi.Client.Model.Dto.Snapshots;
using Locafi.Client.Model.Query;
using Locafi.Client.Model.RelativeUri;
using Locafi.Client.Model.Responses;
using Locafi.Client.Model.Uri;
using Locafi.Client.Model;

namespace Locafi.Client.Repo
{
    public class SnapshotRepo : WebRepo, ISnapshotRepo, IWebRepoErrorHandler
    {
        public SnapshotRepo(IAuthorisedHttpTransferConfigService configService, ISerialiserService serialiser) 
            : base(new SimpleHttpTransferer(), configService, serialiser, SnapshotUri.ServiceName)
        {
        }

        public SnapshotRepo(IHttpTransferer transferer, IAuthorisedHttpTransferConfigService authorisedConfigService, ISerialiserService serialiser)
           : base(transferer, authorisedConfigService, serialiser, SnapshotUri.ServiceName)
        {
        }

        public async Task<SnapshotDetailDto> CreateSnapshot(AddSnapshotDto snapshot)
        {
            if (snapshot.EndTime <= snapshot.StartTime) snapshot.EndTime = DateTime.UtcNow;
            var path = SnapshotUri.CreateUri;
            var result = await Post<SnapshotDetailDto>(snapshot, path);
            return result;
        }

        public async Task<SnapshotDetailDto> GetSnapshot(Guid snapshotId)
        {
            var path = SnapshotUri.GetSnapshot(snapshotId);
            var result = await Get<SnapshotDetailDto>(path);
            return result;
        }

        public async Task<PageResult<SnapshotSummaryDto>> QuerySnapshots(string oDataQueryOptions = null)
        {
            var path = SnapshotUri.GetSnapshots;

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
            var result = await Get<PageResult<SnapshotSummaryDto>>(path);
            return result;
        }

        public async Task<PageResult<SnapshotSummaryDto>> QuerySnapshots(IRestQuery<SnapshotSummaryDto> query)
        {
            return await QuerySnapshots(query.AsRestQuery());
        }

        public async Task<IQueryResult<SnapshotSummaryDto>> QuerySnapshotsContinuation(IRestQuery<SnapshotSummaryDto> query)
        {
            var result = await QuerySnapshots(query.AsRestQuery());
            return result.AsQueryResult(query);
        }

        public async Task<bool> Delete(Guid id)
        {
            var path = SnapshotUri.DeleteSnapshot(id);
            return await Delete(path);
        }

        public override async Task Handle(HttpResponseMessage responseMessage, string url, string payload)
        {
            throw new SnapshotRepoException($"{url} -- {payload} -- " + await responseMessage.Content.ReadAsStringAsync());
        }

        public override async Task Handle(IEnumerable<CustomResponseMessage> serverMessages, HttpStatusCode statusCode, string url, string payload)
        {
            throw new SnapshotRepoException(serverMessages, statusCode, url, payload);
        }
    }
}
