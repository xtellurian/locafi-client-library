using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Processors;
using Locafi.Client.Model.Dto.Orders;

namespace Locafi.Client.Processors.Orders
{
    class ProcessTagResult : IProcessTagResult
    {
        public ProcessTagResult(bool isDisputeRequired, bool isUnrecognisedTag, OrderSkuLineItemDto skuLineItem = null, OrderItemLineItemDto itemLineItem = null)
        {
            IsDisputeRequired = isDisputeRequired;
            IsUnrecognisedTag = isUnrecognisedTag;
            SkuLineItem = skuLineItem;
            ItemLineItem = itemLineItem;
        }

        public bool IsDisputeRequired { get; }
        public bool IsUnrecognisedTag { get; }
        public OrderSkuLineItemDto SkuLineItem { get; }
        public OrderItemLineItemDto ItemLineItem { get; }
    }
}
