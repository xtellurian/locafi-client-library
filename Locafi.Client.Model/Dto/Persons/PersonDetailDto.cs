using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Persons
{
    public class PersonDetailDto : PersonSummaryDto
    {
        public Guid? TagId { get; set; }

        public string EmailAddress { get; set; }

        public IList<ReadEntityExtendedPropertyDto> PersonExtendedPropeertyList { get; set; }

        public PersonDetailDto()
        {
            PersonExtendedPropeertyList = new List<ReadEntityExtendedPropertyDto>();
        }
    }
}
