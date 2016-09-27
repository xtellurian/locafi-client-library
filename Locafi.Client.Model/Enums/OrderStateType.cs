using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Enums
{
    public enum OrderStateType
    {
        Created,
        Allocating,
        Allocated,
        Dispatched,
        Receiving,
        Received,
        Completed,
        Cancelled
    }
}
