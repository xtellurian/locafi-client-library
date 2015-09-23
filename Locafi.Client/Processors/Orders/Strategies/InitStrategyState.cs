using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.RFID;

namespace Locafi.Client.Processors.Orders.Strategies
{
    public sealed class InitStrategyState : StrategyState
    {
        public InitStrategyState(IEnumerable<IRfidTag> alreadyAllocated, IEnumerable<IRfidTag> alreadyReceived):base(alreadyAllocated, alreadyReceived)
        {
            
        }
    }
}
