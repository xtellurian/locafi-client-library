using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Locafi.Client.Contract.Errors;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Exceptions;
using Locafi.Client.Model.ConfigurationContract;
using Locafi.Client.Model.Dto.Skus;
using Locafi.Client.Model.Responses;
using Locafi.Client.Model.Uri;

namespace Locafi.Client.Repo
{
    public class SkuRepo : WebRepo, ISkuRepo, IWebRepoErrorHandler
    {
        public SkuRepo(IAuthorisedHttpTransferConfigService downloader, ISerialiserService entitySerialiser)
            : base(downloader, entitySerialiser, SkuUri.ServiceName)
        {
        }

        public async Task<SkuDetailDto> CreateSku(AddSkuDto addSkuDto)
        {
            var path = SkuUri.CreateSku;
            var result = await Post<SkuDetailDto>(addSkuDto, path);
            return result;
        }

        public async Task<IList<SkuSummaryDto>> GetAllSkus()
        {
            var path = SkuUri.GetSkus;
            var result = await Get<IList<SkuSummaryDto>>(path);
            return result;
        }

        public async Task<SkuDetailDto> GetSkuDetail(Guid skuId)
        {
            var path = SkuUri.GetSkuDetail(skuId);
            var result = await Get<SkuDetailDto>(path);
            return result;
        }

        public async Task Delete(Guid id)
        {
            var path = SkuUri.DeleteSku(id);
            await Delete(path);
        }

        protected async Task<IList<SkuSummaryDto>> QuerySkus(string filterString)
        {
            var path = $"{SkuUri.GetSkus}{filterString}";
            var result = await Get<IList<SkuSummaryDto>>(path);
            return result;
        }

        public override async Task Handle(HttpResponseMessage responseMessage)
        {
            throw new SkuRepoException(await responseMessage.Content.ReadAsStringAsync());
        }

        public override Task Handle(IEnumerable<CustomResponseMessage> serverMessages, HttpStatusCode statusCode)
        {
            throw new SkuRepoException(serverMessages, statusCode);
        }
    }
}
