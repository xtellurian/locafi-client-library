using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.PortalDtos
{
    public class DeviceDetailDto : DeviceSummaryDto
    {

        public IList<ReadEntityExtendedPropertyDto> DeviceProperties { get; set; }

        public IList<ReadSensorDto> Sensors { get; set; }

        public IList<ReadActuatorDto> Actuators { get; set; }

        public IList<ReadAntennaDto> Antennas { get; set; }

        public DeviceDetailDto()
        {
            DeviceProperties = new List<ReadEntityExtendedPropertyDto>();
            Sensors = new List<ReadSensorDto>();
            Actuators = new List<ReadActuatorDto>();
            Antennas = new List<ReadAntennaDto>();
        }

    }
}
