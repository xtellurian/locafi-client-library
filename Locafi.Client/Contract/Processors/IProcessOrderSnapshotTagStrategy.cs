using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.Orders;
using Locafi.Client.Model.Dto.Snapshots;
using Locafi.Client.Processors.Orders.Strategies;

namespace Locafi.Client.Contract.Processors
{
    public interface IProcessSnapshotTagOrderStrategy
    {
        IProcessSnapshotTagResult ProcessTag(SnapshotTagDto snapshotTag, OrderDetailDto orderDetail, StrategyState state);
    }
}
