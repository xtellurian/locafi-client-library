using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.PortalDtos
{
    public class RfidPortalSummaryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string SerialNumber { get; set; }
        public int MaxRfidReaders { get; set; }
        public int MaxPeripheralDevices { get; set; }
    }
}
