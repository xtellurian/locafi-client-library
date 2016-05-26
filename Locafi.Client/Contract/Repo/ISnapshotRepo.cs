// This is a contract file
// Breaking changes to this contract should be accompanied by a version change in the third version number
// ie ... 1.2.42.11 -> 1.2.43.0

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.Snapshots;
using Locafi.Client.Model.Query;
using Locafi.Client.Model;

namespace Locafi.Client.Contract.Repo
{
    public interface ISnapshotRepo
    {
        Task<SnapshotDetailDto> CreateSnapshot(AddSnapshotDto snapshot);
        Task<SnapshotDetailDto> GetSnapshot(Guid snapshotId);
        Task<PageResult<SnapshotSummaryDto>> QuerySnapshots(string oDataQueryOptions = null);
        Task<PageResult<SnapshotSummaryDto>> QuerySnapshots(IRestQuery<SnapshotSummaryDto> query);
        Task<IQueryResult<SnapshotSummaryDto>> QuerySnapshotsContinuation(IRestQuery<SnapshotSummaryDto> query);
        Task<bool> Delete(Guid id);
    }
}