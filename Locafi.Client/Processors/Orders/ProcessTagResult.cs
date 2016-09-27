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
        public ProcessTagResult(bool isRecognised, string gtin, ReadOrderSkuDto skuLineItem = null, ReadOrderItemDto itemLineItem = null)
        {
            IsRecognised = isRecognised;
            Gtin = gtin;
            SkuLineItem = skuLineItem;
            ItemLineItem = itemLineItem;
        }

        public bool IsRecognised { get; }
        public ReadOrderSkuDto SkuLineItem { get; }
        public ReadOrderItemDto ItemLineItem { get; }
        public string Gtin { get; }
    }
}
