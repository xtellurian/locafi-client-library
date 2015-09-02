using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Locafi.Client.Model.Dto.Reader
{
    public class RssiConfigDto
    {
        public double RssiMin { get; set; }

        public double RssiMax { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public TagFor TagFor { get; set; }  // TagFor Enum
    }
}
