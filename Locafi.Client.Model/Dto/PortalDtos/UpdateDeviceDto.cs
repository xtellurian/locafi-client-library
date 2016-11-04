using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.PortalDtos
{
    public class UpdateDeviceDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string ConnectionType { get; set; }

        public string SerialNumber { get; set; }

        public IList<WriteEntityExtendedPropertyDto> DeviceExtendedPropertyList { get; set; }

        public IList<UpdateSensorDto> Sensors { get; set; }

        public IList<UpdateActuatorDto> Actuators { get; set; }

        public IList<UpdateAntennaDto> Antennas { get; set; }

        public UpdateDeviceDto()
        {
            DeviceExtendedPropertyList = new List<WriteEntityExtendedPropertyDto>();
            Sensors = new List<UpdateSensorDto>();
            Actuators = new List<UpdateActuatorDto>();
            Antennas = new List<UpdateAntennaDto>();
        }
    }
}
