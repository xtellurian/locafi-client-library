using System.Collections.Generic;
using Locafi.Client.Model.Dto.Devices;

namespace Locafi.Client.Model.Dto.Portals
{
    public class PortalDetailDto : PortalSummaryDto
    {
        public PortalDetailDto()
        {
            PeripheralDevices = new List<PeripheralDeviceDetailDto>();
            RfidReaders = new List<RfidReaderDetailDto>();
        }

        public IList<PeripheralDeviceDetailDto> PeripheralDevices { get; set; }
        public IList<RfidReaderDetailDto> RfidReaders { get; set; }
    }
}
