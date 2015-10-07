using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Model.Dto.Authentication;

namespace Locafi.Client.UnitTests.Implementations
{
    public class AuthorisedHttpTransferConfigService : IAuthorisedHttpTransferConfigService
    {
        private readonly TokenGroup _tokenGroup;

        public AuthorisedHttpTransferConfigService(TokenGroup tokenGroup)
        {
            _tokenGroup = tokenGroup;
        }

        public AuthorisedHttpTransferConfigService(TokenGroup tokenGroup, Func<IHttpTransferConfigService, Task<IAuthorisedHttpTransferConfigService>> onUnauthorised) : this(tokenGroup)
        {
            OnUnauthorised = onUnauthorised;
        }

        public string BaseUrl { get; set; }
        public async Task<string> GetBaseUrlAsync()
        {
            return BaseUrl;
        }

        public string GetTokenString()
        {
            return _tokenGroup?.Token;
        }

        public async Task<string> GetTokenStringAsync()
        {
            return _tokenGroup?.Token;
        }

        public Func<IHttpTransferConfigService, Task<IAuthorisedHttpTransferConfigService>> OnUnauthorised { get; set; }

    }
}
