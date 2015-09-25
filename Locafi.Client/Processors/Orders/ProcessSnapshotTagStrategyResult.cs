using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Processors;
using Locafi.Client.Model.Dto.Orders;
using Locafi.Client.Processors.Orders.Strategies;

namespace Locafi.Client.Processors.Orders
{
    public class ProcessSnapshotTagStrategyResult : IProcessSnapshotTagStrategyResult
    {
        public ProcessSnapshotTagStrategyResult(bool isTagExpected, StrategyState state, OrderSkuLineItemDto skuLineItem= null, 
            OrderItemLineItemDto itemLineItem = null, ProcessSnapshotTagResultCategory resultCategory = ProcessSnapshotTagResultCategory.Ok)
        {
            IsTagExpected = isTagExpected;
            State = state;
            SkuLineItem = skuLineItem;
            ItemLineItem = itemLineItem;
            ResultCategory = resultCategory;

            if (!isTagExpected && resultCategory == ProcessSnapshotTagResultCategory.Ok)
            {
                throw new ArgumentException("Result cannot be OK when success is false");
            }
        }

        public bool IsTagExpected { get; }
        public OrderSkuLineItemDto SkuLineItem { get; }
        public OrderItemLineItemDto ItemLineItem { get; }
        public StrategyState State { get; }
        public ProcessSnapshotTagResultCategory ResultCategory { get; }
    }
}
