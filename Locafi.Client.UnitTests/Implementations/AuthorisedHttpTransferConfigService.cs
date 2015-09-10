using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;

namespace Locafi.Client.UnitTests.Implementations
{
    public class AuthorisedHttpTransferConfigService : IAuthorisedHttpTransferConfigService
    {
        public AuthorisedHttpTransferConfigService(string token)
        {
            _token = token;
        }

        public string BaseUrl { get; set; }
        public async Task<string> GetBaseUrlAsync()
        {
            return BaseUrl;
        }

        public async Task<IHttpTransferConfig> GetConfigAsync()
        {
            var auth = new KeyValuePair<string, string>("Authorization", $"Token {_token}");
            return new HttpConfig(BaseUrl,auth);
        }

        private string _token;

        public string GetTokenString()
        {
            return _token;
        }

        public async Task<string> GetTokenStringAsync()
        {
            return _token;
        }

        public void SetTokenString(string token)
        {
            _token = token;
        }
    }
}
