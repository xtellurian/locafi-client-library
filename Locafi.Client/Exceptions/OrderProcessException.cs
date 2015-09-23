using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Processors;

namespace Locafi.Client.Exceptions
{
    public class OrderProcessException : System.Exception
    {
        public OrderProcessException()
        {
            
        }

        public OrderProcessException(IProcessSnapshotTagResult processResult)
        {
            
        }
    }
}
