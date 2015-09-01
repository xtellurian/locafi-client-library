using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Locafi.Client.Model.Dto.Reasons
{
    public class ReasonDetailDto : EntityDtoBase
    {
        public string ReasonNo { get; set; }

        public string Name { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public ReasonFor ReasonFor { get; set; }

        public override bool Equals(object obj)
        {
            var reason = obj as ReasonDetailDto;
            return reason != null && reason.Id == this.Id;
        }
    }
}
