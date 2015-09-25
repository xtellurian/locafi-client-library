using Locafi.Client.Model.Dto.Orders;
using Locafi.Client.Model.Dto.Snapshots;
using Locafi.Client.Processors.Orders.Strategies;

namespace Locafi.Client.Contract.Processors
{
    public interface IProcessSnapshotTagOrderStrategy
    {
        IProcessSnapshotTagStrategyResult ProcessTag(SnapshotTagDto snapshotTag, OrderDetailDto orderDetail, StrategyState state);
    }
}
