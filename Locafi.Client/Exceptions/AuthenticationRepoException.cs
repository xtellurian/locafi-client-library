using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Responses;

namespace Locafi.Client.Exceptions
{
    public class AuthenticationRepoException : WebRepoException
    {
        public AuthenticationRepoException(IEnumerable<CustomResponseMessage> serverMessages) : base(serverMessages)
        {
        }

        public AuthenticationRepoException(string message) : base(message)
        {
        }
    }
}
