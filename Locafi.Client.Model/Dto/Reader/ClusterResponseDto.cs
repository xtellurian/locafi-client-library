using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.Items;

namespace Locafi.Client.Model.Dto.Reader
{
    public class ClusterReponseDto
    {
        public List<ItemSummaryDto> Items { get; set; }
        public List<ClusterTagDto> UnknownTags { get; set; }

        public ClusterReponseDto()
        {
            Items = new List<ItemSummaryDto>();
            UnknownTags = new List<ClusterTagDto>();
        }
    }
}
