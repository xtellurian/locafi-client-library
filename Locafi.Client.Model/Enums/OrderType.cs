using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Enums
{
    public enum OrderType
    {
        Inbound,    // receive
        Outbound,   // allocate dispatch
        Transfer,   //allocate dispatch - receive
        Return,     // allocate dispatch receive
        Loan,       // allocate dispatch and receive
    }
}
