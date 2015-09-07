﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.Errors;
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
        public TemplateRepo(IAuthorisedHttpTransferConfigService authorisedUnauthorizedConfigService, ISerialiserService serialiser) 
            : base(authorisedUnauthorizedConfigService, serialiser, TemplateUri.ServiceName)
        {
        }

        public async Task<IList<TemplateSummaryDto>> GetAllTemplates()
        {
            var path = TemplateUri.GetTemplates;
            var result = await Get<IList<TemplateSummaryDto>>(path);
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
            var result = await Get<IList<TemplateSummaryDto>>(path);
            return result;
        }

        public override async Task Handle(HttpResponseMessage responseMessage)
        {
            throw new TemplateRepoException(await responseMessage.Content.ReadAsStringAsync());
        }

        public override Task Handle(IEnumerable<CustomResponseMessage> serverMessages)
        {
            throw new TemplateRepoException(serverMessages);
        }
    }
}