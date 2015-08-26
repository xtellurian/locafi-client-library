using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.Services;
using Locafi.Client.Data;
using Locafi.Client.Model.Extensions;

namespace Locafi.Client.Services.Repo
{
    public class SnapshotRepo : WebRepo, ISnapshotRepo
    {
        public SnapshotRepo(IAuthorisedHttpTransferConfigService configService, ISerialiserService serialiser) : base(configService, serialiser, "Snapshots")
        {
        }

        public async Task<SnapshotDto> CreateSnapshot(SnapshotDto snapshot)
        {
            var path = snapshot.CreateUri();
            var result = await Post<SnapshotDto>(snapshot, path);
            return result;
        }

        public async Task<SnapshotDto> GetSnapshot(Guid snapshotId)
        {
            var path = $"GetSnapshot/{snapshotId}";
            var result = await Get<SnapshotDto>(path);
            return result;
        }

        public async Task<IList<SnapshotDto>> GetAllSnapshots()
        {
            var path = "GetSnapshots";
            var result = await Get<IList<SnapshotDto>>(path);
            return result;
        }

        protected async Task<IList<SnapshotDto>> QuerySnapshots(string queryString)
        {
            var result = await Get<IList<SnapshotDto>>(queryString);
            return result;
        }
    }
}
