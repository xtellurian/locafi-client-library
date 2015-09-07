using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.Items;

namespace Locafi.Client.Model.Dto.Reader
{
    public class ClusterResponseDto
    {
        public Guid? PersonId { get; set; }
        public string PersonName { get; set; }

        public Guid PlaceId { get; set; }
        public string PlaceName { get; set; }

        public IList<ItemSummaryDto> Items { get; set; }
        public IList<ClusterTagDto> UnknownTags { get; set; }

        public ClusterResponseDto()
        {
            Items = new List<ItemSummaryDto>();
            UnknownTags = new List<ClusterTagDto>();
        }
    }
}
