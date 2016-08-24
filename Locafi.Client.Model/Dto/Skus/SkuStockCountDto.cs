using Locafi.Client.Model.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Skus
{
    public class SkuStockCountDto
    {
        public Guid SkuId { get; set; }

        public string SkuName { get; set; }

        public Guid TemplateId { get; set; }

        public string TemplateName { get; set; }

        public bool IsSgtin { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ItemStateType? ItemStatus { get; set; }

        public int ItemCount { get; set; }

        public Guid PlaceId { get; set; }

        public string PlaceName { get; set; }
    }
}
