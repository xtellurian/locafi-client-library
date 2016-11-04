using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.PortalDtos
{
    public class AntennaEventPortalRuleDetailDto : PortalRuleDetailDto
    {
        public Guid PlaceInId { get; set; }

        public string PlaceInName { get; set; }
    }
}
