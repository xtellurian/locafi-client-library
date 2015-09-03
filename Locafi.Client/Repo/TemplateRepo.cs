using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Model.Dto.Templates;
using Locafi.Client.Model.Enums;
using Locafi.Client.Model.RelativeUri;

namespace Locafi.Client.Repo
{
    public class TemplateRepo : WebRepo, ITemplateRepo
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

    }
}
