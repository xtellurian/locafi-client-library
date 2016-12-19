using System.Threading.Tasks;
using Locafi.Client.Model.Dto.Portals.Clusters;

//using Locafi.Client.Model.Dto.Devices;

namespace Locafi.Client.Contract.Repo
{
    public interface IClusterRepo
    {
        Task<bool> ProcessCluster(ClusterDto cluster);
        Task FlushCache(int? amount = null);
    }
}