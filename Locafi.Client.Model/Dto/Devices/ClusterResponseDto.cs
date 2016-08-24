using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.Items;

namespace Locafi.Client.Model.Dto.Devices
{
    // tells the client to do something after it uploads a cluster
    public class ClusterResponseDto
    {
        public Guid? PersonId { get; set; }
        public string PersonName { get; set; }

        public Guid PlaceId { get; set; }
        public string PlaceName { get; set; }

        public List<ItemSummaryDto> Items { get; set; }
        public List<ClusterTagDto> UnknownTags { get; set; }

        public ClusterResponseDto()
        {
            Items = new List<ItemSummaryDto>();
            UnknownTags = new List<ClusterTagDto>();
        }

        public ClusterResponseDto(ClusterResponseDto dto)
        {
            if (dto == null) return;

            var type = typeof(ClusterResponseDto);
            var properties = type.GetTypeInfo().DeclaredProperties;
            foreach (var property in properties)
            {
                var value = property.GetValue(dto);
                property.SetValue(this, value);
            }
        }
    }
}
