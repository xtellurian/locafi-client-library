using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Templates
{
    public class AddTemplateExtendedPropertyDto
    {
        public Guid ExtendedPropertyId { get; set; }

        public bool IsRequired { get; set; }
    }
}
