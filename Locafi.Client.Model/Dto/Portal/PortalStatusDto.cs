using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.Devices;
using Locafi.Client.Model.Enums;

namespace Locafi.Client.Model.Dto.Portal
{
    public class PortalStatusDto
    {
        public Guid Id { get; set; }
        public RfidPortalStatus Status { get; set; }
        public DateTime TimeStamp { get; set; }
        public List<RfidReaderStatusDto> RfidReaderStatuses { get; set; }
    }
}
