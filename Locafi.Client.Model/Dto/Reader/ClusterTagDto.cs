using Locafi.Client.Model.Enums;
using Locafi.Client.Model.RFID;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Locafi.Client.Model.Dto.Reader
{
    public class ClusterTagDto : IRfidTag
    {
        public string TagNumber { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public TagType TagType { get; set; }
        public int ReadCount { get; set; }  // number of times the tag was read during this inventory/allocation/receive etc
        public double AverageRssi { get; set; } // average RSSI of the tag during this inventory/allocation/receive etc

        public ClusterTagDto()
        {
            TagType = TagType.PassiveRfid;
        }

        public ClusterTagDto(string tagNumber) :this()
        {
            TagNumber = tagNumber;
        }

        public ClusterTagDto(string tagNumber, TagType tagType) : this(tagNumber)
        {
            TagType = tagType;
        }

        public ClusterTagDto(string tagNumber, int readCount, double averageRssi, TagType tagType = TagType.PassiveRfid) : this(tagNumber, tagType)
        {
            ReadCount = readCount;
            AverageRssi = averageRssi;
        }

        public override bool Equals(object obj)
        {
            var tag = obj as ClusterTagDto;
            return tag != null && string.Equals(tag.TagNumber, TagNumber);
        }

        public override int GetHashCode()
        {
            return TagNumber.GetHashCode();
        }
    }
}
