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
        Task<OrderActionResponseDto> Allocate(OrderSummaryDto orderDetail, Guid snapshotId);
        Task<OrderActionResponseDto> Receive(OrderSummaryDto order, Guid snapshotId);
        Task<OrderActionResponseDto> Dispatch(OrderSummaryDto orderDetail);
        Task<OrderActionResponseDto> Complete(OrderSummaryDto orderDetail);

        Task<OrderActionResponseDto> DisputeAllocate(OrderSummaryDto orderSummary, OrderDisputeDto dispute, Guid snapshotId);
        Task<OrderActionResponseDto> DisputeDispatch(OrderSummaryDto orderSummary, OrderDisputeDto dispute);

        Task<OrderActionResponseDto> DisputeReceive(OrderSummaryDto order, Guid snapshotId,
            OrderDisputeDto dispute);

        Task<OrderActionResponseDto> Cancel(OrderSummaryDto order);
        Task<bool> DeleteOrder(Guid orderId);
    }
}