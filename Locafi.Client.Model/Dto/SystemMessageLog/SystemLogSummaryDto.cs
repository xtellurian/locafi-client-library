using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Locafi.Client.Model.Dto.SystemMessageLog
{
    public class SystemLogSummaryDto
    {
        public Guid Id { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public SystemMessageType MessageType { get; set; }
        public string Message { get; set; }
        public DateTime TimeStamp { get; set; }
        public string LoggedByUserFullName { get; set; }
    }
}
