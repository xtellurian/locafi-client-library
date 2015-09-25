using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.Orders;
using Locafi.Client.Processors.Orders.Strategies;

namespace Locafi.Client.Contract.Processors
{
    public interface IProcessSnapshotTagStrategyResult
    {
        bool IsTagExpected { get; }
        OrderSkuLineItemDto SkuLineItem { get; }
        OrderItemLineItemDto ItemLineItem { get; }
        StrategyState State { get; }
        ProcessSnapshotTagResultCategory ResultCategory { get; }
    }
}
