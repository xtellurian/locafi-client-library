using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Enums
{
    public enum OrderStatus
    {
        Cancelled,
        Created,
        Allocating,
        PartiallyAllocated,
        Allocated,
        Dispatched,
        Receiving,
        PartiallyReceived,
        Received,
        Disputed,
        Closed
    }
}
