using Locafi.Client.Model.Dto.Persons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Builder.Model.Persons
{
    public class BuilderAddPersonDto : AddPersonDto
    {
        public string TemplateName { get; set; }

        public IList<BuilderWriteEntityExtendedPropertyDto> BuilderPersonExtendedPropertyList { get; set; }

        public BuilderAddPersonDto()
        {
            BuilderPersonExtendedPropertyList = new List<BuilderWriteEntityExtendedPropertyDto>();
        }
    }
}
