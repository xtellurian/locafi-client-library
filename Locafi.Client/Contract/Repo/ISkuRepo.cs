// This is a contract file
// Breaking changes to this contract should be accompanied by a version change in the third version number
// ie ... 1.2.42.11 -> 1.2.43.0

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.Skus;
using Locafi.Client.Model.Query;

namespace Locafi.Client.Contract.Repo
{
    public interface ISkuRepo
    {
        Task<IList<SkuSummaryDto>> GetAllSkus();
        Task<SkuDetailDto> GetSkuDetail(Guid skuId);
        Task<SkuDetailDto> CreateSku(AddSkuDto addSkuDto);
        Task Delete(Guid id);
        [Obsolete]
        Task<IList<SkuSummaryDto>> QuerySkus(IRestQuery<SkuSummaryDto> query);
        Task<IQueryResult<SkuSummaryDto>> QuerySkusAsync(IRestQuery<SkuSummaryDto> query);
    }
}