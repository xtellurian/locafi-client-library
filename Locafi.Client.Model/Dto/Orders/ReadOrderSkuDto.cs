using Locafi.Client.Model.Dto.Skus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Orders
{
    public class ReadOrderSkuDto : SkuSummaryDto
    {
        public int RequiredCount { get; set; }

        public IList<string> AllocatedTagNumbers { get; set; }

        public IList<string> ReceivedTagNumbers { get; set; }

        public ReadOrderSkuDto(SkuSummaryDto dto) : base(dto)
        {
            AllocatedTagNumbers = new List<string>();
            ReceivedTagNumbers = new List<string>();

            if (dto == null) return;

            var type = typeof(SkuSummaryDto);
            var properties = type.GetTypeInfo().DeclaredProperties;
            foreach (var property in properties)
            {
                var value = property.GetValue(dto);
                property.SetValue(this, value);
            }
        }

    }
}
