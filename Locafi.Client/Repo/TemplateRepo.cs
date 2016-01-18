using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.ErrorHandlers;
using Locafi.Client.Contract.Http;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Exceptions;
using Locafi.Client.Model.Dto.Templates;
using Locafi.Client.Model.Enums;
using Locafi.Client.Model.RelativeUri;
using Locafi.Client.Model.Responses;

namespace Locafi.Client.Repo
{
    public class TemplateRepo : WebRepo, ITemplateRepo, IWebRepoErrorHandler
    {
        public TemplateRepo(IAuthorisedHttpTransferConfigService authorisedConfigService, ISerialiserService serialiser) 
            : base(new SimpleHttpTransferer(), authorisedConfigService, serialiser, TemplateUri.ServiceName)
        {
        }

        public TemplateRepo(IHttpTransferer transferer, IAuthorisedHttpTransferConfigService authorisedConfigService, ISerialiserService serialiser)
           : base(transferer, authorisedConfigService, serialiser, TemplateUri.ServiceName)
        {
        }

        public async Task<IList<TemplateSummaryDto>> GetAllTemplates()
        {
            var path = TemplateUri.GetTemplates;
            var result = await Get<List<TemplateSummaryDto>>(path);
            return result;
        }

        public async Task<TemplateDetailDto> GetById(Guid id)
        {
            var path = TemplateUri.GetTemplate(id);
            var result = await Get<TemplateDetailDto>(path);
            return result;
        }

        public async Task<IList<TemplateSummaryDto>> GetTemplatesForType(TemplateFor templateTarget)
        {
            var path = TemplateUri.GetTemplateFor(templateTarget);
            var result = await Get<List<TemplateSummaryDto>>(path);
            return result;
        }

        public override async Task Handle(HttpResponseMessage responseMessage, string url, string payload)
        {
            throw new TemplateRepoException($"{url} -- {payload} -- " + await responseMessage.Content.ReadAsStringAsync());
        }

        public override Task Handle(IEnumerable<CustomResponseMessage> serverMessages, HttpStatusCode statusCode, string url, string payload)
        {
            throw new TemplateRepoException(serverMessages, statusCode, url, payload);
        }
    }
}
