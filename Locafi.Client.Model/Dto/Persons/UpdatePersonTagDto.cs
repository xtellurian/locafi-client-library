using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Locafi.Client.Model.Dto.Tags;

namespace Locafi.Client.Model.Dto.Persons
{
    public class UpdatePersonTagDto
    {
        public Guid Id { get; set; }

        public IList<WriteTagDto> PersonTagList { get; set; }

        public static UpdatePersonTagDto FromPerson(PersonSummaryDto detail, string newTagNumber, TagType newTagType = TagType.PassiveRfid)
        {
            var dto = new UpdatePersonTagDto
            {
                Id = detail.Id
            };
            dto.PersonTagList.Add(new WriteTagDto() { TagNumber = newTagNumber, TagType = newTagType });
            return dto;
        }
    }
}
