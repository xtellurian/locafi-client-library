using System;
using System.Reflection;
using Locafi.Client.Model.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Locafi.Client.Model.Dto.Items
{
    public class ItemSummaryDto : EntityDtoBase
    {
        public ItemSummaryDto()
        {
            
        }
        public ItemSummaryDto(ItemSummaryDto dto) : base(dto)
        {
            var type = typeof(ItemSummaryDto);
            var properties = type.GetTypeInfo().DeclaredProperties;
            foreach (var property in properties)
            {
                var value = property.GetValue(dto);
                property.SetValue(this, value);
            }
        }

        public string Name { get; set; }
        public Guid SkuId { get; set; }
        public string SkuName { get; set; }
        public Guid PlaceId { get; set; }
        public string PlaceName { get; set; }
        public Guid? TagId { get; set; }
        private string _tagNumber;
        public string TagNumber {
            get { return _tagNumber; }
            set { _tagNumber = value.ToUpper(); }
            }
        [JsonConverter(typeof(StringEnumConverter))]
        public TagType? TagType { get; set; }

        public Guid? PersonId { get; set; }
        public string PersonName { get; set; }

        public string Thumbnail { get; set; }
    }
}