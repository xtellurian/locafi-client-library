using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;

namespace Locafi.Client.UnitTests.Implementations
{
    class HttpConfig : IHttpTransferConfig
    {
        public HttpConfig()
        {
            
        }
        public HttpConfig(string baseUrl, KeyValuePair<string,string> authHeader)
        {
            
        }
        public string BaseUrl { get; set; }
        public Dictionary<string, string> Headers { get; set; }
    }
}
