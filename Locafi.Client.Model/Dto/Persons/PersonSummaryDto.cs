using Locafi.Client.Model.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Persons
{
    public class PersonSummaryDto : EntityDtoBase
    {
        public PersonSummaryDto()
        {
            
        }

        public PersonSummaryDto(PersonSummaryDto dto): base(dto)
        {
            var type = typeof(PersonSummaryDto);
            var properties = type.GetTypeInfo().DeclaredProperties;
            foreach (var property in properties)
            {
                var value = property.GetValue(dto);
                property.SetValue(this, value);
            }
        }
        public Guid TemplateId { get; set; }

        public string TemplateName { get; set; }

        public string TagNumber { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public TagType? TagType { get; set; } // TagType Enum

        public string GivenName { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public Guid? PlaceId { get; set; }

        public string PlaceName { get; set; }
    }
}
