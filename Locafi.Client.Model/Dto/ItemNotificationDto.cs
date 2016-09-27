using Locafi.Client.Model.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto
{
    public class ItemNotificationDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public TagType TagType { get; set; }

        public string TagNumber { get; set; }

        public string ItemTypeName { get; set; }    // sku name

        [JsonConverter(typeof(StringEnumConverter))]
        public ItemStateType State { get; set; }

    }
}
