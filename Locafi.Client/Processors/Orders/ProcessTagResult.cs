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
        public ProcessTagResult(bool isRecognised, string gtin, OrderSkuLineItemDto skuLineItem = null, OrderItemLineItemDto itemLineItem = null)
        {
            IsRecognised = isRecognised;
            Gtin = gtin;
            SkuLineItem = skuLineItem;
            ItemLineItem = itemLineItem;
        }

        public bool IsRecognised { get; }
        public OrderSkuLineItemDto SkuLineItem { get; }
        public OrderItemLineItemDto ItemLineItem { get; }
        public string Gtin { get; }
    }
}
