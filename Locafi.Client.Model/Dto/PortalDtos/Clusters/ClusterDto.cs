using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.PortalDtos.Clusters
{
    public class ClusterDto
    {
        public IList<ClusterTagDto> Tags { get; set; }

        public Guid? PlaceId { get; set; }

        public string RuleType { get; set; }

        public string TagNumber { get; set; }

        public ClusterDto()
        {
            Tags = new List<ClusterTagDto>();
        }

    }
}
