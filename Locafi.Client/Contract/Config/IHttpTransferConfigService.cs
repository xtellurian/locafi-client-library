using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace Locafi.Client.Contract.Config
{
    public interface IHttpTransferConfigService
    {
        [Obsolete]
        Task<string> GetBaseUrlAsync();

        Task<IHttpTransferConfig> GetConfigAsync();
    }
}