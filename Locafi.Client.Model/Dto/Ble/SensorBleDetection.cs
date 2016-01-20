using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Ble
{
    public class SensorBleDetection : BleDetectionBase
    {
        public double? TemperatureInCelcius { get; set; }
        public double? LightInLumins { get; set; }
        public double? AccelerationInMetresPerSecond { get; set; }

    }
}
