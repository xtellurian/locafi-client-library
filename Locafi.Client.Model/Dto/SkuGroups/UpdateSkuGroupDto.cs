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
            Id = skuGroupId;
            SkuGroupNameId = skuGroupNameId;
        }

        public UpdateSkuGroupDto(SkuGroupDetailDto detailDto)
        {
            Id = detailDto.Id;
            SkuGroupNameId = detailDto.SkuGroupNameId;
            SkuGroupSkus = detailDto.Skus.Select(s => s.Id).ToList();
            SkuGroupPlaces = detailDto.Places.Select(p => p.Id).ToList();
        }

        #region Properties
        public Guid Id { get; set; }    // Id of the sku group to modify
        public Guid? SkuGroupNameId { get; set; }    // set if you want to change the name of the group
        public IList<Guid> SkuGroupSkus { get; set; }
        public IList<Guid> SkuGroupPlaces { get; set; }
        #endregion

        #region Methods
        public void AddSku(Guid skuId)
        {
            if(SkuGroupSkus==null) SkuGroupSkus = new List<Guid>();
            if (SkuGroupSkus.Contains(skuId)) return;
            SkuGroupSkus.Add(skuId);
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
            if (SkuGroupSkus == null) return;
            if (!SkuGroupSkus.Contains(skuId)) return;
            SkuGroupSkus.Remove(skuId);
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
            if(SkuGroupPlaces==null) SkuGroupPlaces = new List<Guid>();
            if (SkuGroupPlaces.Contains(placeId)) return;
            SkuGroupPlaces.Add(placeId);
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
            if (SkuGroupPlaces == null) return;
            if(!SkuGroupPlaces.Contains(placeId)) return;
            SkuGroupPlaces.Remove(placeId);
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
