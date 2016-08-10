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
using Locafi.Client.Model.Query;
using Locafi.Client.Model.Responses;
using Locafi.Client.Model.Uri;
using Locafi.Client.Model;
using Locafi.Client.Model.Dto.ExtendedProperties;

namespace Locafi.Client.Repo
{
    public class ExtendedPropertyRepo : WebRepo, IExtendedPropertyRepo
    {

        public ExtendedPropertyRepo(IAuthorisedHttpTransferConfigService authorisedUnauthorizedConfigService, ISerialiserService serialiser) 
            : base(new SimpleHttpTransferer(), authorisedUnauthorizedConfigService, serialiser, ExtendedPropertyUri.ServiceName)
        {
        }

        public ExtendedPropertyRepo(IHttpTransferer transferer, IAuthorisedHttpTransferConfigService authorisedUnauthorizedConfigService, ISerialiserService serialiser)
           : base(transferer, authorisedUnauthorizedConfigService, serialiser, ExtendedPropertyUri.ServiceName)
        {
        }

        public async Task<PageResult<ExtendedPropertySummaryDto>> QueryExtendedProperties(string oDataQueryOptions = null)
        {
            var path = ExtendedPropertyUri.GetExtendedProperties;

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
            var result = await Get<PageResult<ExtendedPropertySummaryDto>>(path);
            return result;
        }

        public async Task<PageResult<ExtendedPropertySummaryDto>> QueryExtendedProperties(IRestQuery<ExtendedPropertySummaryDto> query)
        {
            return await QueryExtendedProperties(query.AsRestQuery());
        }

        public async Task<IQueryResult<ExtendedPropertySummaryDto>> QueryExtendedPropertiesContinuation(IRestQuery<ExtendedPropertySummaryDto> query)
        {
            var results = await QueryExtendedProperties(query.AsRestQuery());
            return results.AsQueryResult(query);
        }

        public async Task<ExtendedPropertyDetailDto> CreateExtendedProperty(AddExtendedPropertyDto addDto)
        {
            var path = ExtendedPropertyUri.CreateExtendedProperty;
            var result = await Post<ExtendedPropertyDetailDto>(addDto, path);
            return result;
        }

        public async Task<ExtendedPropertyDetailDto> UpdateExtendedProperty(UpdateExtendedPropertyDto updateDto)
        {
            var path = ExtendedPropertyUri.UpdateExtendedProperty;
            var result = await Post<ExtendedPropertyDetailDto>(updateDto, path);
            return result;
        }

        public async Task<ExtendedPropertyDetailDto> GetExtendedPropertyById(Guid id)
        {
            var path = ExtendedPropertyUri.GetExtendedProperty(id);
            var result = await Get<ExtendedPropertyDetailDto>(path);
            return result;
        }       

        public async Task<bool> DeleteExtendedProperty(Guid id)
        {
            var path = ExtendedPropertyUri.DeleteExtendedProperty(id);
            return await Delete(path);
        }

        public override async Task Handle(HttpResponseMessage responseMessage, string url, string payload)
        {
            throw new ExtendedPropertyRepoException($"{url} -- {payload} -- " + await responseMessage.Content.ReadAsStringAsync());
        }

        public override Task Handle(IEnumerable<CustomResponseMessage> serverMessages, HttpStatusCode statusCode, string url, string payload)
        {
            throw new ExtendedPropertyRepoException(serverMessages, statusCode, url, payload);
        }
    }
}
