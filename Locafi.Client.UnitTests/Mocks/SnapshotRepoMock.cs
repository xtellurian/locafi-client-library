using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Model.Dto.Snapshots;
using Locafi.Client.Model.Query;
using Locafi.Client.Model;

namespace Locafi.Client.UnitTests.Mocks
{
    public class SnapshotRepoMock : ISnapshotRepo
    {
        public Task<SnapshotDetailDto> CreateSnapshot(AddSnapshotDto snapshot)
        {
            throw new NotImplementedException();
        }

        public Task<SnapshotDetailDto> GetSnapshot(Guid snapshotId)
        {
            throw new NotImplementedException();
        }

        public Task<IList<SnapshotSummaryDto>> GetAllSnapshots()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<PageResult<SnapshotSummaryDto>> QuerySnapshots(string oDataQueryOptions = null)
        {
            throw new NotImplementedException();
        }

        public Task<PageResult<SnapshotSummaryDto>> QuerySnapshots(IRestQuery<SnapshotSummaryDto> query)
        {
            throw new NotImplementedException();
        }

        public Task<IQueryResult<SnapshotSummaryDto>> QuerySnapshotsContinuation(IRestQuery<SnapshotSummaryDto> query)
        {
            throw new NotImplementedException();
        }
    }
}
