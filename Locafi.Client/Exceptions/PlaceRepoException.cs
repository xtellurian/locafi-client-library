using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Responses;

namespace Locafi.Client.Exceptions
{
    public class PlaceRepoException : WebRepoException
    {

        public PlaceRepoException(IEnumerable<CustomResponseMessage> serverMessages, HttpStatusCode statusCode) 
            : base(serverMessages, statusCode)
        {
        }

        public PlaceRepoException(string message) : base(message)
        {
        }
    }
}
