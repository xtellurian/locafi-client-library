﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.Orders;
using Locafi.Client.Model.Query;

namespace Locafi.Client.Contract.Repo
{
    public interface IOrderRepo
    {
        Task<IList<OrderSummaryDto>> GetAllOrders();
        Task<OrderDetailDto> GetOrderById(Guid id);
        Task<IList<OrderSummaryDto>> QueryOrders(IRestQuery<OrderSummaryDto> query);

        Task<OrderDetailDto> Create(AddOrderDto orderDetail);
        Task<OrderActionResponseDto> Allocate(OrderSummaryDto orderDetail, Guid snapshotId);
        Task<OrderActionResponseDto> Receive(OrderSummaryDto order, Guid snapshotId);
        Task<OrderActionResponseDto> Dispatch(OrderSummaryDto orderDetail);


        Task<OrderActionResponseDto> DisputeAllocate(OrderSummaryDto orderSummary, OrderDisputeDto dispute, Guid snapshotId);
        Task<OrderActionResponseDto> DisputeDispatch(OrderSummaryDto orderSummary, OrderDisputeDto dispute);

        Task<OrderActionResponseDto> DisputeReceive(OrderSummaryDto order, Guid snapshotId,
            OrderDisputeDto dispute);

        Task<OrderActionResponseDto> Cancel(OrderSummaryDto order);
        Task DeleteOrder(Guid orderId);
    }
}