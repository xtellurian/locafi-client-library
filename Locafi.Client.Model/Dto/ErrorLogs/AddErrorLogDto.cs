using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Locafi.Client.Model.Dto.ErrorLogs
{
    public class AddErrorLogDto
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public ErrorLevel ErrorLevel { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorDetails { get; set; }
        public DateTime TimeStamp { get; set; }
    }

}
