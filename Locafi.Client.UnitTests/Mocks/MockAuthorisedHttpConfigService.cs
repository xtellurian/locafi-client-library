using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Model.Dto.Authentication;

namespace Locafi.Client.UnitTests.Mocks
{
    class MockAuthorisedHttpConfigService : IAuthorisedHttpTransferConfigService
    {
        public MockAuthorisedHttpConfigService()
        {
            
        }

        public async Task<string> GetBaseUrlAsync()
        {
            return "www.google.com";
        }

        public async Task<string> GetTokenStringAsync()
        {
            return "";
        }

        public IAuthenticationRepo AuthenticationRepo { get; set; }
        public async Task<TokenGroup> GetTokenGroupAsync()
        {
            return new TokenGroup();
        }

        public async Task SetTokenGroupAsync(TokenGroup tokenGroup)
        {
            
        }
    }
}
