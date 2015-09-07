using System;
using System.Collections.Generic;
using System.Net;
using Locafi.Client.Model.Responses;

namespace Locafi.Client.Exceptions
{
    public class InventoryException : WebRepoException
    {
        public InventoryException()
        {
            
        }

        public InventoryException(string message) : base(message)
        {
            
        }

        public InventoryException(IEnumerable<CustomResponseMessage> serverMessages, HttpStatusCode statusCode) 
            :base(serverMessages, statusCode)
        {
        }
    }
}
