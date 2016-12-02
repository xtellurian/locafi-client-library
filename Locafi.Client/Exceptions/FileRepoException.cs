using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Responses;

namespace Locafi.Client.Exceptions
{
    public class FileRepoException : WebRepoException
    {
        public FileRepoException(string message) : base(message)
        {
        }

        public FileRepoException(IEnumerable<CustomResponseMessage> serverMessages, HttpStatusCode statusCode, string url, string payload)
            : base(serverMessages, statusCode, url, payload)
        {
        }
    }
}
