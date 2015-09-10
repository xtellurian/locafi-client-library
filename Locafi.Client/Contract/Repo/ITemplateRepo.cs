// This is a contract file
// Breaking changes to this contract should be accompanied by a version change in the third version number
// ie ... 1.2.42.11 -> 1.2.43.0

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.Templates;
using Locafi.Client.Model.Enums;

namespace Locafi.Client.Contract.Repo
{
    public interface ITemplateRepo
    {
        Task<IList<TemplateSummaryDto>> GetAllTemplates();
        Task<TemplateDetailDto> GetById(Guid id);
        Task<IList<TemplateSummaryDto>> GetTemplatesForType(TemplateFor templateTarget);
    }
}