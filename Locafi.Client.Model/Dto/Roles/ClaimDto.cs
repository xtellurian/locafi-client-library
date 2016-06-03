using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Locafi.Client.Model.Dto.Roles
{
    public class ClaimDto
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public NavigatorModule? ModuleName { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public DataAccessMode? Permission { get; set; }
    }
}
