using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Processors;
using Locafi.Client.Model.Dto.Orders;
using Locafi.Client.Model.RFID;
using Locafi.Client.Processors.Orders.Processors;

namespace Locafi.Client.Processors.Orders
{
    public class OrderReceiver : OrderProcessor
    {
        public OrderReceiver(OrderDetailDto orderDetail) : base(orderDetail)
        {
        }

        public override IProcessTagResult Add(IRfidTag tag)
        {
            base.Add(tag);
            var expectedSku = base.GetSkuLineItem(tag);
            if (expectedSku != null)
            {
                expectedSku.ReceivedTagNumbers.Add(tag.TagNumber);
                return new ProcessTagResult(true, expectedSku);
            }

            var expectedItem = base.GetItemLineItem(tag);
            if (expectedItem != null)
            {
                expectedItem.IsAllocated = true;
                return new ProcessTagResult(true, itemLineItem:expectedItem);

            }
            OrderDetail.UnknownTags.Add(tag);
            return new ProcessTagResult(false);
        }
    }
}
