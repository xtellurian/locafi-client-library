using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.PortalDtos.Clusters
{
    public class ClusterTagDto
    {
        public string TagNumber { get; set; }

        public string TagType { get; set; }

        public int ReadCount { get; set; }

        public double AverageRssi { get; set; }

        public DateTime ReadTime { get; set; }

    }
}
