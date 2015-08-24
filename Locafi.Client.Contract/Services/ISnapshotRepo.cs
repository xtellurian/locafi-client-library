using System.Threading.Tasks;
using Locafi.Client.Data;

namespace Locafi.Client.Contract.Services
{
    public interface ISnapshotRepo
    {
        Task<SnapshotDto> PostSnapshot(SnapshotDto snapshot);
        Task<SnapshotDto> GetSnapshot(string snapshotId);
    }
}