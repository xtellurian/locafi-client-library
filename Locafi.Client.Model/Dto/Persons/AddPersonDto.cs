using Locafi.Client.Model.Dto.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Persons
{
    public class AddPersonDto
    {
        public Guid TemplateId { get; set; }

        public string GivenName { get; set; }

        public string Surname { get; set; }

        public Guid? PlaceId { get; set; }

        public string Email { get; set; }

        public IList<WriteTagDto> PersonTagList { get; set; }

        public IList<WriteEntityExtendedPropertyDto> PersonExtendedPropertyList { get; set; }

        public AddPersonDto()
        {
            PersonTagList = new List<WriteTagDto>();
            PersonExtendedPropertyList = new List<WriteEntityExtendedPropertyDto>();
        }
    }
}
