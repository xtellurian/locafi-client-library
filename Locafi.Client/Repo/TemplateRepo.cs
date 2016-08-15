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
using Locafi.Client.Model;
using Locafi.Client.Model.Query;
using Locafi.Client.Model.Query.PropertyComparison;

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

        public async Task<PageResult<TemplateSummaryDto>> QueryTemplates(string oDataQueryOptions = null)
        {
            var path = TemplateUri.GetTemplates;

            // add the query options if required
            if (!string.IsNullOrEmpty(oDataQueryOptions))
            {
                if (oDataQueryOptions[0] != '?')
                    path += "?";

                path += oDataQueryOptions;
            }

            // make sure the query asks to return the item count
            if (!path.Contains("$count"))
            {
                if (path.Contains("?"))
                    path += "&$count=true";
                else
                    path += "?$count=true";
            }

            // run query
            var result = await Get<PageResult<TemplateSummaryDto>>(path);
            return result;
        }

        public async Task<PageResult<TemplateSummaryDto>> QueryTemplates(IRestQuery<TemplateSummaryDto> query)
        {
            return await QueryTemplates(query.AsRestQuery());
        }

        public async Task<IQueryResult<TemplateSummaryDto>> QueryTemplatesContinuation(IRestQuery<TemplateSummaryDto> query)
        {
            var result = await QueryTemplates(query.AsRestQuery());
            return result.AsQueryResult(query);
        }

        public async Task<TemplateDetailDto> GetById(Guid id)
        {
            var path = TemplateUri.GetTemplate(id);
            var result = await Get<TemplateDetailDto>(path);
            return result;
        }

        public async Task<PageResult<TemplateSummaryDto>> GetTemplatesForType(TemplateFor templateTarget)
        {
            var result = await QueryTemplates(TemplateQuery.NewQuery(t => t.TemplateType, templateTarget, ComparisonOperator.Equals));
            return result;
        }

        public async Task<TemplateDetailDto> CreateTemplate(AddTemplateDto addDto)
        {
            var path = TemplateUri.CreateTemplate;
            var result = await Post<TemplateDetailDto>(addDto, path);
            return result;
        }

        public async Task<TemplateDetailDto> UpdateTemplate(UpdateTemplateDto updateDto)
        {
            var path = TemplateUri.UpdateTemplate;
            var result = await Post<TemplateDetailDto>(updateDto, path);
            return result;
        }

        public async Task<bool> DeleteTemplate(Guid id)
        {
            var path = TemplateUri.DeleteTemplate(id);
            var result = await Delete(path);
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
