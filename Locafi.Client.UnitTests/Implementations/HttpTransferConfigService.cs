using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;

namespace Locafi.Client.UnitTests.Implementations
{
    public class HttpTransferConfigService : IHttpTransferConfigService
    {
        public HttpTransferConfigService(string token)
        {
            _token = token;
        }

        internal string BaseUrl { get; set; }

        private string _token;

        public async Task<string> GetBaseUrl()
        {
            return BaseUrl;
        }

        public string GetTokenString()
        {
            return _token;
        }

        public void SetTokenString(string token)
        {
            _token = token;
        }
    }
}
