using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Http;

namespace Locafi.Client.UnitTests.Mocks
{
    class UnauthorisedMockHttpTransferer : IHttpTransferer
    {

        public async Task<HttpResponseMessage> GetResponse(HttpMethod method, string url, string content = null, string authToken = null,
            IDictionary<string, string> headers = null)
        {
            return new HttpResponseMessage(HttpStatusCode.Unauthorized);
        }
    }
}
