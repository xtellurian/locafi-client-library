using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Responses;

namespace Locafi.Client.Exceptions
{
    public class PersonException : WebRepoException
    {
        public PersonException(IEnumerable<CustomResponseMessage> serverMessages) : base(serverMessages)
        {
        }

        public PersonException(string message) : base(message)
        {
        }
    }
}
