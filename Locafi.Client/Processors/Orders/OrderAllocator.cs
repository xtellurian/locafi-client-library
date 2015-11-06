using System.Linq;
using Locafi.Client.Contract.Processors;
using Locafi.Client.Model.Dto.Orders;
using Locafi.Client.Model.RFID;
using Locafi.Client.Processors.Encoding;

namespace Locafi.Client.Processors.Orders
{
    public class OrderAllocator : OrderProcessor
    {
        public OrderAllocator(OrderDetailDto orderDetail) : base(orderDetail)
        {
        }

        private readonly object _skuLock = new object();
        private readonly object _unknownTagLock = new object();
        public override IProcessTagResult Add(IRfidTag tag)
        {
            base.Add(tag);


            var expectedSku = base.GetSkuLineItem(tag);
            if (expectedSku != null)
            {
                lock (_skuLock)
                {
                    if (!expectedSku.AllocatedTagNumbers.Contains(tag.TagNumber)) expectedSku.AllocatedTagNumbers.Add(tag.TagNumber);
                }
                
                return new ProcessTagResult(true, skuLineItem:expectedSku);
            }

            var expectedItem = base.GetItemLineItem(tag);
            if (expectedItem != null)
            {
                expectedItem.IsAllocated = true;
                return new ProcessTagResult(true, itemLineItem:expectedItem);
            }
            lock (_unknownTagLock)
            {
                OrderDetail.UnknownTags.Add(tag);
            }
            
            return new ProcessTagResult(false);
        }


    }
}

