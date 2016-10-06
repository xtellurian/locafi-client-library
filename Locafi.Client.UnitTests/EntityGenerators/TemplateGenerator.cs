using Locafi.Client.Contract.Repo;
using Locafi.Client.Model.Dto.ExtendedProperties;
using Locafi.Client.Model.Dto.Templates;
using Locafi.Client.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.UnitTests.EntityGenerators
{
    public static class TemplateGenerator
    {
        public static async Task<AddTemplateDto> GenerateRandomAddTemplateDto(TemplateFor? templateFor = null, int numExtProps = 0)
        {
            var ran = new Random(DateTime.UtcNow.Millisecond);
            IExtendedPropertyRepo _extPropRepo = WebRepoContainer.ExtendedPropertyRepo;

            var extProps = new List<ExtendedPropertyDetailDto>();

            // set template type
            TemplateFor templateType;
            if (templateFor.HasValue)
                templateType = templateFor.Value;
            else
            {
                Array TemplateForValues = Enum.GetValues(typeof(TemplateFor));
                templateType = (TemplateFor)TemplateForValues.GetValue(ran.Next(TemplateForValues.Length));
            }

            // generate the extended properties to use
            for (var i = 0; i < numExtProps; i++)
            {
                var addDto = ExtendedPropertyGenerator.GenerateRandomAddExtPropDto(templateType);
                var extProp = await _extPropRepo.CreateExtendedProperty(addDto);
                extProps.Add(extProp);
            }

            // build the add dto
            var addTemplateDto = new AddTemplateDto()
            {
                Name = "Random Template - " + ran.Next().ToString(),
                TemplateType = templateType,
                TemplateExtendedPropertyList = extProps.Select(p => new AddTemplateExtendedPropertyDto() { ExtendedPropertyId = p.Id, IsRequired = false }).ToList()
            };

            return addTemplateDto;
        }

        public static async Task<AddTemplateDto> GenerateAddTemplateDtoWithFullExtProps(TemplateFor templateFor)
        {
            var ran = new Random(DateTime.UtcNow.Millisecond);
            IExtendedPropertyRepo _extPropRepo = WebRepoContainer.ExtendedPropertyRepo;

            var extProps = new List<ExtendedPropertyDetailDto>();

            // set template type
            TemplateFor templateType = templateFor;

            // generate the extended properties to use
            Array ExtPropTypes = Enum.GetValues(typeof(TemplateDataTypes));
            foreach(TemplateDataTypes propType in ExtPropTypes)
            {
                var addDto = new AddExtendedPropertyDto()
                {
                    Name = propType.ToString() + " Ext Prop",
                    Description = propType.ToString() + " Ext Prop Description",
                    DataType = propType,
                    TemplateType = templateFor
                };
                var extProp = await _extPropRepo.CreateExtendedProperty(addDto);
                extProps.Add(extProp);
            }

            // build the add dto
            var addTemplateDto = new AddTemplateDto()
            {
                Name = "Random Template - " + ran.Next().ToString(),
                TemplateType = templateType,
                TemplateExtendedPropertyList = extProps.Select(p => new AddTemplateExtendedPropertyDto() { ExtendedPropertyId = p.Id, IsRequired = false }).ToList()
            };

            return addTemplateDto;
        }
    }
}
