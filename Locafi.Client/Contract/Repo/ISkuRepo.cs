using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.Skus;

namespace Locafi.Client.Contract.Repo
{
    public interface ISkuRepo
    {
        Task<IList<SkuSummaryDto>> GetAllSkus();
        Task<SkuDetailDto> GetSkuDetail(Guid skuId);
        Task<SkuDetailDto> CreateSku(AddSkuDto addSkuDto);
        Task Delete(Guid id);
    }
}