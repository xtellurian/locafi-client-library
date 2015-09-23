using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.Snapshots;
using Locafi.Client.Model.RFID;

namespace Locafi.Client.Processors.Orders.Strategies
{
    public abstract class StrategyState
    {
        protected StrategyState(IEnumerable<IRfidTag> alreadyAllocated = null, IEnumerable<IRfidTag> alreadyReceived = null )
        {
            AlreadyAllocated = alreadyAllocated?.ToList() ?? new List<IRfidTag>();
            AlreadyReceived = alreadyReceived?.ToList() ?? new List<IRfidTag>();
        }
        public IList<IRfidTag> AlreadyAllocated { get; private set; }
        public IList<IRfidTag> AlreadyReceived { get; private set; }
    }
}
