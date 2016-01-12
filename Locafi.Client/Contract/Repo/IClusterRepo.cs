using System.Threading.Tasks;
using Locafi.Client.Model.Dto.Devices;

namespace Locafi.Client.Contract.Repo
{
    public interface IClusterRepo
    {
        Task<ClusterResponseDto> ProcessCluster(ClusterDto cluster);
    }
}