using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.Snapshots;

namespace Locafi.Client.Contract.Repo
{
    public interface ISnapshotRepo
    {
        Task<SnapshotDetailDto> CreateSnapshot(AddSnapshotDto snapshot);
        Task<SnapshotDetailDto> GetSnapshot(Guid snapshotId);
        Task<IList<SnapshotSummaryDto>> GetAllSnapshots();
        Task Delete(Guid id);
    }
}