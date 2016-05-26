using Locafi.Client.Model.Dto.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Persons
{
    public class PersonDetailDto : PersonSummaryDto
    {
        public PersonDetailDto()
        {
        }

        public PersonDetailDto(PersonDetailDto dto)
        {
            var type = typeof(PersonDetailDto);
            var properties = type.GetTypeInfo().DeclaredProperties;
            foreach (var property in properties)
            {
                var value = property.GetValue(dto);
                property.SetValue(this, value);
            }
        }
        public IList<TagDetailDto> PersonTagList { get; set; }

        public IList<ReadEntityExtendedPropertyDto> PersonExtendedPropertyList { get; set; }
    }
}
