using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.Skus;

namespace Locafi.Client.Contract.Services
{
    public interface ISkuRepo
    {
        Task<IList<SkuSummaryDto>> GetAllSkus();
        Task<SkuDetailDto> GetSkuDetail(string skuId);
        Task<SkuDetailDto> CreateSku(AddSkuDto addSkuDto);
        Task Delete(Guid id);
    }
}