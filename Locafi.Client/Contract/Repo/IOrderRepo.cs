// This is a contract file
// Breaking changes to this contract should be accompanied by a version change in the third version number
// ie ... 1.2.42.11 -> 1.2.43.0

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.Orders;
using Locafi.Client.Model.Query;
using Locafi.Client.Model.Dto.Skus;
using Locafi.Client.Model;

namespace Locafi.Client.Contract.Repo
{
    public interface IOrderRepo
    {
        Task<PageResult<OrderSummaryDto>> QueryOrders(string oDataQueryOptions = null);
        Task<PageResult<OrderSummaryDto>> QueryOrders(IRestQuery<OrderSummaryDto> query);
        Task<IQueryResult<OrderSummaryDto>> QueryOrdersContinuation(IRestQuery<OrderSummaryDto> query);
        Task<OrderDetailDto> GetOrderById(Guid id);
        Task<IList<SkuDetailDto>> GetSkuPrintInfoById(Guid id);

        Task<OrderDetailDto> Create(AddOrderDto orderDetail);

        Task<OrderDetailDto> Allocate(Guid orderId);
        Task<OrderDetailDto> RevertToAllocating(Guid orderId);

        Task<OrderDetailDto> Dispatch(Guid orderId);

        Task<OrderDetailDto> Receive(Guid orderId);
        Task<OrderDetailDto> RevertToReceiving(Guid orderId);

        Task<OrderDetailDto> Complete(Guid orderId);

        Task<OrderDetailDto> Cancel(Guid orderId);

        Task<AddOrderSnapshotResultDto> AddSnapshotToOrder(AddOrderSnapshotDto orderSnapshotDto);
    }
}