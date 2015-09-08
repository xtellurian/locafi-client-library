using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.Errors;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Exceptions;
using Locafi.Client.Model.Dto.Orders;
using Locafi.Client.Model.Query;
using Locafi.Client.Model.RelativeUri;
using Locafi.Client.Model.Responses;

namespace Locafi.Client.Repo
{
    public class OrderRepo : WebRepo, IOrderRepo, IWebRepoErrorHandler
    {
        public OrderRepo(IAuthorisedHttpTransferConfigService unauthorizedConfigService, ISerialiserService serialiser) 
            : base(unauthorizedConfigService, serialiser, OrderUri.ServiceName)
        {
        }

        public async Task<IList<OrderSummaryDto>> GetAllOrders()
        {
            var path = OrderUri.GetOrders;
            var result = await Get<IList<OrderSummaryDto>>(path);
            return result;
        }

        public async Task<OrderDetailDto> GetOrderById(Guid id)
        {
            var path = OrderUri.GetOrder(id);
            var result = await Get<OrderDetailDto>(path);
            return result;
        }

        public async Task<IList<OrderSummaryDto>> QueryOrders(IRestQuery<OrderSummaryDto> query)
        {
            return await QueryOrders(query.AsRestQuery());
        }

        public async Task<OrderDetailDto> Create(AddOrderDto addOrder)
        {
            var path = OrderUri.Create;
            var result = await Post<OrderDetailDto>(addOrder, path);
            return result;
        }

        public async Task<OrderActionResponseDto> Allocate(OrderSummaryDto orderSummary, Guid snapshotId)
        {
            var path = OrderUri.Allocate(orderSummary, snapshotId);
            var result = await Post<OrderActionResponseDto>(orderSummary, path);
            return result;
        }

        public async Task<OrderActionResponseDto> DisputeAllocate(OrderSummaryDto orderSummary, OrderDisputeDto dispute, Guid snapshotId)
        {
            var path = OrderUri.DisputeAllocate(orderSummary, snapshotId);
            var result = await Post<OrderActionResponseDto>(dispute , path);
            return result;
        }

        public async Task<OrderActionResponseDto> Dispatch(OrderSummaryDto orderDetail)
        {
            var path = OrderUri.Dispatch(orderDetail);
            var result = await Post<OrderActionResponseDto>(orderDetail, path);
            return result;
        }

        public async Task<OrderActionResponseDto> DisputeDispatch(OrderSummaryDto orderSummary, OrderDisputeDto dispute)
        {
            var path = OrderUri.DisputeDispatch(orderSummary);
            var result = await Post<OrderActionResponseDto>(dispute, path);
            return result;
        }

        public async Task<OrderActionResponseDto> Receive(OrderSummaryDto order, Guid snapshotId)
        {
            var path = OrderUri.Receive(order, snapshotId);
            var result = await Post<OrderActionResponseDto>(order, path);
            return result;
        }

        public async Task<OrderActionResponseDto> DisputeReceive(OrderSummaryDto order, Guid snapshotId,
            OrderDisputeDto dispute)
        {
            var path = OrderUri.DisputeReceive(order, snapshotId);
            var result = await Post<OrderActionResponseDto>(dispute, path);
            return result;
        }

        public async Task<OrderActionResponseDto> Cancel(OrderSummaryDto order)
        {
            var path = OrderUri.Cancel(order);
            var result = await Post<OrderActionResponseDto>(null, path);
            return result;
        }

        public async Task DeleteOrder(Guid orderId)
        {
            var path = OrderUri.Delete(orderId);
            await Delete(path);
        }

        protected async Task<IList<OrderSummaryDto>> QueryOrders(string queryString)
        {
            var path = $"{OrderUri.GetOrders}/{queryString}";
            var result = await Get<IList<OrderSummaryDto>>(path);
            return result;
        }

        public override async Task Handle(HttpResponseMessage responseMessage)
        {
            throw new OrderException(await responseMessage.Content.ReadAsStringAsync());
        }

        public override Task Handle(IEnumerable<CustomResponseMessage> serverMessages, HttpStatusCode statusCode)
        {
            throw new OrderException(serverMessages, statusCode);
        }
    }
}
