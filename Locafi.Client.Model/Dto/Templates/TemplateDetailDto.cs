using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Templates
{
    public class TemplateDetailDto : TemplateSummaryDto
    {

        public IList<TemplateExtendedPropertyDto> TemplateExtendedPropertyList { get; set; }

        public TemplateDetailDto()
        {
            TemplateExtendedPropertyList = new List<TemplateExtendedPropertyDto>();
        }
    }
}
