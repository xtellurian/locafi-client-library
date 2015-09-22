using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.Errors;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Exceptions;
using Locafi.Client.Model.Dto.Snapshots;
using Locafi.Client.Model.Query;
using Locafi.Client.Model.RelativeUri;
using Locafi.Client.Model.Responses;
using Locafi.Client.Model.Uri;

namespace Locafi.Client.Repo
{
    public class SnapshotRepo : WebRepo, ISnapshotRepo, IWebRepoErrorHandler
    {
        public SnapshotRepo(IAuthorisedHttpTransferConfigService unauthorizedConfigService, ISerialiserService serialiser) 
            : base(unauthorizedConfigService, serialiser, SnapshotUri.ServiceName)
        {
        }

        public async Task<SnapshotDetailDto> CreateSnapshot(AddSnapshotDto snapshot)
        {
            if (snapshot.EndTimeUtc <= snapshot.StartTimeUtc) snapshot.EndTimeUtc = DateTime.UtcNow;
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

        public async Task<IList<SnapshotSummaryDto>> GetAllSnapshots()
        {
            var path = SnapshotUri.GetSnapshots;
            var result = await Get<IList<SnapshotSummaryDto>>(path);
            return result;
        }

        public async Task<IList<SnapshotSummaryDto>> QuerySnapshots(IRestQuery<SnapshotSummaryDto> query)
        {
            return await QuerySnapshots(query.AsRestQuery());
        }

        public async Task<bool> Delete(Guid id)
        {
            var path = SnapshotUri.DeleteSnapshot(id);
            return await Delete(path);
        }

        protected async Task<IList<SnapshotSummaryDto>> QuerySnapshots(string queryString)
        {
            var path = $"{SnapshotUri.GetSnapshots}/{queryString}";
            var result = await Get<IList<SnapshotSummaryDto>>(path);
            return result;
        }

        public override async Task Handle(HttpResponseMessage responseMessage)
        {
            throw new SnapshotRepoException(await responseMessage.Content.ReadAsStringAsync());
        }

        public override Task Handle(IEnumerable<CustomResponseMessage> serverMessages, HttpStatusCode statusCode)
        {
            throw new SnapshotRepoException(serverMessages, statusCode);
        }
    }
}
