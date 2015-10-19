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
    public class RfidReaderSummaryDto : EntityDtoBase
    {
        public string Name { get; set; }
        public string IpAddress { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public ReaderType ReaderType { get; set; }  // ReaderType Enum
        public string SerialNumber { get; set; }
    }
}
