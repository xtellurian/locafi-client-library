using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Model.Dto.Skus;
using Locafi.Client.Services;

namespace Locafi.Client.Repo
{
    public class SkuRepo : WebRepo, ISkuRepo
    {
        public SkuRepo(IAuthorisedHttpTransferConfigService downloader, ISerialiserService entitySerialiser) : base(downloader, entitySerialiser, "Skus")
        {
        }

        public async Task<SkuDetailDto> CreateSku(AddSkuDto addSkuDto)
        {
            var path = "CreateSku";
            var result = await Post<SkuDetailDto>(addSkuDto, path);
            return result;
        }

        public async Task<IList<SkuSummaryDto>> GetAllSkus()
        {
            var path = "GetSkus";
            var result = await Get<IList<SkuSummaryDto>>(path);
            return result;
        }

        public async Task<SkuDetailDto> GetSkuDetail(Guid skuId)
        {
            var path = $"GetSkuDetail/{skuId}";
            var result = await Get<SkuDetailDto>(path);
            return result;
        }

        public async Task Delete(Guid id)
        {
            var path = $"{id}/Delete";
            await Delete(path);
        }

        protected async Task<IList<SkuSummaryDto>> QuerySkus(string filterString)
        {
            var path = $"GetSkus{filterString}";
            var result = await Get<IList<SkuSummaryDto>>(path);
            return result;
        }

    }
}
