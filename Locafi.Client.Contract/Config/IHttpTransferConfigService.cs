using System.Threading.Tasks;

namespace Locafi.Client.Contract.Config
{
    public interface IHttpTransferConfigService
    {
        Task<string> GetBaseUrl();
        string GetTokenString();
       // void SetTokenString(string token);
    }
}