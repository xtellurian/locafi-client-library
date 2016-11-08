using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Portals
{
    public class AddDeviceDto
    {
        public string Name { get; set; }

        public Guid TemplateId { get; set; }

        public string DeviceType { get; set; }

        public string ConnectionType { get; set; }

        public string SerialNumber { get; set; }

        public IList<WriteEntityExtendedPropertyDto> DeviceExtendedPropertyList { get; set; }

        public IList<AddSensorDto> Sensors { get; set; }

        public IList<AddActuatorDto> Actuators { get; set; }

        public IList<AddAntennaDto> Antennas { get; set; }

        public AddDeviceDto()
        {
            DeviceExtendedPropertyList = new List<WriteEntityExtendedPropertyDto>();
            Sensors = new List<AddSensorDto>();
            Actuators = new List<AddActuatorDto>();
            Antennas = new List<AddAntennaDto>();
        }

    }
}
