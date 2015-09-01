using System;

namespace Locafi.Client.Exceptions
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
