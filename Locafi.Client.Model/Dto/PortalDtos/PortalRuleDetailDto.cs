﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.PortalDtos
{
    public class PortalRuleDetailDto : PortalRuleSummaryDto
    {
        public IList<Guid> Antennas { get; set; }
    }
}
