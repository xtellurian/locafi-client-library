using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Contract.Http
{
    public interface IHttpTransferer
    {
        Task<HttpResponseMessage> GetResponse(HttpMethod method, string url, string content = null, string authToken = null, IDictionary<string, string> headers = null);
    }
}
