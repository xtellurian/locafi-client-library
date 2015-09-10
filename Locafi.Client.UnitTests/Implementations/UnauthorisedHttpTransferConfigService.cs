using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;

namespace Locafi.Client.UnitTests.Implementations
{
    public class UnauthorisedHttpTransferConfigService : IHttpTransferConfigService
    {
        public string BaseUrl => StringConstants.BaseUrl;
        public async Task<string> GetBaseUrlAsync()
        {
            return StringConstants.BaseUrl;
        }

        public async Task<IHttpTransferConfig> GetConfigAsync()
        {
            return new HttpConfig() {BaseUrl = StringConstants.BaseUrl};
        }
    }
}
