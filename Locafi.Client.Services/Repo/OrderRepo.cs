using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.Services;
using Locafi.Client.Data;
using Locafi.Client.Model.Actions;
using Locafi.Client.Model.Dto.Orders;
using Locafi.Client.Model.Uri;

namespace Locafi.Client.Services.Repo
{
    public class OrderRepo : WebRepo<OrderDto>, IOrderRepo
    {
        public OrderRepo(IHttpTransferConfigService configService, ISerialiserService serialiser) : base(configService, serialiser, "orders")
        {
        }

        public async Task<IList<OrderDto>> GetAllOrders()
        {
            return await base.GetList();
        }

        public async Task<OrderDto> Create(OrderDto order)
        {
            var path = order.RelativeUri(OrderAction.Create, null);
            var result = await Post(order, path);

            return result;
        }

        public async Task<OrderDto> Allocate(OrderDto order, string snapshotId)
        {

            var path = order.RelativeUri(OrderAction.Allocate, snapshotId);
            var result = await Post(order, path);

            return result;
        }

        public async Task<OrderDto> Receive(OrderDto order, string snapshotId)
        {

            var path = order.RelativeUri(OrderAction.Receive, snapshotId);
            var result = await Post(order, path);

            return result;
        }

        public async Task<OrderDto> Dispatch(OrderDto order)
        {
            var path = order.RelativeUri(OrderAction.Dispatch, null);
            var result = await Post(order, path);

            return result;
        }

    }
}
