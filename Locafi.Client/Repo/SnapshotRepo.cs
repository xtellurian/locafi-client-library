using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Model.Dto.Snapshots;
using Locafi.Client.Model.RelativeUri;
using Locafi.Client.Model.Uri;

namespace Locafi.Client.Repo
{
    public class SnapshotRepo : WebRepo, ISnapshotRepo
    {
        public SnapshotRepo(IAuthorisedHttpTransferConfigService unauthorizedConfigService, ISerialiserService serialiser) 
            : base(unauthorizedConfigService, serialiser, SnapshotUri.ServiceName)
        {
        }

        public async Task<SnapshotDetailDto> CreateSnapshot(AddSnapshotDto snapshot)
        {
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

        public async Task Delete(Guid id)
        {
            var path = SnapshotUri.DeleteSnapshot(id);
            await Delete(path);
        }

        protected async Task<IList<SnapshotSummaryDto>> QuerySnapshots(string queryString)
        {
            var path = $"{SnapshotUri.GetSnapshots}/{queryString}";
            var result = await Get<IList<SnapshotSummaryDto>>(path);
            return result;
        }
    }
}
