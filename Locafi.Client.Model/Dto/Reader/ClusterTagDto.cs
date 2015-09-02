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
    public class ClusterTagDto
    {
        public string TagNumber { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public TagType TagType { get; set; }
    }
}
