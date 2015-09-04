using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Responses;

namespace Locafi.Client.Exceptions
{
    public class SkuRepoException : WebRepoException

    {
        public SkuRepoException(IEnumerable<CustomResponseMessage> serverMessages) : base(serverMessages)
        {

        }

        public SkuRepoException(string message) : base(message)
        {
        }
    }
}
