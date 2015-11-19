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
        public IList<Guid> AddSkuIds { get; set; }
        public IList<Guid> AddPlaceIds { get; set; }
        public IList<Guid> RemoveSkuIds { get; set; }
        public IList<Guid> RemovePlaceIds { get; set; }
        #endregion

        #region Methods
        public void AddSku(Guid skuId)
        {
            if(AddSkuIds==null) AddSkuIds = new List<Guid>();
            if (AddSkuIds.Contains(skuId)) return;
            AddSkuIds.Add(skuId);
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
            if(RemoveSkuIds==null) RemoveSkuIds = new List<Guid>();
            if (RemoveSkuIds.Contains(skuId)) return;
            RemoveSkuIds.Add(skuId);
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
            if(AddPlaceIds==null) AddPlaceIds = new List<Guid>();
            if (AddPlaceIds.Contains(placeId)) return;
            AddPlaceIds.Add(placeId);
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
            if(RemovePlaceIds==null) RemovePlaceIds = new List<Guid>();
            if(RemovePlaceIds.Contains(placeId)) return;
            RemovePlaceIds.Add(placeId);
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
