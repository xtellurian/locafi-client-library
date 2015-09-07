using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Responses;

namespace Locafi.Client.Exceptions
{
    public class ItemException : WebRepoException
    {
        public ItemException(string message) : base(message)
        {
        }

        public ItemException(IEnumerable<CustomResponseMessage> serverMessages, HttpStatusCode statusCode)
            : base(serverMessages, statusCode)
        {
        }
    }
}
