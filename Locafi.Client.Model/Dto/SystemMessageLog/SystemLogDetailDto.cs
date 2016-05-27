using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.SystemMessageLog
{
    public class SystemLogDetailDto : SystemLogSummaryDto
    {
        public Guid? LoggedById { get; set; }
    }
}
