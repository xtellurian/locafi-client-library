using Locafi.Client.Contract.Repo;
using Locafi.Client.Model.Dto.Skus;
using Locafi.Client.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.UnitTests.EntityGenerators
{
    public static class SkuGenerator
    {
        public static async Task<AddSkuDto> GenerateSgtinSkuDto(Guid? templateId = null)
        {
            ITemplateRepo _templateRepo = WebRepoContainer.TemplateRepo;

            var ran = new Random(DateTime.UtcNow.Millisecond);

            if (!templateId.HasValue)
            {
                var templates = await _templateRepo.GetTemplatesForType(TemplateFor.Item);
                var template = templates.Items.ElementAt(ran.Next(templates.Items.Count()));
                templateId = template.Id;
            }
            var templateDetail = await _templateRepo.GetById(templateId.Value);

            var skuNo = ran.Next().ToString();
            var companyPrefix = ran.Next(100000,999999).ToString();
            var itemReference = ran.Next(9999).ToString().PadLeft(7,'0');
            var name = "Sku - " + templateDetail.Name + " - " + ran.Next().ToString();
            var description = name + " - Description";

            var addSku = new AddSkuDto(templateDetail)
            {
                CompanyPrefix = companyPrefix,
                Description = description,
                ItemReference = itemReference,
                ItemTemplateId = templateDetail.Id,
                Name = name,
                SkuNumber = skuNo,
                IsSgtin = true
            };

            return addSku;
        }

        public static async Task<AddSkuDto> GenerateTagPrefixSkuDto(Guid? templateId = null)
        {
            ITemplateRepo _templateRepo = WebRepoContainer.TemplateRepo;

            var ran = new Random(DateTime.UtcNow.Millisecond);

            if (!templateId.HasValue)
            {
                var templates = await _templateRepo.GetTemplatesForType(TemplateFor.Item);
                var template = templates.Items.ElementAt(ran.Next(templates.Items.Count()));
                templateId = template.Id;
            }
            var templateDetail = await _templateRepo.GetById(templateId.Value);

            var skuNo = ran.Next().ToString();
            var tagPrefix = ran.Next(9999).ToString().PadLeft(4);
            var name = "Sku - " + templateDetail.Name + " - " + ran.Next().ToString();
            var description = name + " - Description";

            var addSku = new AddSkuDto(templateDetail)
            {
                CustomPrefix = tagPrefix,
                Description = description,
                ItemTemplateId = templateDetail.Id,
                Name = name,
                SkuNumber = skuNo
            };

            return addSku;
        }

        public static async Task<AddSkuDto> GeneratePlainSkuDto(Guid? templateId = null)
        {
            ITemplateRepo _templateRepo = WebRepoContainer.TemplateRepo;

            var ran = new Random(DateTime.UtcNow.Millisecond);

            if (!templateId.HasValue)
            {
                var templates = await _templateRepo.GetTemplatesForType(TemplateFor.Item);
                var template = templates.Items.ElementAt(ran.Next(templates.Items.Count()));
                templateId = template.Id;
            }
            var templateDetail = await _templateRepo.GetById(templateId.Value);

            var skuNo = ran.Next().ToString();
            var name = "Sku - " + templateDetail.Name + " - " + ran.Next().ToString();
            var description = name + " - Description";

            var addSku = new AddSkuDto(templateDetail)
            {
                Description = description,
                ItemTemplateId = templateDetail.Id,
                Name = name,
                SkuNumber = skuNo
            };

            return addSku;
        }
    }
}
