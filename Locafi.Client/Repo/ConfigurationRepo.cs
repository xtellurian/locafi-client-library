using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.Http;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Exceptions;
using Locafi.Client.Model.Dto.Configuration;
using Locafi.Client.Model.RelativeUri;
using Locafi.Client.Model.Responses;

namespace Locafi.Client.Repo
{
    public class ConfigurationRepo : WebRepo, IConfigurationRepo
    {
        public ConfigurationRepo(IHttpTransferer transferer, IAuthorisedHttpTransferConfigService authorisedConfigService, ISerialiserService serialiser, string service) : base(transferer, authorisedConfigService, serialiser, service)
        {
        }

        public ConfigurationRepo(IHttpTransferer transferer, IHttpTransferConfigService configService, ISerialiserService serialiser, string service) : base(transferer, configService, serialiser, service)
        {
        }

        public async Task<ConfigurationDto> GetConfigurations()
        {
            var path = ConfigurationUri.GetConfigurations;
            var result = await Get<ConfigurationDto>(path);
            return result;
        }

        public override Task Handle(IEnumerable<CustomResponseMessage> serverMessages, HttpStatusCode statusCode, string url, string payload)
        {
            throw new ConfigurationRepoException(serverMessages, statusCode, url, payload);
        }

        public override async Task Handle(HttpResponseMessage response, string url, string payload)
        {
            throw new ConfigurationRepoException($"{url} -- {payload} -- " + await response.Content.ReadAsStringAsync());
        }
    }
}
