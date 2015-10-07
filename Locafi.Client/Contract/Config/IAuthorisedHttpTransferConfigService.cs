// This is a contract file
// Breaking changes to this contract should be accompanied by a version change in the third version number
// ie ... 1.2.42.11 -> 1.2.43.0

using System;
using System.Threading.Tasks;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Model.Dto.Authentication;

namespace Locafi.Client.Contract.Config
{
    public interface IAuthorisedHttpTransferConfigService : IHttpTransferConfigService
    {
        /// <summary>
        /// A function that is used by WebRepo to try and get a new AuthorisedConfigService
        /// You should pass in a Func that uses your Refresh Token to re-authenticate
        /// The input argument to the Func is the internal HttpConfig from the WebRepo.
        /// </summary>

        IAuthenticationRepo AuthenticationRepo { get; set; }
        Task<TokenGroup> GetTokenGroupAsync();
        Task SetTokenGroupAsync(TokenGroup tokenGroup);
    }
}