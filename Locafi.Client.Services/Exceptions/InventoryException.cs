using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Services.Exceptions
{
    public class InventoryException : Exception
    {
        public InventoryException()
        {
            
        }

        public InventoryException(string message) : base(message)
        {
            
        }
    }
}
