using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.Items;
using Locafi.Client.Model.Dto.Orders;

namespace Locafi.Client.Contract.Processors
{
    public interface IProcessTagResult
    {
        bool IsDisputeRequired { get; }
        bool IsUnrecognisedTag { get; }
        OrderSkuLineItemDto SkuLineItem { get; }
        OrderItemLineItemDto ItemLineItem { get; }
        

    }
}
