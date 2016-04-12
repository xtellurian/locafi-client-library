using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Enums;

namespace Locafi.Client.Model.Dto.Devices
{
    public class RfidReaderStatusDto
    {
        public Guid Id { get; set; }
        public RfidReaderStatus Status { get; set; }
        public List<RfidReaderAntennaStatusDto> AntennaStatuses { get; set; }

        //TODO: Implement Sensors and Actuators
        //public List<PeripheralDeviceStatusDto> SensorStatuses { get; set; }
        //public List<PeripheralDeviceStatusDto> ActuatorStatuses { get; set; }

        public RfidReaderStatusDto()
        {
            AntennaStatuses = new List<RfidReaderAntennaStatusDto>();
        }
    }
}
