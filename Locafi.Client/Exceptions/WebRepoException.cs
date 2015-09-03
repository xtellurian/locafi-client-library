using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Responses;

namespace Locafi.Client.Exceptions
{
    public abstract class WebRepoException : Exception
    {
        public IList<CustomResponseMessage> ServerMessages { get; set; }

        protected WebRepoException()
        {
            
        }
        protected WebRepoException(string message) : base(message)
        {
        }

        protected WebRepoException(IEnumerable<CustomResponseMessage> serverMessages)
        {
            ServerMessages = new List<CustomResponseMessage>(serverMessages);
        }
    }
}
