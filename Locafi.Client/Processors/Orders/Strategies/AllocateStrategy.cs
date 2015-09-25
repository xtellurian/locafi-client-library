using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Processors;
using Locafi.Client.Model.Dto.Orders;
using Locafi.Client.Model.Dto.Snapshots;
using Locafi.Client.Model.RFID;
using Locafi.Client.Processors.Encoding;

namespace Locafi.Client.Processors.Orders.Strategies
{
    /// <summary>
    /// 
    /// </summary>
    public class AllocateStrategy : IProcessSnapshotTagOrderStrategy
    {
        public IProcessSnapshotTagStrategyResult ProcessTag(SnapshotTagDto snapshotTag, OrderDetailDto orderDetail, StrategyState state)
        {
            var allocateState = state as AllocateState ?? new AllocateState(state.AlreadyAllocated, state.AlreadyReceived); // init state

            if (state.AlreadyAllocated.Any(tag => string.Equals(tag.TagNumber, snapshotTag.TagNumber)) ||
        allocateState.TagsAddedThisRound.Any(tag => string.Equals(tag.TagNumber, snapshotTag.TagNumber)))
            {
                // return OK
                return new ProcessSnapshotTagStrategyResult(true, allocateState, ProcessSnapshotTagResultCategory.AllocateOk);
            }
            allocateState.AddTag(snapshotTag);


            var gtin = SgtinTagCoder.GetGtin(snapshotTag.TagNumber);
            if (orderDetail.RequiredSkus.Any(rs => string.Equals(rs.Gtin, gtin)))
                // check if the sku is a part of the order
            {
                var skuDetail = orderDetail.RequiredSkus.FirstOrDefault(t => string.Equals(t.Gtin, gtin));
                skuDetail.QtyAllocated++; // increment amount allocated
                //check if Tag is already allocated / added to the stae
                    
                // Is Extra Item Allowed?
                return skuDetail.QtyAllocated <=
                       skuDetail.Quantity ? new ProcessSnapshotTagStrategyResult(true, allocateState, ProcessSnapshotTagResultCategory.AllocateOk, skuDetail) 
                       : new ProcessSnapshotTagStrategyResult(false, state, ProcessSnapshotTagResultCategory.LineOverAllocated, skuDetail, null);

            }
            else
            {
                // check if tag is in list of unique items
                var item =
                    orderDetail.RequiredItems.FirstOrDefault(tag => string.Equals(tag.TagNumber, snapshotTag.TagNumber));
                if (item!=null)
                {
                    // OK
                    return new ProcessSnapshotTagStrategyResult(true, allocateState,ProcessSnapshotTagResultCategory.AllocateOk, null, item);
                }
                else
                {
                    // unknown tag
                    return new ProcessSnapshotTagStrategyResult(false, allocateState, ProcessSnapshotTagResultCategory.UnknownTag, null, null);
                }
            }
                
        }


        sealed private class AllocateState : StrategyState
        {
            public AllocateState(IList<IRfidTag> alreadyAllocated, IList<IRfidTag> alreadyReceived) : base(alreadyAllocated, alreadyReceived)
            {
                TagsAddedThisRound = new List<SnapshotTagDto>();
            }

            internal void AddTag(SnapshotTagDto tag)
            {
                TagsAddedThisRound.Add(tag);
            }

            public IList<SnapshotTagDto> TagsAddedThisRound { get; private set; } 
        }
    }
}
