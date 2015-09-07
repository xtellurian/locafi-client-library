using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Enums
{
    public enum ReasonFor
    {
        Order_Allocate,
        Order_Receive,
        Order_Cancel,
        Inventory_ExpectedItem,
        Inventory_UnexpectedItem,
        Inventory_MissingItem
    }
}
