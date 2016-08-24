using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Locafi.Client.Model.Dto.Reasons
{
    public class ReasonSummaryDto : EntityDtoBase
    {
        public ReasonSummaryDto()
        {
            
        }

        public ReasonSummaryDto(ReasonSummaryDto dto):base(dto)
        {
            if (dto == null) return;

            var type = typeof(ReasonSummaryDto);
            var properties = type.GetTypeInfo().DeclaredProperties;
            foreach (var property in properties)
            {
                var value = property.GetValue(dto);
                property.SetValue(this, value);
            }
        }

        public string ReasonNumber { get; set; }

        public string Description { get; set; }

        public override string ToString()
        {
            return Description;
        }
    }
}
