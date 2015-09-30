// This is a contract file
// Breaking changes to this contract should be accompanied by a version change in the third version number
// ie ... 1.2.42.11 -> 1.2.43.0

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.Snapshots;
using Locafi.Client.Model.Query;

namespace Locafi.Client.Contract.Repo
{
    public interface ISnapshotRepo
    {
        Task<SnapshotDetailDto> CreateSnapshot(AddSnapshotDto snapshot);
        Task<SnapshotDetailDto> GetSnapshot(Guid snapshotId);
        Task<IList<SnapshotSummaryDto>> GetAllSnapshots();
        Task<bool> Delete(Guid id);
        [Obsolete]
        Task<IList<SnapshotSummaryDto>> QuerySnapshots(IRestQuery<SnapshotSummaryDto> query);
        Task<IQueryResult<SnapshotSummaryDto>> QuerySnapshotsAsync(IRestQuery<SnapshotSummaryDto> query);
    }
}