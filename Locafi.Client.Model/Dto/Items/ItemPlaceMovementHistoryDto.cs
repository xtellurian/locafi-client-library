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
    public class ItemPlaceMovementHistoryDto
    {
        public Guid Id { get; set; }

        public Guid ItemId { get; set; }

        public string Name { get; set; }

        public Guid? PersonId { get; set; }

        public string PersonName { get; set; }

        public Guid OldPlaceId { get; set; }

        public string OldPlaceName { get; set; }

        public Guid NewPlaceId { get; set; }

        public string NewPlaceName { get; set; }

        public Guid? TagId { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public TagType? TagType { get; set; }

        public string TagNumber { get; set; }

        public DateTime DateMoved { get; set; }

        public Guid? MovedByUserId { get; set; }

        public string MovedByUserFullName { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public AgentType? AgentType { get; set; }

        public Guid? AgentId { get; set; }

        public string AgentName { get; set; }

    }
}
