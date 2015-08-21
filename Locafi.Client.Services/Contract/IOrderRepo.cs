using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Data;

namespace Locafi.Client.Services.Contract
{
    public interface IOrderRepo
    {
        Task<IList<OrderDto>> GetAllOrders();
        Task<OrderDto> Allocate(OrderDto order, string snapshotId);
        Task<OrderDto> Receive(OrderDto order, string snapshotId);
        Task<OrderDto> Dispatch(OrderDto order);
        Task<OrderDto> Create(OrderDto order);
    }
}