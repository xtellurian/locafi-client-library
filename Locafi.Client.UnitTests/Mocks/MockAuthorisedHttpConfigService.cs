using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;

namespace Locafi.Client.UnitTests.Mocks
{
    class MockAuthorisedHttpConfigService : IAuthorisedHttpTransferConfigService
    {
        public MockAuthorisedHttpConfigService()
        {
            
        }

        public MockAuthorisedHttpConfigService(Func<IHttpTransferConfigService, Task<IAuthorisedHttpTransferConfigService>> onUnauthorised)
        {
            OnUnauthorised = onUnauthorised;
        }

        public async Task<string> GetBaseUrlAsync()
        {
            return "www.google.com";
        }

        public async Task<string> GetTokenStringAsync()
        {
            return "";
        }

        public Func<IHttpTransferConfigService, Task<IAuthorisedHttpTransferConfigService>> OnUnauthorised { get; set; }
    }
}
