using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Responses;

namespace Locafi.Client.Exceptions
{
    public class AuthenticationRepoException : WebRepoException
    {
        public AuthenticationRepoException(IEnumerable<CustomResponseMessage> serverMessages, HttpStatusCode statusCode) 
            : base(serverMessages, statusCode)
        {
        }

        public AuthenticationRepoException(string message) : base(message)
        {
        }
    }
}
