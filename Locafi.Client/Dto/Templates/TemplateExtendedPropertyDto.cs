using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Templates
{
    public class TemplateExtendedPropertyDto
    {
        public Guid Id { get; set; }

        public Guid ExtendedPropertyId { get; set; }

        public Guid TemplateId { get; set; }

        public string ExtendedPropertyName { get; set; }

        public string ExtendedPropertyDescription { get; set; }

        public bool TemplateExtendedPropertyIsRequired { get; set; }

        public string ExtendedPropertyDataType { get; set; }

    }
}
