using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.Devices;

namespace Locafi.Client.Model.Dto.Portal
{
    public class PortalHeartbeatDto
    {
        public Guid RfidPortalId { get; set; }
        public IList<RfidReaderTemperatureDto> RfidReaderTemperatures { get; set; }

        public PortalHeartbeatDto()
        {
            RfidReaderTemperatures = new List<RfidReaderTemperatureDto>();
        }
    }
}
