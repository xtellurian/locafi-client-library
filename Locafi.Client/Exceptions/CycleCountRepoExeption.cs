using Locafi.Client.Model.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Exceptions
{
    public class CycleCountRepoExeption : WebRepoException
    {
        public CycleCountRepoExeption()
        {

        }

        public CycleCountRepoExeption(string message) : base(message)
        {

        }

        public CycleCountRepoExeption(IEnumerable<CustomResponseMessage> serverMessages, HttpStatusCode statusCode, string url, string payload) 
            :base(serverMessages, statusCode, url, payload)
        {
        }
    }
}
