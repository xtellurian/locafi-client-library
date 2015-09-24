﻿using System;
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
    public class ReceiveStrategy : IProcessSnapshotTagOrderStrategy
    {
        public IProcessSnapshotTagResult ProcessTag(SnapshotTagDto snapshotTag, OrderDetailDto orderDetail, StrategyState state)
        {
            var receivedState = state as ReceiveState ?? new ReceiveState(state.AlreadyAllocated, state.AlreadyReceived);

            var gtin = SgtinTagCoder.GetGtin(snapshotTag.TagNumber);
            if (receivedState.TagsAddedThisRound.Any(tag => string.Equals(tag.TagNumber, snapshotTag.TagNumber)))
            {
                return new ProcessSnapshotTagResult(true, receivedState);
            }
            receivedState.AddTag(snapshotTag);
            var skuDetail = orderDetail.RequiredSkus.FirstOrDefault(d => string.Equals(d.SgtinRef, gtin));
            // check if part of allocated snaps
            if (receivedState.AlreadyAllocated.Any(tag => string.Equals(tag.TagNumber, snapshotTag.TagNumber)))
            {
                
                if (skuDetail != null) // gtin is in order
                {
                    if (!receivedState.QuantityOfSkuAddedthisRound.ContainsKey(skuDetail.SkuId))
                    {
                        receivedState.QuantityOfSkuAddedthisRound[skuDetail.SkuId] = 0;
                    }
                    receivedState.QuantityOfSkuAddedthisRound[skuDetail.SkuId] ++;
                    if (skuDetail.QtyAllocated >
                        skuDetail.QtyReceived + receivedState.QuantityOfSkuAddedthisRound[skuDetail.SkuId])
                        // over received
                    {
                        return new ProcessSnapshotTagResult(false, receivedState,
                            ProcessSnapshotTagResultCategory.LineOverReceived);
                    }
                    else
                    {
                        return new ProcessSnapshotTagResult(true, receivedState);
                    }
                }
                else
                {
                    return new ProcessSnapshotTagResult(false, receivedState, ProcessSnapshotTagResultCategory.UnknownTag);
                }
                
            }
            else
            {
                // tag is not part of allocation
                if (skuDetail == null)
                {
                    return new ProcessSnapshotTagResult(false, receivedState,
                        ProcessSnapshotTagResultCategory.UnknownTag);
                }
                else
                {
                    // sku is in order but tag was never allocated
                    return new ProcessSnapshotTagResult(false, receivedState, ProcessSnapshotTagResultCategory.TagNumberMismatch );
                }
            }

        }


        private sealed class ReceiveState : StrategyState
        {
            public ReceiveState(IList<IRfidTag> alreadyAllocated, IList<IRfidTag> alreadyReceived) : base(alreadyAllocated, alreadyReceived)
            {
                TagsAddedThisRound = new List<SnapshotTagDto>();
                QuantityOfSkuAddedthisRound = new Dictionary<Guid, int>();
            }

            internal void AddTag(SnapshotTagDto tag)
            {
                TagsAddedThisRound.Add(tag);
            }

            public IDictionary<Guid, int> QuantityOfSkuAddedthisRound { get; private set; }
            public IList<SnapshotTagDto> TagsAddedThisRound { get; private set; }
        }
    }
}