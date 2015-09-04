using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Responses;

namespace Locafi.Client.Exceptions
{
    public class SnapshotRepoException : WebRepoException
    {
        public SnapshotRepoException(IEnumerable<CustomResponseMessage> serverMessages) : base(serverMessages)
        {
        }

        public SnapshotRepoException(string message) : base(message)
        {
        }
    }
}
