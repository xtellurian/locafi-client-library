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
    public class WriteTagDto
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public TagType? TagType { get; set; }

        private string _tagNumber;
        public string TagNumber
        {
            get { return _tagNumber; }
            set { _tagNumber = value?.ToUpper(); }
        }
    }
}
