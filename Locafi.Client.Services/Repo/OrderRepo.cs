using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Action;
using Locafi.Client.Data;
using Locafi.Client.Services.Contract;
using Locafi.Client.Uri;

namespace Locafi.Client.Services.Repo
{
    public class OrderRepo : WebRepoBase<OrderDto>, IOrderRepo
    {
        public OrderRepo(IHttpTransferConfigService configService, ISerialiserService serialiser) : base(configService, serialiser, "orders")
        {
        }

        public async Task<IList<OrderDto>> GetAllOrders()
        {
            return await base.GetList();
        }

        public async Task<IList<OrderDto>> GetOrders(string extra)
        {
            return await base.GetList(extra);
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
