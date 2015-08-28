using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.Templates;
using Locafi.Client.Model.Enums;

namespace Locafi.Client.Contract.Services
{
    public interface ITemplateRepo
    {
        Task<IList<TemplateSummaryDto>> GetAllTemplates();
        Task<TemplateDetailDto> GetById(Guid id);
        Task<IList<TemplateSummaryDto>> GetTemplatesForType(TemplateFor templateTarget);
    }
}