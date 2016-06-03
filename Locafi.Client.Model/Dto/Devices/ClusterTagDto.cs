using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Locafi.Client.Model.Dto.Devices
{
    public class ClusterTagDto
    {
        public string TagNumber { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public TagType? TagType { get; set; }

        public int ReadCount { get; set; }  // number of times the tag was read during this inventory/allocation/receive etc

        public double AverageRssi { get; set; } // average RSSI of the tag during this inventory/allocation/receive etc

        public DateTime ReadTime { get; set; } //time of first read
    }
}
