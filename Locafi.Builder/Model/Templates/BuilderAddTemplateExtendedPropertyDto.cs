﻿using Locafi.Client.Model.Dto.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Builder.Model.Templates
{
    public class BuilderAddTemplateExtendedPropertyDto : AddTemplateExtendedPropertyDto
    {
        public string ExtendedPropertyName { get; set; }
    }
}
