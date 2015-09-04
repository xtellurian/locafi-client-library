using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Responses;

namespace Locafi.Client.Exceptions
{
    public class ReasonRepoException : WebRepoException
    {
        public ReasonRepoException(IEnumerable<CustomResponseMessage> serverMessages) : base(serverMessages)
        {
        }

        public ReasonRepoException(string message) : base(message)
        {
        }
    }
}
