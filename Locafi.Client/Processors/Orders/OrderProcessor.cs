using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Processors;
using Locafi.Client.Model.Dto.Orders;
using Locafi.Client.Model.Dto.Snapshots;
using Locafi.Client.Model.RFID;
using Locafi.Client.Processors.Encoding;

namespace Locafi.Client.Processors.Orders.Processors
{
    public abstract class OrderProcessor
    {
        private readonly List<SnapshotTagDto> _snapshotTags;
        public OrderDetailDto OrderDetail { get; set; }

        protected OrderProcessor(OrderDetailDto orderDetail)
        {
            OrderDetail = orderDetail;
            _snapshotTags = new List<SnapshotTagDto>();
        }

        public IList<SnapshotTagDto> GetSnapshotTags()
        {
            return _snapshotTags;
        }

        public virtual IProcessTagResult Add(IRfidTag tag)
        {
            if(!_snapshotTags.Any(t=>string.Equals(t.TagNumber, tag.TagNumber))) _snapshotTags.Add(new SnapshotTagDto(tag.TagNumber));
            return null;
        }

        protected OrderSkuLineItemDto GetSkuLineItem(IRfidTag tag)
        {
            if (!Sgtin.IsSgtinTag(tag)) return null;
            var gtin = Sgtin.GetGtin(tag);
            return OrderDetail.ExpectedSkus.FirstOrDefault(s => string.Equals(s.Gtin, gtin));
        }

        protected OrderItemLineItemDto GetItemLineItem(IRfidTag tag)
        {
            return OrderDetail.ExpectedItems.FirstOrDefault(i => string.Equals(i.TagNumber, tag.TagNumber));
        }
    }
}
