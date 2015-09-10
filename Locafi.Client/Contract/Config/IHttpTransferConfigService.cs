using System.Threading.Tasks;

namespace Locafi.Client.Contract.Config
{
    public interface IHttpTransferConfigService
    {
        Task<string> GetBaseUrlAsync();
    }
}