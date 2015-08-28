using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Templates
{
    public class AddTemplateDto
    {
        public AddTemplateDto()
        {
            TemplateExtendedPropertyList = new List<AddTemplateExtendedPropertyDto>();
        }

        public string Name { get; set; }

        public string TemplateType { get; set; }

        // ReSharper disable once MemberCanBePrivate.Global
        public IList<AddTemplateExtendedPropertyDto> TemplateExtendedPropertyList { get; set; }
    }
}
