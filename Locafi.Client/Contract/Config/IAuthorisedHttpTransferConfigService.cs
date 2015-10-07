// This is a contract file
// Breaking changes to this contract should be accompanied by a version change in the third version number
// ie ... 1.2.42.11 -> 1.2.43.0

using System;
using System.Threading.Tasks;

namespace Locafi.Client.Contract.Config
{
    public interface IAuthorisedHttpTransferConfigService : IHttpTransferConfigService
    { 
        Task<string> GetTokenStringAsync();
        /// <summary>
        /// A function that is used by WebRepo to try and get a new AuthorisedConfigService
        /// You should pass in a Func that uses your Refresh Token to re-authenticate
        /// The input argument to the Func is the internal HttpConfig from the WebRepo.
        /// </summary>
        Func<IHttpTransferConfigService, Task<IAuthorisedHttpTransferConfigService>> OnUnauthorised { get; set; }
    }
}