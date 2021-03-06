﻿using System;
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

        public UpdateTemplateDto(TemplateDetailDto dto)
        {
            Id = dto.Id;
            Name = dto.Name;
            TemplateExtendedPropertyList = dto.TemplateExtendedPropertyList.Select(p => new AddTemplateExtendedPropertyDto() { ExtendedPropertyId = p.ExtendedPropertyId }).ToList();
        }
    }
}
