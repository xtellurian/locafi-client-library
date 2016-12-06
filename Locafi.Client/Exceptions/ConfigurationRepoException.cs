using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Responses;

namespace Locafi.Client.Exceptions
{
    public class ConfigurationRepoException : WebRepoException
    {
        public ConfigurationRepoException(string message) : base(message)
        {
        }

        public ConfigurationRepoException(IEnumerable<CustomResponseMessage> serverMessages, HttpStatusCode statusCode, string url, string payload)
            : base(serverMessages, statusCode, url, payload)
        {
        }
    }
}
