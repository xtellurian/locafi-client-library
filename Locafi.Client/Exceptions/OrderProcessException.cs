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
        public IProcessSnapshotTagResult ProcessResult { get; set; }

        public OrderProcessException()
        {
            
        }

        public OrderProcessException(IProcessSnapshotTagResult processResult)
        {
            ProcessResult = processResult;
        }
    }
}
