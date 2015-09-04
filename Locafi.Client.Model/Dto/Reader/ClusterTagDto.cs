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

        public double Rssi { get; set; }
    }
}
