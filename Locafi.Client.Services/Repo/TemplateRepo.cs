using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.Services;
using Locafi.Client.Model.Dto.Templates;
using Locafi.Client.Model.Enums;

namespace Locafi.Client.Services.Repo
{
    public class TemplateRepo : WebRepo, ITemplateRepo
    {
        public TemplateRepo(IAuthorisedHttpTransferConfigService authorisedUnauthorizedConfigService, ISerialiserService serialiser) 
            : base(authorisedUnauthorizedConfigService, serialiser, "templates")
        {
        }

        public async Task<IList<TemplateSummaryDto>> GetAllTemplates()
        {
            var path = "GetTemplates";
            var result = await Get<IList<TemplateSummaryDto>>(path);
            return result;
        }

        public async Task<TemplateDetailDto> GetById(Guid id)
        {
            var path = $"GetTemplate/{id}";
            var result = await Get<TemplateDetailDto>(path);
            return result;
        }

        public async Task<IList<TemplateSummaryDto>> GetTemplatesForType(TemplateFor templateTarget)
        {
            var path = $"GetTemplatesForType/{Enum.GetName(typeof(TemplateFor),templateTarget)}";
            var result = await Get<IList<TemplateSummaryDto>>(path);
            return result;
        }

    }
}
