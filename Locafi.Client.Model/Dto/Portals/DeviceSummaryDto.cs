using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Locafi.Client.Model.Dto.Portals
{
    public class DeviceSummaryDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public DeviceType DeviceType { get; set; }

        public string ConnectionType { get; set; }

        public string SerialNumber { get; set; }

        public Guid TemplateId { get; set; }

        public string TemplateName { get; set; }

    }
}
