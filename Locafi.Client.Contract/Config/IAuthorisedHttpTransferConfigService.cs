using System;
using System.Threading.Tasks;

namespace Locafi.Client.Contract.Config
{
    public interface IAuthorisedHttpTransferConfigService : IHttpTransferConfigService
    { 
        string GetTokenString();
    }
}