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
        public ProcessSnapshotTagStrategyResult(bool isTagExpected, StrategyState state, ProcessSnapshotTagResultCategory resultCategory, 
            OrderSkuLineItemDto skuLineItem= null, OrderItemLineItemDto itemLineItem = null)
        {
            IsTagExpected = isTagExpected;
            State = state;
            SkuLineItem = skuLineItem;
            ItemLineItem = itemLineItem;
            ResultCategory = resultCategory;

            if (state == null)
            {
                throw new NullReferenceException("Strategy State cannot be null");
            }

            if (!isTagExpected && (resultCategory == ProcessSnapshotTagResultCategory.AllocateOk || resultCategory == ProcessSnapshotTagResultCategory.ReceiveOk))
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
