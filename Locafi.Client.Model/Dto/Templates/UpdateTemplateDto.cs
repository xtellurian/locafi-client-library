using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Templates
{
    public class UpdateTemplateDto
    {
        public UpdateTemplateDto()
        {
            TemplateExtendedPropertyList = new List<AddTemplateExtendedPropertyDto>();
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public IList<AddTemplateExtendedPropertyDto> TemplateExtendedPropertyList { get; set; }

    }
}
