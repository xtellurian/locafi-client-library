using System;
using System.Threading.Tasks;

namespace Locafi.Client.Contract.Config
{
    public interface IAuthorisedHttpTransferConfigService : IHttpTransferConfigService
    { 
        [Obsolete]
        Task<string> GetTokenStringAsync();
    }
}