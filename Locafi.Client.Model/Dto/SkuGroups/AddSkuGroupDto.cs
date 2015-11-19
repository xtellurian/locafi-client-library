using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.Places;
using Locafi.Client.Model.Dto.Skus;

namespace Locafi.Client.Model.Dto.SkuGroups
{
    public class AddSkuGroupDto
    {
        public AddSkuGroupDto()
        {
            
        }

        public AddSkuGroupDto(Guid skuGroupNameId)
        {
            SkuGroupNameId = skuGroupNameId;
            SkuIds = new List<Guid>();
            PlaceIds = new List<Guid>();
        }

        public void AddSkus(IEnumerable<SkuSummaryDto> skuSummaries)
        {
            foreach (var sku in skuSummaries)
            {
                AddSku(sku.Id);
            }
        }

        public void AddSku(Guid skuId)
        {
            if(SkuIds==null) SkuIds = new List<Guid>();
            if (SkuIds.Contains(skuId)) return;
            SkuIds.Add(skuId);
        }

        public void AddPlaces(IEnumerable<PlaceSummaryDto> places)
        {
            foreach (var place in places)
            {
                AddPlace(place.Id);
            }
        }

        public void AddPlace(Guid placeId)
        {
            if(PlaceIds==null) PlaceIds = new List<Guid>();
            if (PlaceIds.Contains(placeId)) return;
            PlaceIds.Add(placeId);
        }

        public Guid SkuGroupNameId { get; set; }
        public IList<Guid> SkuIds { get; set; }
        public IList<Guid> PlaceIds { get; set; }
    }
}
