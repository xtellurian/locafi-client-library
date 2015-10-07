using System;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.UnitTests;

namespace Locafi.Script.Implementations
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

        private string _token;

        public string GetTokenString()
        {
            return _token;
        }

        public async Task<string> GetTokenStringAsync()
        {
            return StringConstants.Token;
        }

        public Func<IHttpTransferConfigService, Task<IAuthorisedHttpTransferConfigService>> OnUnauthorised { get; set; }

        public void SetTokenString(string token)
        {
            _token = token;
        }
    }
}
