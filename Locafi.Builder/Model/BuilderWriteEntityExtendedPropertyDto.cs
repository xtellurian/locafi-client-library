using Locafi.Client.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Builder.Model
{
    public class BuilderWriteEntityExtendedPropertyDto : WriteEntityExtendedPropertyDto
    {
        public string ExtendedPropertyName { get; set; }
    }
}
