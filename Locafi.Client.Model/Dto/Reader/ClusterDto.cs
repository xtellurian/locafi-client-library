using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Reader
{
    public class ClusterDto
    {
        public IList<ClusterTagDto> Tags { get; set; }
        public Guid PlaceId { get; set; }
        public DateTime TimeStamp { get; set; }

        public ClusterDto()
        {
            Tags = new List<ClusterTagDto>();
        }
    }
}
