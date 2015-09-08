using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.ConfigurationContract;

namespace Locafi.Client.UnitTests.Implementations
{
    public class UnauthorisedHttpTransferConfigService : IHttpTransferConfigService
    {
        public string BaseUrl => StringConstants.BaseUrl;
    }
}
