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

        Func<IHttpTransferConfigService, Task<IAuthorisedHttpTransferConfigService>> OnUnauthorised { get; set; }
    }
}