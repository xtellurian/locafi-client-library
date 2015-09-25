using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Processors.Orders.Strategies
{
    public enum ProcessSnapshotTagResultCategory
    {
        AllocateOk,
        ReceiveOk,
        LineOverAllocated,
        LineOverReceived,
        UnknownTag
    }
}
