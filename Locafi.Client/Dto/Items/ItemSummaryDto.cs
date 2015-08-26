using System;

namespace Locafi.Client.Model.Dto.Items
{
    public class ItemSummaryDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid SkuId { get; set; }
        public string SkuName { get; set; }

        public Guid PlaceId { get; set; }
        public string PlaceName { get; set; }

        public string TagNumber { get; set; }
        public string TagTypeName { get; set; }

        public Guid? CreatedByUserId { get; set; }

        public string CreatedByUserFullName { get; set; }

        public DateTime DateCreated { get; set; }

        public Guid? LastModifiedByUserId { get; set; }

        public string LastModifiedByUserFullName { get; set; }

        public DateTime? DateLastModified { get; set; }

        public override bool Equals(object obj)
        {
            var itemSummary = obj as ItemSummaryDto;
            return itemSummary != null && itemSummary.Id.Equals(Id);
        }
    }
}