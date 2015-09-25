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
            
            var gtin = SgtinTagCoder.GetGtin(snapshotTag.TagNumber);
            if (orderDetail.RequiredSkus.Any(rs => string.Equals(rs.Gtin, gtin)))
                // check if the sku is a part of the order
            {
                var skuDetail = orderDetail.RequiredSkus.FirstOrDefault(t => string.Equals(t.Gtin, gtin));
                //check if Tag is already allocated / added to the stae
                if (state.AlreadyAllocated.Any(tag => string.Equals(tag.TagNumber , snapshotTag.TagNumber)) ||
                    allocateState.TagsAddedThisRound.Any(tag => string.Equals(tag.TagNumber, snapshotTag.TagNumber)))
                {
                    // return OK
                    return new ProcessSnapshotTagStrategyResult(true, allocateState);
                }
                else
                { 
                    // add tag to the state
                    allocateState.AddSkuLineItem(skuDetail.SkuId, snapshotTag);
                    // Is Extra Item Allowed?
                    if (skuDetail.QtyAllocated + allocateState.QuantityOfSkuAddedthisRound[skuDetail.SkuId] <=
                        skuDetail.Quantity) // extra item is allowed
                    {
                        return new ProcessSnapshotTagStrategyResult(true, allocateState, skuDetail);
                    }
                    else
                    {
                        // we have too many of this sku
                        return new ProcessSnapshotTagStrategyResult(false, state ,skuDetail, null, ProcessSnapshotTagResultCategory.LineOverAllocated);
                    }
                }
            }
            else
            {
                allocateState.AddTag(snapshotTag);
                // check if tag is in list of unique items
                var item =
                    orderDetail.RequiredItems.FirstOrDefault(tag => string.Equals(tag.TagNumber, snapshotTag.TagNumber));
                if (item!=null)
                {
                    // OK
                    return new ProcessSnapshotTagStrategyResult(true, allocateState, null, item);
                }
                else
                {
                    // unknown tag
                    return new ProcessSnapshotTagStrategyResult(false, allocateState, null, null, ProcessSnapshotTagResultCategory.UnknownTag);
                }
            }
                
        }


        sealed private class AllocateState : StrategyState
        {
            public AllocateState(IList<IRfidTag> alreadyAllocated, IList<IRfidTag> alreadyReceived) : base(alreadyAllocated, alreadyReceived)
            {
                TagsAddedThisRound = new List<SnapshotTagDto>();
                QuantityOfSkuAddedthisRound = new Dictionary<Guid, int>();
            }

            internal void AddTag(SnapshotTagDto tag)
            {
                TagsAddedThisRound.Add(tag);
            }

            internal void AddSkuLineItem(Guid skuId, SnapshotTagDto tag)
            {
                if (QuantityOfSkuAddedthisRound.ContainsKey(skuId))
                {
                    QuantityOfSkuAddedthisRound[skuId] ++;
                }
                else
                {
                    QuantityOfSkuAddedthisRound[skuId] = 1;
                }
                TagsAddedThisRound.Add(tag);
            }
            public IDictionary<Guid,int> QuantityOfSkuAddedthisRound { get; private set; }
            public IList<SnapshotTagDto> TagsAddedThisRound { get; private set; } 
        }
    }
}
