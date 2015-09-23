using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Processors.Orders.Strategies;

namespace Locafi.Client.Contract.Processors
{
    public interface IProcessSnapshotTagResult
    {
        bool IsSuccessful { get; }
        StrategyState State { get; }
        ProcessSnapshotTagResultCategory ResultCategory { get; }
    }
}
