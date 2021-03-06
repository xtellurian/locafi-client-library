﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Locafi.Client.Model.Dto.Reasons
{
    public class ReasonDetailDto : ReasonSummaryDto
    {
        public ReasonDetailDto()
        {
            
        }

        public ReasonDetailDto(ReasonDetailDto dto):base(dto)
        {
            if (dto == null) return;

            var type = typeof(ReasonDetailDto);
            var properties = type.GetTypeInfo().DeclaredProperties;
            foreach (var property in properties)
            {
                var value = property.GetValue(dto);
                property.SetValue(this, value);
            }
        }
    }
}
