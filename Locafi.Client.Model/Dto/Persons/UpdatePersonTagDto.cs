using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Locafi.Client.Model.Dto.Persons
{
    public class UpdatePersonTagDto
    {
        public Guid PersonId { get; set; }

        public string NewTagNumber { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public TagType NewTagType { get; set; } // TagType Enum

        public static UpdatePersonTagDto FromPerson(PersonSummaryDto detail, string newTagNumber, TagType newTagType = TagType.PassiveRfid)
        {
            return new UpdatePersonTagDto
            {
                PersonId = detail.Id,
                NewTagNumber = newTagNumber,
                NewTagType = newTagType
            };
        }
    }
}
