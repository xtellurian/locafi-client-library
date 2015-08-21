using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Data;
using Locafi.Client.Services.Contract;

namespace Locafi.Client.Services.Repo
{
    public class SnapshotRepo : WebRepo<SnapshotDto>, ISnapshotRepo
    {
        public SnapshotRepo(IHttpTransferConfigService configService, ISerialiserService serialiser) : base(configService, serialiser, "Snapshots")
        {
        }

        public async Task<SnapshotDto> PostSnapshot(SnapshotDto snapshot)
        {
            var result = await Post(snapshot);
            return result;
        }

        public async Task<SnapshotDto> GetSnapshot(string snapshotId)
        {
            var result = await Get("/" + snapshotId);
            return result;
        }
    }
}
