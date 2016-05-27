using System;
using System.Collections.Generic;
using Locafi.Client.Model.Dto.Devices;

namespace Locafi.Client.Model.Dto.Portals
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
