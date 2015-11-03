using System.Linq;
using Locafi.Client.Contract.Processors;
using Locafi.Client.Model.Dto.Orders;
using Locafi.Client.Model.RFID;
using Locafi.Client.Processors.Encoding;
using Locafi.Client.Processors.Orders.Processors;

namespace Locafi.Client.Processors.Orders
{
    public class OrderAllocator : OrderProcessor
    {
        public OrderAllocator(OrderDetailDto orderDetail) : base(orderDetail)
        {
        }

        public override IProcessTagResult Add(IRfidTag tag)
        {
            base.Add(tag);


            var expectedSku = base.GetSkuLineItem(tag);
            if (expectedSku != null)
            {
                expectedSku.AllocatedTagNumbers.Add(tag.TagNumber);
                return new ProcessTagResult(true, skuLineItem:expectedSku);
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

