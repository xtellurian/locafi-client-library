using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.Http;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Exceptions;
using Locafi.Client.Model.Dto.Orders;
using Locafi.Client.Model.Query;
using Locafi.Client.Model.RelativeUri;
using Locafi.Client.Model.Responses;
using Locafi.Client.Model.Uri;
using System.Collections;
using Locafi.Client.Model.Dto.Skus;
using Locafi.Client.Model;

namespace Locafi.Client.Repo
{
    public class OrderRepo : WebRepo, IOrderRepo
    {
        public OrderRepo(IAuthorisedHttpTransferConfigService configService, ISerialiserService serialiser)
            : base(new SimpleHttpTransferer(), configService, serialiser, OrderUri.ServiceName)
        {
        }

        public OrderRepo(IHttpTransferer transferer, IAuthorisedHttpTransferConfigService authorisedConfigService, ISerialiserService serialiser)
           : base(transferer, authorisedConfigService, serialiser, OrderUri.ServiceName)
        {
        }

        public async Task<PageResult<OrderSummaryDto>> QueryOrders(string oDataQueryOptions = null)
        {
            var path = OrderUri.GetOrders;

            // add the query options if required
            if (!string.IsNullOrEmpty(oDataQueryOptions))
            {
                if (oDataQueryOptions[0] != '?')
                    path += "?";

                path += oDataQueryOptions;
            }

            // make sure the query asks to return the item count
            if (!path.Contains("$count"))
            {
                if (path.Contains("?"))
                    path += "&$count=true";
                else
                    path += "?$count=true";
            }

            // run query
            var result = await Get<PageResult<OrderSummaryDto>>(path);
            return result;
        }

        public async Task<PageResult<OrderSummaryDto>> QueryOrders(IRestQuery<OrderSummaryDto> query)
        {
            return await QueryOrders(query.AsRestQuery());
        }

        public async Task<IQueryResult<OrderSummaryDto>> QueryOrdersContinuation(IRestQuery<OrderSummaryDto> query)
        {
            var result = await QueryOrders(query.AsRestQuery());
            return result.AsQueryResult(query);
        }

        public async Task<OrderDetailDto> GetOrderById(Guid id)
        {
            var path = OrderUri.GetOrder(id);
            var result = await Get<OrderDetailDto>(path);
            return result;
        }

        public async Task<IList<SkuDetailDto>> GetSkuPrintInfoById(Guid id)
        {
            return await Get<List<SkuDetailDto>>(OrderUri.GetPrintInfo(id));
        }

        public async Task<OrderDetailDto> Create(AddOrderDto addOrder)
        {
            var path = OrderUri.Create;
            var result = await Post<OrderDetailDto>(addOrder, path);
            return result;
        }

        public async Task<OrderDetailDto> Allocate(Guid orderId)
        {
            var path = OrderUri.Allocate(orderId);
            var result = await Post<OrderDetailDto>(null,path);
            return result;
        }

        public async Task<OrderDetailDto> RevertToAllocating(Guid orderId)
        {
            var path = OrderUri.RevertToAllocating(orderId);
            var result = await Post<OrderDetailDto>(null, path);
            return result;
        }

        public async Task<OrderDetailDto> Dispatch(Guid orderId)
        {
            var path = OrderUri.Dispatch(orderId);
            var result = await Post<OrderDetailDto>(null, path);
            return result;
        }

        public async Task<OrderDetailDto> Complete(Guid orderId)
        {
            var path = OrderUri.Complete(orderId);
            var result = await Post<OrderDetailDto>(null, path);
            return result;
        }

        public async Task<OrderDetailDto> Receive(Guid orderId)
        {
            var path = OrderUri.Receive(orderId);
            var result = await Post<OrderDetailDto>(null, path);
            return result;
        }

        public async Task<OrderDetailDto> RevertToReceiving(Guid orderId)
        {
            var path = OrderUri.RevertToReceiving(orderId);
            var result = await Post<OrderDetailDto>(null, path);
            return result;
        }

        public async Task<OrderDetailDto> Cancel(Guid orderId)
        {
            var path = OrderUri.Cancel(orderId);
            var result = await Post<OrderDetailDto>(null, path);
            return result;
        }

        public async Task<AddOrderSnapshotResultDto> AddSnapshotToOrder(AddOrderSnapshotDto orderSnapshotDto)
        {
            var path = OrderUri.AddSnapshotToOrder;
            var result = await Post<AddOrderSnapshotResultDto>(orderSnapshotDto, path);
            return result;
        }

        public override async Task Handle(HttpResponseMessage responseMessage, string url, string payload)
        {
            throw new OrderRepoException($"{url} -- {payload} -- " + await responseMessage.Content.ReadAsStringAsync());
        }

        public override Task Handle(IEnumerable<CustomResponseMessage> serverMessages, HttpStatusCode statusCode, string url, string payload)
        {
            throw new OrderRepoException(serverMessages, statusCode, url, payload);
        }
    }
}
