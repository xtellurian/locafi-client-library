using Locafi.Client.Model.Dto.ExtendedProperties;
using Locafi.Client.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.UnitTests.EntityGenerators
{
    public static class ExtendedPropertyGenerator
    {
        public static AddExtendedPropertyDto GenerateRandomAddExtPropDto(TemplateFor? templateFor = null)
        {
            var ran = new Random(DateTime.UtcNow.Millisecond);

            Array TemplateDataTypeValues = Enum.GetValues(typeof(TemplateDataTypes));
            Array TemplateForValues = Enum.GetValues(typeof(TemplateFor));

            var extPropDto = new AddExtendedPropertyDto()
            {
                Name = "Random Ext Prop - " + ran.Next().ToString(),
                Description = "Random Ext Prop Description",
                DataType = (TemplateDataTypes)TemplateDataTypeValues.GetValue(ran.Next(TemplateDataTypeValues.Length)),
                TemplateType = templateFor ?? (TemplateFor)TemplateForValues.GetValue(ran.Next(TemplateForValues.Length))
            };

            return extPropDto;
        }
    }
}
