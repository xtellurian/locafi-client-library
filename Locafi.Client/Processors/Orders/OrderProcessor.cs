using System.Collections.Generic;
using System.Linq;
using Locafi.Client.Contract.Processors;
using Locafi.Client.Model.Dto.Orders;
using Locafi.Client.Model.Dto.Snapshots;
using Locafi.Client.Model.RFID;
using Locafi.Client.Processors.Encoding;

namespace Locafi.Client.Processors.Orders
{
    public abstract class OrderProcessor
    {
        private readonly List<SnapshotTagDto> _snapshotAddTags;
        private readonly List<SnapshotTagDto> _snapshotRemoveTags;
        public OrderDetailDto OrderDetail { get; set; }

        protected OrderProcessor(OrderDetailDto orderDetail)
        {
            OrderDetail = orderDetail;
            _snapshotAddTags = new List<SnapshotTagDto>();
            _snapshotRemoveTags = new List<SnapshotTagDto>();
        }

        public IList<SnapshotTagDto> GetAddTags()
        {
            return _snapshotAddTags;
        }

        public IList<SnapshotTagDto> GetRemoveTags()
        {
            return _snapshotRemoveTags;
        } 

        public virtual IProcessTagResult Add(IRfidTag tag)
        {
            if(!_snapshotAddTags.Any(t=>string.Equals(t.TagNumber, tag.TagNumber))) _snapshotAddTags.Add(new SnapshotTagDto(tag.TagNumber));
            return null;
        }

        public virtual IProcessTagResult Remove(IRfidTag tag)
        {
            if (!_snapshotRemoveTags.Any(t => string.Equals(t.TagNumber, tag.TagNumber))) _snapshotRemoveTags.Add(new SnapshotTagDto(tag.TagNumber));
            return null;
        }

        protected OrderSkuLineItemDto GetSkuLineItem(IRfidTag tag)
        {
            if (!Sgtin.IsSgtinTag(tag)) return null;
            var gtin = Sgtin.GetGtin(tag);
            return OrderDetail.ExpectedSkus.FirstOrDefault(s => string.Equals(s.Gtin, gtin));
        }

        protected string GetGtin(IRfidTag tag)
        {
            return !Sgtin.IsSgtinTag(tag) ? null : Sgtin.GetGtin(tag);
        }

        protected OrderItemLineItemDto GetItemLineItem(IRfidTag tag)
        {
            return OrderDetail.ExpectedItems.FirstOrDefault(i => string.Equals(i.TagNumber, tag.TagNumber));
        }
    }
}
