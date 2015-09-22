using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.UnitTests;

namespace Locafi.Script.Implementations
{
    public class UnauthorisedHttpTransferConfigService : IHttpTransferConfigService
    {
        public string BaseUrl => StringConstants.BaseUrl;
        public async Task<string> GetBaseUrlAsync()
        {
            return StringConstants.BaseUrl;
        }
    }
}
