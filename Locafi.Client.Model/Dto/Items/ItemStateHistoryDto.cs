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
    public class ItemStateHistoryDto
    {
        public Guid Id { get; set; }

        public Guid ItemId { get; set; }

        public string Name { get; set; }

        public Guid? TagId { get; set; }

        public string TagType { get; set; }

        public string TagNumber { get; set; }

        public Guid ReasonId { get; set; }

        public string ReasonNumber { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ItemStateType? State { get; set; }

        public DateTime ChangedOn { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public AgentType? AgentType { get; set; }

        public Guid? AgentId { get; set; }

        public string AgentName { get; set; }
    }
}
