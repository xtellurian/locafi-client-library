using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.Services;
using Locafi.Client.Data;
using Locafi.Client.Model.Dto.Snapshots;
using Locafi.Client.Model.Extensions;

namespace Locafi.Client.Services.Repo
{
    public class SnapshotRepo : WebRepo, ISnapshotRepo
    {
        public SnapshotRepo(IAuthorisedHttpTransferConfigService unauthorizedConfigService, ISerialiserService serialiser) : base(unauthorizedConfigService, serialiser, "Snapshots")
        {
        }

        public async Task<SnapshotDetailDto> CreateSnapshot(AddSnapshotDto snapshot)
        {
            var path = "CreateSnapshot";
            var result = await Post<SnapshotDetailDto>(snapshot, path);
            return result;
        }

        public async Task<SnapshotDetailDto> GetSnapshot(Guid snapshotId)
        {
            var path = $"GetSnapshot/{snapshotId}";
            var result = await Get<SnapshotDetailDto>(path);
            return result;
        }

        public async Task<IList<SnapshotSummaryDto>> GetAllSnapshots()
        {
            var path = "GetSnapshots";
            var result = await Get<IList<SnapshotSummaryDto>>(path);
            return result;
        }

        public async Task Delete(Guid id)
        {
            var path = $"DeleteSnapshot/{id}";
            await Delete(path);
        }

        protected async Task<IList<SnapshotSummaryDto>> QuerySnapshots(string queryString)
        {
            var result = await Get<IList<SnapshotSummaryDto>>(queryString);
            return result;
        }
    }
}
