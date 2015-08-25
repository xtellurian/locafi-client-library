using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.Services;
using Locafi.Client.Data;
using Locafi.Client.Model.Dto.Skus;
using Locafi.Client.Services.Odata;

namespace Locafi.Client.Services.Repo
{
    public class SkuRepo : WebRepo, ISkuRepo
    {
        public SkuRepo(IAuthorisedHttpTransferConfigService downloader, ISerialiserService entitySerialiser) : base(downloader, entitySerialiser, "Skus")
        {
        }

        public async Task<IList<SkuSummaryDto>> GetAllSkus()
        {
            var path = "GetSkus";
            var result = await Get<IList<SkuSummaryDto>>(path);
            return result;
        }

        public async Task<SkuDetailDto> GetSkuDetail(string skuId)
        {
            var path = $"GetSkuDetail/{skuId}";
            var result = await Get<SkuDetailDto>(path);
            return result;
        }

        protected async Task<IList<SkuSummaryDto>> QuerySkus(string filterString)
        {
            var path = $"GetSkus{filterString}";
            var result = await Get<IList<SkuSummaryDto>>(path);
            return result;
        }

    }
}
