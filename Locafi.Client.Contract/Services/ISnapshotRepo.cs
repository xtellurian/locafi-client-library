using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Data;

namespace Locafi.Client.Contract.Services
{
    public interface ISnapshotRepo
    {
        Task<SnapshotDto> CreateSnapshot(SnapshotDto snapshot);
        Task<SnapshotDto> GetSnapshot(Guid snapshotId);
        Task<IList<SnapshotDto>> GetAllSnapshots();
    }
}