using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Responses;

namespace Locafi.Client.Exceptions
{
    public class TemplateRepoException : WebRepoException
    {
        public TemplateRepoException(IEnumerable<CustomResponseMessage> serverMessages) : base(serverMessages)
        {
        }

        public TemplateRepoException(string message) : base(message)
        {
        }
    }
}
