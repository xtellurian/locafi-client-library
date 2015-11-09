using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Processors;
using Locafi.Client.Model.Dto.Orders;
using Locafi.Client.Model.RFID;

namespace Locafi.Client.Processors.Orders
{
    public class OrderReceiver : OrderProcessor
    {
        public OrderReceiver(OrderDetailDto orderDetail) : base(orderDetail)
        {
        }

        private readonly object _skuLock = new object();

        public override IProcessTagResult Add(IRfidTag tag)
        {
            base.Add(tag);
            var gtin = GetGtin(tag);
            var expectedSku = base.GetSkuLineItem(tag);
            if (expectedSku != null)
            {
                lock (_skuLock)
                {
                    if(!expectedSku.ReceivedTagNumbers.Contains(tag.TagNumber)) expectedSku.ReceivedTagNumbers.Add(tag.TagNumber);
                }
                
                return new ProcessTagResult(true, gtin, expectedSku);
            }

            var expectedItem = base.GetItemLineItem(tag);
            if (expectedItem != null)
            {
                expectedItem.IsAllocated = true;
                return new ProcessTagResult(true, gtin, itemLineItem:expectedItem);

            }
            OrderDetail.UnknownTags.Add(tag);
            return new ProcessTagResult(false, gtin);
        }

        public override IProcessTagResult Remove(IRfidTag tag)
        {
            base.Remove(tag);

            var gtin = GetGtin(tag);
            var expectedSku = base.GetSkuLineItem(tag);
            if (expectedSku != null)
            {
                lock (_skuLock)
                {
                    if (expectedSku.ReceivedTagNumbers.Contains(tag.TagNumber)) expectedSku.ReceivedTagNumbers.Remove(tag.TagNumber);
                }

                return new ProcessTagResult(true, gtin, expectedSku);
            }

            var expectedItem = base.GetItemLineItem(tag);
            if (expectedItem != null)
            {
                expectedItem.IsAllocated = false;
                return new ProcessTagResult(true, gtin, itemLineItem: expectedItem);

            }
            OrderDetail.UnknownTags.Add(tag);
            return new ProcessTagResult(false, gtin);
        }
    }
}
