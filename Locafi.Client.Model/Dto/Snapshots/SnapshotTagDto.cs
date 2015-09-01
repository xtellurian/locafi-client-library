using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Locafi.Client.Model.Dto.Snapshots
{
    public class SnapshotTagDto
    {
        public string TagNumber { get; set; }   // tag number ie. EPC, barcode number, etc

        [JsonConverter(typeof(StringEnumConverter))]
        public TagType TagType { get; set; }   // reference to the type of tag ie. passive UHF, barcode, RFCode, etc

        public SnapshotTagDto()
        {
            TagType = TagType.PassiveRfid;
        }

        public override bool Equals(object obj)
        {
            var tag = obj as SnapshotTagDto;
            return tag != null && string.Equals(tag.TagNumber, TagNumber);
        }
    }
}
