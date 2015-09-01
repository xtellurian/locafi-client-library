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

        public string TagNumber { get; set; }

        public string TagType { get; set; } // TagType Enum

        public string GivenName { get; set; }

        public string Surname { get; set; }

        public string EmailAddress { get; set; }

        public IList<WriteEntityExtendedPropertyDto> PersonExtendedPropertyList { get; set; }

        public AddPersonDto()
        {
            PersonExtendedPropertyList = new List<WriteEntityExtendedPropertyDto>();
        }
    }
}
