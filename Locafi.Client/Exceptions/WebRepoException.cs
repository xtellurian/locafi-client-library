using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Responses;

namespace Locafi.Client.Exceptions
{
    public abstract class WebRepoException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public IList<CustomResponseMessage> ServerMessages { get; private set; }

        public HttpStatusCode HttpResponseCode { get; private set; }

        protected WebRepoException()
        {
            
        }
        protected WebRepoException(string message) : base(message)
        {
        }

        protected WebRepoException(IEnumerable<CustomResponseMessage> serverMessages, HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
            ServerMessages = serverMessages.ToList();
        }
    }
}
