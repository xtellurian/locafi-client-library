using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Enums;
using Locafi.Client.Model.RFID;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Locafi.Client.Model.Dto.Snapshots
{
    public class SnapshotTagDto : IRfidTag
    {
        public string TagNumber { get; set; }   // tag number ie. EPC, barcode number, etc

        [JsonConverter(typeof(StringEnumConverter))]
        public TagType? TagType { get; set; }   // reference to the type of tag ie. passive UHF, barcode, RFCode, etc
        public int ReadCount { get; set; }  // number of times the tag was read during this inventory/allocation/receive etc
        public double Rssi { get; set; } // average RSSI of the tag during this inventory/allocation/receive etc

        public SnapshotTagDto()
        {
            TagType = Enums.TagType.PassiveRfid;
        }

        public SnapshotTagDto(string tagNumber) : this()
        {
            TagNumber = tagNumber;
        }

        public SnapshotTagDto(string tagNumber, TagType tagType) : this(tagNumber)
        {
            TagType = tagType;
        }

        public SnapshotTagDto(string tagNumber, int readCount, double averageRssi, TagType tagType = Enums.TagType.PassiveRfid): this (tagNumber, tagType)
        {
            ReadCount = readCount;
            Rssi = averageRssi;
        }

        public override bool Equals(object obj)
        {
            var tag = obj as SnapshotTagDto;
            return tag != null && string.Equals(tag.TagNumber, TagNumber, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return TagNumber.GetHashCode();
        }
    }
}
