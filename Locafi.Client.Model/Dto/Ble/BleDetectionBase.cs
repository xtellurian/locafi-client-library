using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Ble
{
    public abstract class BleDetectionBase : IBleDetection
    {
        public string TagNumber { get; set; }
        public DateTime Timestamp { get; set; }
        public string DetectedBy { get; set; }
        public string LocationGeoHash { get; set; }
        public double? Lat { get; set; }
        public double? Long { get; set; }
        public double? AverageRssi { get; set; }
    }
}
