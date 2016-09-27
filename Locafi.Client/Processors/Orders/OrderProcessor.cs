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
            // make sure all of our skus have a valid gtin
            foreach(var sku in OrderDetail.OrderSkuList)
            {
                if(string.IsNullOrEmpty(sku.Gtin))
                {
                    sku.Gtin = Sgtin.GetGtin(sku.CompanyPrefix, sku.ItemReference);
                }
            }
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

        protected ReadOrderSkuDto GetExpectedSkuLineItem(IRfidTag tag)
        {
            if (!tag.HasSgtin()) return null;
            var gtin = tag.GetGtin();
            return OrderDetail.OrderSkuList.FirstOrDefault(s => string.Equals(s.Gtin, gtin));
        }

        protected ReadOrderSkuDto GetAdditionalSkuLineItem(IRfidTag tag)
        {
            if (!tag.HasSgtin()) return null;
            var gtin = tag.GetGtin();
            return OrderDetail.OrderSkuList.FirstOrDefault(s => string.Equals(s.Gtin, gtin));
        }

        protected string GetGtin(IRfidTag tag)
        {
            return tag.GetGtin();
        }

        protected ReadOrderItemDto GetItemLineItem(IRfidTag tag)
        {
            return OrderDetail.OrderItemList.FirstOrDefault(i => string.Equals(i.TagNumber, tag.TagNumber));
        }
    }
}
