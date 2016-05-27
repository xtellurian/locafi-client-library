using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.Places;
using Locafi.Client.Model.Dto.Skus;

namespace Locafi.Client.Model.Dto.SkuGroups
{
    public class UpdateSkuGroupDto
    {
        public UpdateSkuGroupDto()
        {
            
        }

        public UpdateSkuGroupDto(Guid skuGroupId, Guid? skuGroupNameId = null)
        {
            SkuGroupId = skuGroupId;
            SkuGroupNameId = skuGroupNameId;
        }

        #region Properties
        public Guid SkuGroupId { get; set; }    // Id of the sku group to modify
        public Guid? SkuGroupNameId { get; set; }    // set if you want to change the name of the group
        public IList<Guid> SkuIds { get; set; }
        public IList<Guid> PlaceIds { get; set; }
        #endregion

        #region Methods
        public void AddSku(Guid skuId)
        {
            if(SkuIds==null) SkuIds = new List<Guid>();
            if (SkuIds.Contains(skuId)) return;
            SkuIds.Add(skuId);
        }

        public void AddSkus(IEnumerable<SkuSummaryDto> skuSummaries)
        {
            foreach (var sku in skuSummaries)
            {
                AddSku(sku.Id);
            }
        }

        public void RemoveSku(Guid skuId)
        {
            if (SkuIds == null) return;
            if (!SkuIds.Contains(skuId)) return;
            SkuIds.Remove(skuId);
        }

        public void RemoveSkus(IEnumerable<SkuSummaryDto> skuSummaries)
        {
            foreach (var sku in skuSummaries)
            {
                RemoveSku(sku.Id);
            }
        }

        public void AddPlace(Guid placeId)
        {
            if(PlaceIds==null) PlaceIds = new List<Guid>();
            if (PlaceIds.Contains(placeId)) return;
            PlaceIds.Add(placeId);
        }

        public void AddPlaces(IEnumerable<PlaceSummaryDto> placeSummaries)
        {
            foreach (var place in placeSummaries)
            {
                AddPlace(place.Id);
            }
        }

        public void RemovePlace(Guid placeId)
        {
            if (PlaceIds == null) return;
            if(!PlaceIds.Contains(placeId)) return;
            PlaceIds.Remove(placeId);
        }

        public void RemovePlaces(IEnumerable<PlaceSummaryDto> places)
        {
            foreach (var place in places)
            {
                RemovePlace(place.Id);
            }
        }
#endregion
    }
}
