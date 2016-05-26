// This is a contract file
// Breaking changes to this contract should be accompanied by a version change in the third version number
// ie ... 1.2.42.11 -> 1.2.43.0

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.Templates;
using Locafi.Client.Model.Enums;
using Locafi.Client.Model;
using Locafi.Client.Model.Query;

namespace Locafi.Client.Contract.Repo
{
    public interface ITemplateRepo
    {
        Task<PageResult<TemplateSummaryDto>> QueryTemplates(string oDataQueryOptions = null);
        Task<PageResult<TemplateSummaryDto>> QueryTemplates(IRestQuery<TemplateSummaryDto> query);
        Task<IQueryResult<TemplateSummaryDto>> QueryTemplatesContinuation(IRestQuery<TemplateSummaryDto> query);
        Task<TemplateDetailDto> GetById(Guid id);
        Task<PageResult<TemplateSummaryDto>> GetTemplatesForType(TemplateFor templateTarget);
    }
}