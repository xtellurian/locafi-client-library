using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Data;
using Locafi.Client.Model.Dto.Orders;

namespace Locafi.Client.Contract.Services
{
    public interface IOrderRepo
    {
        Task<IList<OrderDto>> GetAllOrders();
        Task<OrderDto> Allocate(OrderDto order, Guid snapshotId);
        Task<OrderDto> Receive(OrderDto order, Guid snapshotId);
        Task<OrderDto> Dispatch(OrderDto order);
        Task<OrderDto> Create(OrderDto order);
        Task<OrderDto> GetOrderById(Guid id);
    }
}