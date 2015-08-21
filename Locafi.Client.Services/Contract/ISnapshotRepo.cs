using System.Threading.Tasks;
using Locafi.Client.Data;

namespace Locafi.Client.Services.Contract
{
    public interface ISnapshotRepo
    {
        Task<SnapshotDto> PostSnapshot(SnapshotDto snapshot);
        Task<SnapshotDto> GetSnapshot(string snapshotId);
    }
}