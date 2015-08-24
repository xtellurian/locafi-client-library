using System;
using System.Threading.Tasks;

namespace Locafi.Client.Contract.Config
{
    public interface IHttpTransferConfigService
    {
        Task<string> GetBaseUrlString();

        string GetTokenString();
       // void SetTokenString(string token);
        Task<Uri> GetBaseUri();
    }
}