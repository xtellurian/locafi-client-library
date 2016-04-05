using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Enums;

namespace Locafi.Client.Model.Dto.Devices
{
    public class RfidReaderAntennaStatusDto
    {
        public Guid Id { get; set; }
        public RfidReaderAntennaStatus Status { get; set; }
    }
}
