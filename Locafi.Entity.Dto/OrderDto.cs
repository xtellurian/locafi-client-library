﻿using System;
using System.Collections.Generic;

namespace Locafi.Entity.Dto
{
    public class OrderDto
    {
        public string Id { get; set; }
        public string ReferenceNumber { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public string SourcePlaceId { get; set; }
        public string DestinationPlaceId { get; set; }
        public IList<string> SourceSnapshotIds { get; set; }
        public IList<string> DestinationSnapshotIds { get; set; }
        public IList<OrderSkuDetailDto> RequiredSkus { get; set; }
        public IList<OrderItemDetailDto> RequiredItems { get; set; }
        public string ServerMessage { get; set; }

        public string CreatedbyId { get; set; }
        public string LastModifiedById { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateLastModified { get; set; }
        public string DeliverToId { get; set; }

        public OrderDto()
        {
            ServerMessage = "";
        }
    }
}