using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Locafi.Client.Model.Dto.Portals.Clusters
{
    public class ClusterDto : ICacheable
    {
        public IList<ClusterTagDto> Tags { get; set; }

        public Guid? PlaceId { get; set; }

        public string PlaceTagNumber { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public PortalRuleType RuleType { get; set; }

        public ClusterDto()
        {
            Id = new Guid().ToString();
            Tags = new List<ClusterTagDto>();
        }

        public string Id { get; set; }
    }
}
