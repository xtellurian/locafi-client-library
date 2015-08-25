using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.Services;
using Locafi.Client.Data;

namespace Locafi.Client.Services.Repo
{
    public class SnapshotRepo : WebRepo, ISnapshotRepo
    {
        public SnapshotRepo(IAuthorisedHttpTransferConfigService configService, ISerialiserService serialiser) : base(configService, serialiser, "Snapshots")
        {
        }

        public async Task<SnapshotDto> PostSnapshot(SnapshotDto snapshot)
        {
            var result = await Post<SnapshotDto>(snapshot);
            return result;
        }

        public async Task<SnapshotDto> GetSnapshot(string snapshotId)
        {
            var result = await Get<SnapshotDto>("/" + snapshotId);
            return result;
        }

        public async Task<IList<SnapshotDto>> GetAllSnapshots()
        {
            var result = await Get<IList<SnapshotDto>>();
            return result;
        }

        protected async Task<IList<SnapshotDto>> QuerySnapshots(string queryString)
        {
            var result = await Get<IList<SnapshotDto>>(queryString);
            return result;
        }
    }
}
