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
        public Guid? TagId { get; set; }

        public string EmailAddress { get; set; }

        public IList<ReadEntityExtendedPropertyDto> PersonExtendedPropeertyList { get; set; }
    }
}
