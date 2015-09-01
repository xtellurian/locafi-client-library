﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Model.Dto.Orders;
using Locafi.Client.Model.Extensions;
using Locafi.Client.Services;

namespace Locafi.Client.Repo
{
    public class OrderRepo : WebRepo, IOrderRepo
    {
        public OrderRepo(IAuthorisedHttpTransferConfigService unauthorizedConfigService, ISerialiserService serialiser) : base(unauthorizedConfigService, serialiser, "orders")
        {
        }

        public async Task<IList<OrderDto>> GetAllOrders()
        {
            var result = await Get<IList<OrderDto>>();
            return result;
        }

        public async Task<OrderDto> GetOrderById(Guid id)
        {
            var result = await Get<OrderDto>(id.ToString());
            return result;
        }

        public async Task<OrderDto> Create(OrderDto order)
        {
            var path = order.CreateUri();
            var result = await Post<OrderDto>(order, path);

            return result;
        }

        public async Task<OrderDto> Allocate(OrderDto order, Guid snapshotId)
        {

            var path = order.AllocateUri(snapshotId);
            var result = await Post<OrderDto>(order, path);

            return result;
        }

        public async Task<OrderDto> Receive(OrderDto order, Guid snapshotId)
        {

            var path = order.ReceiveUri(snapshotId);
            var result = await Post<OrderDto>(order, path);

            return result;
        }

        public async Task<OrderDto> Dispatch(OrderDto order)
        {
            var path = order.DispatchUri();
            var result = await Post<OrderDto>(order, path);

            return result;
        }

    }
}
