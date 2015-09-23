using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Processors;
using Locafi.Client.Processors.Orders.Strategies;

namespace Locafi.Client.Processors.Orders
{
    public class ProcessSnapshotTagResult : IProcessSnapshotTagResult
    {
        public ProcessSnapshotTagResult(bool isTagExpected, StrategyState state, ProcessSnapshotTagResultCategory resultCategory = ProcessSnapshotTagResultCategory.Ok)
        {
            IsTagExpected = isTagExpected;
            State = state;
            ResultCategory = resultCategory;

            if (!isTagExpected && resultCategory == ProcessSnapshotTagResultCategory.Ok)
            {
                throw new ArgumentException("Result cannot be OK when success is false");
            }
        }

        public bool IsTagExpected { get; }
        public StrategyState State { get; }
        public ProcessSnapshotTagResultCategory ResultCategory { get; }
    }
}
