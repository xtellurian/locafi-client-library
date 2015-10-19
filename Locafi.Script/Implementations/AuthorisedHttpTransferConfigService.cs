using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Model.Dto.Authentication;

namespace Locafi.Script.Implementations
{
    public class AuthorisedHttpTransferConfigService : IAuthorisedHttpTransferConfigService
    {
        private TokenGroup _tokenGroup;

        public AuthorisedHttpTransferConfigService(IAuthenticationRepo authenticationRepo, TokenGroup tokenGroup)
        {
            AuthenticationRepo = authenticationRepo;
            _tokenGroup = tokenGroup;
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

        public async Task<TokenGroup> GetTokenGroupAsync()
        {
            return _tokenGroup;
        }

        public async Task SetTokenGroupAsync(TokenGroup tokenGroup)
        {
            _tokenGroup = tokenGroup;
        }

        public IAuthenticationRepo AuthenticationRepo { get; set; }


    }
}
