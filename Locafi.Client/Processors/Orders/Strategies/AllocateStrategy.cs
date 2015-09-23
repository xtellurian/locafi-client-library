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
        public IProcessSnapshotTagResult ProcessTag(SnapshotTagDto snapshotTag, OrderDetailDto orderDetail, StrategyState state)
        {
            var allocateState = state as AllocateState ?? new AllocateState(state.AlreadyAllocated, state.AlreadyReceived); // init state
            
            var gtin = SgtinTagCoder.GetGtin(snapshotTag.TagNumber);
            if (orderDetail.RequiredSkus.Any(rs => string.Equals(rs.SgtinRef, gtin)))
                // check if the sku is a part of the order
            {
                var skuDetail = orderDetail.RequiredSkus.FirstOrDefault(t => string.Equals(t.SgtinRef, gtin));
                //check if Tag is already allocated / added to the stae
                if (state.AlreadyAllocated.Any(tag => string.Equals(tag.TagNumber , snapshotTag.TagNumber)) ||
                    allocateState.TagsAddedThisRound.Any(tag => string.Equals(tag.TagNumber, snapshotTag.TagNumber)))
                {
                    // return OK
                    return new ProcessSnapshotTagResult(true, allocateState);
                }
                else
                { 
                    // add tag to the state
                    allocateState.AddSkuLineItem(skuDetail.SkuId, snapshotTag);
                    // Is Extra Item Allowed?
                    if (skuDetail.QtyAllocated + allocateState.QuantityOfSkuAddedthisRound[skuDetail.SkuId] <=
                        skuDetail.Quantity) // extra item is allowed
                    {
                        return new ProcessSnapshotTagResult(true, allocateState);
                    }
                    else
                    {
                        // we have too many of this sku
                        return new ProcessSnapshotTagResult(false, null, ProcessSnapshotTagResultCategory.LineOverAllocated);
                    }
                }
            }
            else
            {
                allocateState.AddTag(snapshotTag);
                // check if tag is in list of unique items
                if (orderDetail.RequiredItems.Any(tag => string.Equals(tag.TagNumber, snapshotTag.TagNumber)))
                {
                    // OK
                    return new ProcessSnapshotTagResult(true, allocateState);
                }
                else
                {
                    // unknown tag
                    return new ProcessSnapshotTagResult(false, allocateState, ProcessSnapshotTagResultCategory.UnknownTag);
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
