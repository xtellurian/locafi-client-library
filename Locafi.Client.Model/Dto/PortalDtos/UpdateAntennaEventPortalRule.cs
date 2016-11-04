using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.PortalDtos
{
    public class UpdateAntennaEventPortalRule : UpdatePortalRuleDto
    {
        public Guid PlaceInId { get; set; }
    }
}
