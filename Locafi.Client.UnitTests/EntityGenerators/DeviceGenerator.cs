using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.Devices;
using Locafi.Client.Model.Enums;

namespace Locafi.Client.UnitTests.EntityGenerators
{
    public static class DeviceGenerator
    {
        public static AddRfidReaderDto CreateRandomRfidReader()
        {
            return new AddRfidReaderDto
            {
                Antennas = CreateRandomAntennae(),
                IsLdcEnabled = false,
                LdcFieldTimeout = 0,
                LdcPingInterval = 0,
                Name = "Random Rfid Reader",
                PeripherialDevice = CreateRandomPeripheralDevice(),
                PopulationEstimate = 32,
                ReaderMode = ReaderMode.AutoSetDenseReader,
                ReaderType = ReaderType.SpeedwayR440,
                SearchMode = SearchMode.DualTarget,
                Session = 1,
                SerialNumber = "0101010101"
            };
        }

        private static List<AddRfidReaderAntennaDto> CreateRandomAntennae()
        {
            return new List<AddRfidReaderAntennaDto>
            {
                new AddRfidReaderAntennaDto
                {
                    AntennaNumber = 1,
                    IsEnabled = true,
                    Name = "RandomAntenna",
                    TxPower = 27
                }
            };
        }

        public static AddPeripheralDeviceDto CreateRandomPeripheralDevice()
        {
            return new AddPeripheralDeviceDto
            {
                Actuators = CreateRandomActuators(),
                DeviceType = PeripheralDeviceType.SpeedwayR420,
                IpConfig = CreateRandomIpConfigDto(),
                Name = "Random Peripheral Device",
                Sensors = CreateRandomSensors()
            };
        }

        private static AddIpConfigDto CreateRandomIpConfigDto()
        {
            return new AddIpConfigDto
            {
                Hostname = "RandomDevice",
                IpAddress = "192.168.0.111",
                MacAddress = "ABCD123",
                SubnetMask = "255.255.255.0"
            };
        }

        private static List<AddPeripheralDeviceSensorDto> CreateRandomSensors()
        {
            return new List<AddPeripheralDeviceSensorDto>
            {
                new AddPeripheralDeviceSensorDto
                {
                    Name = "RandomSensor",
                    PortNo = 1
                }
            };
        }

        private static List<AddPeripheralDeviceActuatorDto> CreateRandomActuators()
        {
            return new List<AddPeripheralDeviceActuatorDto>
            {
                new AddPeripheralDeviceActuatorDto
                {
                    Name = "RandomActuator",
                    PortNo = 2
                }
            };
        }
    }
}
