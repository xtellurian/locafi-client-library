using System.Threading.Tasks;

namespace Locafi.Client.Contract.Config
{
    public interface IAuthorisedHttpTransferConfigService : IHttpTransferConfigService
    { 
        Task<string> GetTokenStringAsync();
    }
}