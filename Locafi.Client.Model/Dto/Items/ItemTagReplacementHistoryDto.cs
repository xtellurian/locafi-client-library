using Locafi.Client.Model.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Items
{
    public class ItemTagReplacementHistoryDto
    {
        public Guid Id { get; set; }

        public Guid ItemId { get; set; }

        public string Name { get; set; }

        public Guid OldTagId { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public TagType OldTagType { get; set; }

        public string OldTagNumber { get; set; }

        public Guid NewTagId { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public TagType NewTagType { get; set; }

        public string NewTagNumber { get; set; }

        public DateTimeOffset DateReplaced { get; set; }

        public Guid? ReplacedByUserId { get; set; }

        public string ReplacedByUserFullName { get; set; }

    }
}
