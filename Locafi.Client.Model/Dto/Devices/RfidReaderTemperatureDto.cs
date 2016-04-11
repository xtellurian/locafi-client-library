using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Devices
{
    public class RfidReaderTemperatureDto
    {
        public Guid RfidReaderId { get; set; }
        public double Temperature { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
