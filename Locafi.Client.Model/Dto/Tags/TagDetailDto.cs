using Locafi.Client.Model.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Tags
{
    public class TagDetailDto
    {
        public Guid Id { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public TagType TagType { get; set; }

        public string TagNumber { get; set; }
    }
}
