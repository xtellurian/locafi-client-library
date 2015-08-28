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
        Inventory_NewItem,
        Inventory_UnexpectedItem,
        Inventory_MissingItem
    }
}
