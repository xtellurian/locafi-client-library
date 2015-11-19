using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Responses;

namespace Locafi.Client.Exceptions
{
    public class SkuGroupRepoException : WebRepoException
    {
        public SkuGroupRepoException(IEnumerable<CustomResponseMessage> serverMessages, HttpStatusCode statusCode):base(serverMessages,statusCode)
        {
        }

        public SkuGroupRepoException(string message) : base(message)
        {
            
        }
    }
}
