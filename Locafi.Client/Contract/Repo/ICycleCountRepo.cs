using Locafi.Client.Model;
using Locafi.Client.Model.Dto.CycleCountDtos;
using Locafi.Client.Model.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Contract.Repo
{
    public interface ICycleCountRepo
    {
        Task<PageResult<CycleCountSummaryDto>> QueryCycleCounts(string oDataQueryOptions = null);
        Task<PageResult<CycleCountSummaryDto>> QueryCycleCounts(IRestQuery<CycleCountSummaryDto> query);
        Task<IQueryResult<CycleCountSummaryDto>> QueryCycleCountsWithContinuation(IRestQuery<CycleCountSummaryDto> query);
        Task<CycleCountDetailDto> GetCycleCount(Guid id);
        Task<CycleCountDetailDto> CreateCycleCount(AddCycleCountDto addDto);
        Task<ResolveCycleCountResultDto> ResolveCycleCount(ResolveCycleCountDto resolveDto);
    }
}
